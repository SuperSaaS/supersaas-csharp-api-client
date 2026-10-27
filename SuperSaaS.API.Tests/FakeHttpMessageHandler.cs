using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SuperSaaS.API.Tests
{
    /// <summary>
    /// Records every request (including its body, read before the client disposes it) and answers with a
    /// configurable response or exception.
    /// </summary>
    public sealed class FakeHttpMessageHandler : HttpMessageHandler
    {
        public sealed class RecordedRequest
        {
            public RecordedRequest(HttpRequestMessage message, byte[]? body)
            {
                Message = message;
                Body = body;
            }

            public HttpRequestMessage Message { get; }

            public byte[]? Body { get; }
        }

        public List<RecordedRequest> Requests { get; } = new List<RecordedRequest>();

        public Func<HttpRequestMessage, HttpResponseMessage> Respond { get; set; } =
            _ => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("") };

        public RecordedRequest LastRequest => Requests[Requests.Count - 1];

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            byte[]? body = request.Content?.ReadAsByteArrayAsync(cancellationToken).GetAwaiter().GetResult();
            Requests.Add(new RecordedRequest(request, body));
            return Respond(request);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request, cancellationToken));
        }
    }
}
