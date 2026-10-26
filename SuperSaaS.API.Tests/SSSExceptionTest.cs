using System;
using System.Collections.Generic;
using NUnit.Framework;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Tests
{
    [TestFixture()]
    public class SSSExceptionTest
    {
        [Test()]
        public void ExistingConstructorsUnchangedTest()
        {
            SSSException e = new SSSException("boom");
            Assert.AreEqual("boom", e.Message);
            Assert.IsNull(e.StatusCode);
            Assert.AreEqual(0, e.Errors.Count);

            Exception inner = new Exception("inner");
            SSSException e2 = new SSSException("outer", inner);
            Assert.AreEqual("outer", e2.Message);
            Assert.AreSame(inner, e2.InnerException);
            Assert.AreEqual(0, e2.Errors.Count);
            Assert.AreEqual(0, new SSSException().Errors.Count);
        }

        [Test()]
        public void StatusAndErrorsTest()
        {
            string body = "{\"errors\":[{\"status\":\"404\",\"title\":\"Not found\"}]}";
            SSSException e = new SSSException(body, 404, SSSException.ParseErrors(body));
            Assert.AreEqual(body, e.Message);
            Assert.AreEqual(404, e.StatusCode);
            Assert.AreEqual(1, e.Errors.Count);
            Assert.AreEqual("404", e.Errors[0].Status);
            Assert.AreEqual("Not found", e.Errors[0].Title);
            Assert.IsNull(e.Errors[0].Code);
        }

        [Test()]
        public void ParseErrorsVariantsTest()
        {
            IList<ApiError> errors = SSSException.ParseErrors("{\"errors\":[{\"code\":\"taken\",\"title\":\"Already taken\"}]}");
            Assert.AreEqual("taken", errors[0].Code);
            Assert.AreEqual("Already taken", errors[0].Title);

            errors = SSSException.ParseErrors("{\"errors\":[\"Some message\"]}");
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Some message", errors[0].Title);

            errors = SSSException.ParseErrors("{\"errors\":[{\"status\":422}]}");
            Assert.AreEqual("422", errors[0].Status);
        }

        [Test()]
        public void ParseErrorsNeverThrowsTest()
        {
            foreach (string body in new string[] { null, "", "<html>Bad</html>", "[1,2]", "{}", "{\"errors\":\"x\"}", "{\"errors\":{\"a\":1}}", "{\"errors\":[1,null]}" })
            {
                IList<ApiError> errors = null;
                Assert.DoesNotThrow(() => { errors = SSSException.ParseErrors(body); });
                Assert.AreEqual(0, errors.Count, body);
            }
        }
    }
}
