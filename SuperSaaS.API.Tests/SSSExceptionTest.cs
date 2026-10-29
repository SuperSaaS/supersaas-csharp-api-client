using System;
using System.Collections.Generic;
using NUnit.Framework;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Tests
{
    [TestFixture]
    public class SSSExceptionTest
    {
        [Test]
        public void ExistingConstructorsUnchanged()
        {
            var e = new SSSException("boom");
            Assert.That(e.Message, Is.EqualTo("boom"));
            Assert.That(e.StatusCode, Is.Null);
            Assert.That(e.Errors, Is.Empty);

            var inner = new Exception("inner");
            var e2 = new SSSException("outer", inner);
            Assert.That(e2.Message, Is.EqualTo("outer"));
            Assert.That(e2.InnerException, Is.SameAs(inner));
            Assert.That(e2.Errors, Is.Empty);
            Assert.That(new SSSException().Errors, Is.Empty);
        }

        [Test]
        public void StatusAndErrors()
        {
            string body = "{\"errors\":[{\"status\":\"404\",\"title\":\"Not found\"}]}";
            var e = new SSSException(body, 404, SSSException.ParseErrors(body));
            Assert.That(e.Message, Is.EqualTo(body));
            Assert.That(e.StatusCode, Is.EqualTo(404));
            Assert.That(e.Errors, Has.Count.EqualTo(1));
            Assert.That(e.Errors[0].Status, Is.EqualTo("404"));
            Assert.That(e.Errors[0].Title, Is.EqualTo("Not found"));
            Assert.That(e.Errors[0].Code, Is.Null);
        }

        [Test]
        public void NullErrorsBecomeEmptyList()
        {
            Assert.That(new SSSException("x", 500, null!).Errors, Is.Empty);
        }

        [Test]
        public void ParseErrorsVariants()
        {
            IList<ApiError> errors =
                SSSException.ParseErrors("{\"errors\":[{\"code\":\"taken\",\"title\":\"Already taken\"}]}");
            Assert.That(errors[0].Code, Is.EqualTo("taken"));
            Assert.That(errors[0].Title, Is.EqualTo("Already taken"));

            errors = SSSException.ParseErrors("{\"errors\":[\"Some message\"]}");
            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0].Title, Is.EqualTo("Some message"));

            errors = SSSException.ParseErrors("{\"errors\":[{\"status\":422}]}");
            Assert.That(errors[0].Status, Is.EqualTo("422"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("<html>Bad</html>")]
        [TestCase("[1,2]")]
        [TestCase("{}")]
        [TestCase("{\"errors\":\"x\"}")]
        [TestCase("{\"errors\":{\"a\":1}}")]
        [TestCase("{\"errors\":[1,null]}")]
        [TestCase("{\"errors\":[")]
        public void ParseErrorsNeverThrows(string? body)
        {
            IList<ApiError>? errors = null;
            Assert.DoesNotThrow(() => errors = SSSException.ParseErrors(body!));
            Assert.That(errors, Is.Empty);
        }
    }
}
