using System;
using System.Text.RegularExpressions;

#nullable enable

namespace SuperSaaS.API.Api
{
    public class BaseApi
    {
        private static readonly Regex INTEGER_REGEX = new Regex(@"^\d+$");
        private static readonly Regex DATETIME_REGEX = new Regex(@"^\d{4}-\d{1,2}-\d{1,2}\s\d{1,2}:\d{1,2}:\d{1,2}$");
        private static readonly Regex PROMOTION_REGEX = new Regex(@"^[0-9a-zA-Z]+$");
        protected readonly IClient Client;

        public BaseApi(IClient client)
        {
            this.Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        protected int ValidateId(object? value)
        {
            if (value is int intValue) return intValue;
            if (value is string strValue && INTEGER_REGEX.IsMatch(strValue))
                return int.Parse(strValue);
            throw new SSSException($"Invalid id parameter: {value}. Provide an integer value.");
        }

        protected object? ValidateUser(object? value)
        {
            if (value == null) return null;
            if (value is int || value is string) return value;
            throw new SSSException($"Invalid user id parameter: {value}.");
        }

        protected int ValidateNumber(object value)
        {
            return ValidateId(value);
        }

        protected string ValidateName(string? value)
        {
            if (string.IsNullOrEmpty(value)) throw new SSSException("Required parameter name is missing.");
            return value;
        }

        protected object ValidatePresent(object? value)
        {
            return value ?? throw new SSSException("Required parameter is missing.");
        }

        protected string ValidateNotfound(string value)
        {
            if (!string.Equals(value, "error", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(value, "ignore", StringComparison.OrdinalIgnoreCase))
                throw new SSSException("Required parameter notfound can only be 'error' or 'ignore'.");
            return value;
        }

        protected string ValidatePromotion(string value)
        {
            if (string.IsNullOrEmpty(value) || !PROMOTION_REGEX.IsMatch(value))
                throw new SSSException("Required parameter promotional code not found or contains other than alphanumeric characters.");
            return value;
        }

        protected string ValidateDuplicate(string value)
        {
            if (!string.Equals(value, "ignore", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(value, "raise", StringComparison.OrdinalIgnoreCase))
                throw new SSSException("Required parameter duplicate can only be 'ignore' or 'raise'.");
            return value;
        }

        protected string ValidateDateTime(object? value)
        {
            try
            {
                if (value is string strValue && DATETIME_REGEX.IsMatch(strValue)) return strValue;
                if (value is DateTime dateTime) return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                throw new ArgumentException();
            }
            catch (ArgumentException)
            {
                throw new SSSException(
                    $"Invalid datetime parameter: {value}. Provide a DateTime object or formatted 'YYYY-DD-MM HH:MM:SS' string.");
            }
        }

        protected T ValidateOptions<T>(T value, T[] options) where T : notnull
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (!Array.Exists(options, x => x!.Equals(value)))
                throw new SSSException($"Invalid option parameter: {value}. Must be one of {string.Join(", ", options)}.");
            return value;
        }
    }
}