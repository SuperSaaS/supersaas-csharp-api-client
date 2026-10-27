using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SuperSaaS.API.Tests
{
    /// <summary>
    /// Renders the externally visible surface (public and protected members) of an assembly as stable text,
    /// one line per member, so it can be compared with a checked-in snapshot.
    /// </summary>
    public static class ApiSurface
    {
        private const BindingFlags DeclaredMembers =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static |
            BindingFlags.DeclaredOnly;

        public static string Describe(Assembly assembly)
        {
            var lines = new List<string> { "assembly " + assembly.GetName().Name };
            foreach (var type in assembly.GetTypes().Where(IsVisible).OrderBy(t => t.FullName, StringComparer.Ordinal))
            {
                lines.Add(DescribeType(type));
                lines.AddRange(DescribeMembers(type).OrderBy(l => l, StringComparer.Ordinal));
            }
            return string.Join("\n", lines) + "\n";
        }

        private static bool IsVisible(Type type)
        {
            if (type.IsNested)
            {
                return IsVisible(type.DeclaringType!) &&
                       (type.IsNestedPublic || type.IsNestedFamily || type.IsNestedFamORAssem);
            }
            return type.IsPublic;
        }

        private static bool IsVisible(MethodBase? method)
        {
            return method != null && (method.IsPublic || method.IsFamily || method.IsFamilyOrAssembly);
        }

        private static bool IsVisible(FieldInfo field)
        {
            return field.IsPublic || field.IsFamily || field.IsFamilyOrAssembly;
        }

        private static string DescribeType(Type type)
        {
            var sb = new StringBuilder("type ");
            if (type.IsEnum)
            {
                sb.Append("enum ");
            }
            else if (type.IsInterface)
            {
                sb.Append("interface ");
            }
            else if (type.IsValueType)
            {
                sb.Append("struct ");
            }
            else
            {
                if (type.IsAbstract && type.IsSealed)
                {
                    sb.Append("static ");
                }
                else if (type.IsAbstract)
                {
                    sb.Append("abstract ");
                }
                else if (type.IsSealed)
                {
                    sb.Append("sealed ");
                }
                sb.Append("class ");
            }
            sb.Append(Name(type));
            var bases = new List<string>();
            if (type.BaseType != null && type.BaseType != typeof(object) && type.BaseType != typeof(ValueType) &&
                type.BaseType != typeof(Enum))
            {
                bases.Add(Name(type.BaseType));
            }
            if (type.IsEnum)
            {
                bases.Add(Name(Enum.GetUnderlyingType(type)));
            }
            else
            {
                var inherited = type.BaseType?.GetInterfaces() ?? Type.EmptyTypes;
                bases.AddRange(type.GetInterfaces().Where(i => !inherited.Contains(i)).Select(Name)
                    .OrderBy(n => n, StringComparer.Ordinal));
            }
            if (bases.Count > 0)
            {
                sb.Append(" : ").Append(string.Join(", ", bases));
            }
            return sb.ToString();
        }

        private static IEnumerable<string> DescribeMembers(Type type)
        {
            string prefix = "  " + Name(type) + ".";

            foreach (var field in type.GetFields(DeclaredMembers).Where(IsVisible))
            {
                if (type.IsEnum)
                {
                    if (field.IsStatic)
                    {
                        object raw = Convert.ChangeType(field.GetRawConstantValue()!, Enum.GetUnderlyingType(type),
                            CultureInfo.InvariantCulture);
                        yield return prefix + field.Name + " = " + Literal(raw);
                    }
                    continue;
                }
                string modifiers = Access(field.IsPublic) + (field.IsLiteral ? "const " :
                    (field.IsStatic ? "static " : "") + (field.IsInitOnly ? "readonly " : ""));
                string value = field.IsLiteral ? " = " + Literal(field.GetRawConstantValue()) : "";
                yield return prefix + "field " + modifiers + Name(field.FieldType) + " " + field.Name + value;
            }

            foreach (var property in type.GetProperties(DeclaredMembers))
            {
                var getter = property.GetGetMethod(true);
                var setter = property.GetSetMethod(true);
                if (!IsVisible(getter) && !IsVisible(setter))
                {
                    continue;
                }
                var accessors = new List<string>();
                if (IsVisible(getter))
                {
                    accessors.Add(Access(getter!.IsPublic) + "get;");
                }
                if (IsVisible(setter))
                {
                    accessors.Add(Access(setter!.IsPublic) + "set;");
                }
                var any = IsVisible(getter) ? getter! : setter!;
                string indexer = property.GetIndexParameters().Length > 0
                    ? "[" + Parameters(property.GetIndexParameters()) + "]"
                    : "";
                yield return prefix + "property " + Modifiers(any) + Name(property.PropertyType) + " " +
                             property.Name + indexer + " { " + string.Join(" ", accessors) + " }";
            }

            foreach (var evt in type.GetEvents(DeclaredMembers).Where(e => IsVisible(e.AddMethod)))
            {
                yield return prefix + "event " + Modifiers(evt.AddMethod!) + Name(evt.EventHandlerType!) + " " +
                             evt.Name;
            }

            foreach (var ctor in type.GetConstructors(DeclaredMembers).Where(IsVisible))
            {
                if (type.IsValueType && ctor.GetParameters().Length == 0)
                {
                    continue;
                }
                yield return prefix + "ctor " + Access(ctor.IsPublic) + (ctor.IsStatic ? "static " : "") +
                             Name(type) + "(" + Parameters(ctor.GetParameters()) + ")";
            }

            foreach (var method in type.GetMethods(DeclaredMembers).Where(IsVisible))
            {
                if (method.IsSpecialName || type.IsEnum)
                {
                    continue;
                }
                string generic = method.IsGenericMethodDefinition
                    ? "<" + string.Join(", ", method.GetGenericArguments().Select(a => a.Name)) + ">"
                    : "";
                yield return prefix + "method " + Modifiers(method) + Name(method.ReturnType) + " " + method.Name +
                             generic + "(" + Parameters(method.GetParameters()) + ")";
            }
        }

        private static string Access(bool isPublic)
        {
            return isPublic ? "public " : "protected ";
        }

        private static string Modifiers(MethodInfo method)
        {
            var sb = new StringBuilder(Access(method.IsPublic));
            if (method.IsStatic)
            {
                sb.Append("static ");
            }
            else if (method.DeclaringType!.IsInterface)
            {
                // Interface members are implicitly abstract; nothing to add.
            }
            else if (method.IsAbstract)
            {
                sb.Append("abstract ");
            }
            else if (method.IsVirtual && !method.IsFinal)
            {
                sb.Append(method.GetBaseDefinition() == method ? "virtual " : "override ");
            }
            else if (method.IsVirtual && method.GetBaseDefinition() != method)
            {
                sb.Append("sealed override ");
            }
            return sb.ToString();
        }

        private static string Parameters(ParameterInfo[] parameters)
        {
            return string.Join(", ", parameters.Select(p =>
            {
                string modifier = p.IsOut ? "out " : p.ParameterType.IsByRef ? "ref " : "";
                string optional = p.HasDefaultValue ? " = " + Literal(p.DefaultValue) : "";
                return modifier + Name(p.ParameterType) + " " + p.Name + optional;
            }));
        }

        private static string Literal(object? value)
        {
            switch (value)
            {
                case null:
                    return "null";
                case string s:
                    return "\"" + s + "\"";
                case bool b:
                    return b ? "true" : "false";
                case IFormattable f:
                    return f.ToString(null, CultureInfo.InvariantCulture);
                default:
                    return value.ToString() ?? "";
            }
        }

        private static string Name(Type type)
        {
            if (type.IsByRef)
            {
                return Name(type.GetElementType()!);
            }
            if (type.IsArray)
            {
                return Name(type.GetElementType()!) + "[" + new string(',', type.GetArrayRank() - 1) + "]";
            }
            if (type.IsGenericParameter)
            {
                return type.Name;
            }
            string name = type.IsNested ? Name(type.DeclaringType!) + "+" + type.Name : type.Namespace + "." + type.Name;
            if (type.IsGenericType)
            {
                name = name.Substring(0, name.IndexOf('`')) + "<" +
                       string.Join(", ", type.GetGenericArguments().Select(Name)) + ">";
            }
            return name;
        }
    }
}
