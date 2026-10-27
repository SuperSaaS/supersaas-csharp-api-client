using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using NUnit.Framework;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Tests
{
    /// <summary>
    /// Exercises the HTTP layer of <see cref="Client"/> through a fake message Handler.
    /// </summary>
    [TestFixture]
    public class HttpLayerTest : FakeHttpFixture
    {
        [Test]
        public void UsesTheMatchingHttpMethod()
        {
            Client.Get<object>("/test");
            Assert.That(LastMessage.Method, Is.EqualTo(System.Net.Http.HttpMethod.Get));
            Client.Post<object>("/test");
            Assert.That(LastMessage.Method, Is.EqualTo(System.Net.Http.HttpMethod.Post));
            Client.Put<object>("/test");
            Assert.That(LastMessage.Method, Is.EqualTo(System.Net.Http.HttpMethod.Put));
            Client.Delete<object>("/test");
            Assert.That(LastMessage.Method, Is.EqualTo(System.Net.Http.HttpMethod.Delete));
        }

        [Test]
        public void BuildsUrlAndEscapesQueryKeysAndValues()
        {
            Client.Get<object>("/users", new JsonArgs { { "a key", "x&y=z" }, { "name", "Zoë" } });
            Assert.That(LastMessage.RequestUri!.AbsoluteUri,
                Is.EqualTo(Host + "/api/users.json?a%20key=x%26y%3Dz&name=Zo%C3%AB"));
        }

        [Test]
        public void OmitsQueryStringWhenThereAreNoParameters()
        {
            Client.Get<object>("/users", new JsonArgs());
            Assert.That(LastMessage.RequestUri!.AbsoluteUri, Is.EqualTo(Host + "/api/users.json"));
        }

        [Test]
        public void SendsJsonBodyAsUtf8()
        {
            var data = new NestedJsonArgs { { "user", new JsonArgs { { "full_name", "Zoë Ångström" } } } };
            Client.Post<object>("/users", data);

            byte[] expected = Encoding.UTF8.GetBytes("{\"user\":{\"full_name\":\"Zoë Ångström\"}}");
            Assert.That(Handler.LastRequest.Body, Is.EqualTo(expected));
            Assert.That(LastMessage.Content!.Headers.ContentType!.MediaType, Is.EqualTo("application/json"));
            Assert.That(LastMessage.Content.Headers.ContentType.CharSet, Is.EqualTo("utf-8"));
        }

        [Test]
        public void SendsNoBodyWithoutPostData()
        {
            Client.Delete<object>("/users/1");
            Assert.That(Handler.LastRequest.Body, Is.Null);
        }

        [Test]
        public void SendsUtf8BasicAuthHeader()
        {
            Client.AccountName = "accöunt";
            Client.Get<object>("/test");

            Assert.That(LastMessage.Headers.Authorization!.Scheme, Is.EqualTo("Basic"));
            string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(LastMessage.Headers.Authorization.Parameter!));
            Assert.That(decoded, Is.EqualTo("accöunt:secret"));
        }

        [Test]
        public void SendsUserAgentAndAcceptHeaders()
        {
            Client.Get<object>("/test");

            Assert.That(string.Join(" ", LastMessage.Headers.GetValues("User-Agent")),
                Is.EqualTo("SSS/" + Client.VERSION + " DOTNET/" + Environment.Version + " API/" + Client.API_VERSION));
            Assert.That(LastMessage.Headers.Accept.ToString(), Is.EqualTo("application/json"));
        }

        [Test]
        public void DeserializesResponseBody()
        {
            RespondWith(HttpStatusCode.OK, "{\"id\":42,\"name\":\"Zoë\",\"unknown\":null}");
            User user = Client.Get<User>("/users/42");
            Assert.That(user.id, Is.EqualTo(42));
            Assert.That(user.name, Is.EqualTo("Zoë"));
        }

        [Test]
        public void ReturnsDefaultForEmptyResponseBody()
        {
            RespondWith(HttpStatusCode.OK, "");
            Assert.That(Client.Get<User>("/users/42"), Is.Null);
        }

        [Test]
        public void ThrowsSSSExceptionWithResponseBodyOnHttpError()
        {
            RespondWith(HttpStatusCode.UnprocessableEntity, "{\"errors\":[{\"title\":\"Name is invalid\"}]}");

            var ex = Assert.Throws<SSSException>(() => Client.Get<User>("/users/42"));
            Assert.That(ex!.Message, Is.EqualTo("{\"errors\":[{\"title\":\"Name is invalid\"}]}"));
        }

        [Test]
        public void WrapsTransportErrorsKeepingMessageAndInnerException()
        {
            var failure = new HttpRequestException("Connection refused");
            Handler.Respond = _ => throw failure;

            var ex = Assert.Throws<SSSException>(() => Client.Get<User>("/users/42"));
            Assert.That(ex!.Message, Is.EqualTo("Connection refused"));
            Assert.That(ex.InnerException, Is.SameAs(failure));
        }

        [Test]
        public void TestModeDoesNotSendRequests()
        {
            Client.Test = true;
            Assert.That(Client.Post<User>("/users", new NestedJsonArgs()), Is.Null);
            Assert.That(Handler.Requests, Is.Empty);
        }

        [Test]
        public void DefaultTimeoutIsTimeoutSecondsMilliseconds()
        {
            Assert.That(Client.DefaultTimeout, Is.EqualTo(TimeSpan.FromMilliseconds(Client.TIMEOUT_SECONDS)));
            Assert.That(Client.DefaultTimeout, Is.EqualTo(TimeSpan.FromSeconds(10)));
        }

        [TestCase("https://www.supersaas.com/api/bookings/12345.json", 12345)]
        [TestCase("https://www.supersaas.com/api/users/678", 678)]
        [TestCase("/api/bookings/9.json", 9)]
        [TestCase("https://www.supersaas.com/api/users/abc", 0)]
        [TestCase("https://www.supersaas.com/api/bookings/.json", 0)]
        [TestCase("https://www.supersaas.com/api/bookings/99999999999999.json", 0)]
        [TestCase("", 0)]
        [TestCase(null, 0)]
        public void ParsesResourceIdFromLocationHeader(string? location, int expected)
        {
            Assert.That(Client.ParseResourceId(location!), Is.EqualTo(expected));
        }

        [Test]
        public void GetResourceIdFromHeaderReadsLastResponse()
        {
            Assert.That(Client.GetResourceIdFromHeader(), Is.Zero);
            RespondWith(HttpStatusCode.Created, "", Host + "/api/users/321.json");
            Client.Post<User>("/users", new NestedJsonArgs());
            Assert.That(Client.GetResourceIdFromHeader(), Is.EqualTo(321));

            RespondWith(HttpStatusCode.Created, "", Host + "/api/users/not-a-number");
            Client.Post<User>("/users", new NestedJsonArgs());
            Assert.That(Client.GetResourceIdFromHeader(), Is.Zero);
        }
    }
}
