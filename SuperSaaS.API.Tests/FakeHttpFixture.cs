using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;

namespace SuperSaaS.API.Tests
{
    /// <summary>
    /// Base fixture providing a client whose requests go to a <see cref="FakeHttpMessageHandler"/>.
    /// </summary>
    public abstract class FakeHttpFixture
    {
        protected const string Host = "https://test.example";

        protected FakeHttpMessageHandler Handler { get; private set; } = null!;

        protected Client Client { get; private set; } = null!;

        private HttpClient httpClient = null!;

        protected HttpRequestMessage LastMessage => Handler.LastRequest.Message;

        protected string LastUrl => LastMessage.RequestUri!.AbsoluteUri;

        [SetUp]
        public void CreateClient()
        {
            Handler = new FakeHttpMessageHandler();
            httpClient = new HttpClient(Handler);
            Client = new Client("accnt", "secret", Host) { HttpClient = httpClient };
        }

        [TearDown]
        public void DisposeHttpClient()
        {
            httpClient.Dispose();
            Handler.Dispose();
        }

        protected void RespondWith(HttpStatusCode status, string body, string? location = null)
        {
            Handler.Respond = _ =>
            {
                var response = new HttpResponseMessage(status) { Content = new StringContent(body) };
                if (location != null)
                {
                    response.Headers.Location = new Uri(location, UriKind.RelativeOrAbsolute);
                }
                return response;
            };
        }
    }
}
