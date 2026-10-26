using System.Collections.Generic;
using System.Net;
using System.Text;
using NUnit.Framework;
using SuperSaaS.API.Models;
using NetHttpMethod = System.Net.Http.HttpMethod;

namespace SuperSaaS.API.Tests
{
    /// <summary>
    /// Checks Ping, the hook endpoints, the input field list and slot attribute handling through a fake message
    /// handler.
    /// </summary>
    [TestFixture]
    public class HooksTest : FakeHttpFixture
    {
        [Test]
        public void PingReturnsTrueOnNoContent()
        {
            RespondWith(HttpStatusCode.NoContent, "");

            Assert.That(Client.Ping(), Is.True);
            Assert.That(LastMessage.Method, Is.EqualTo(NetHttpMethod.Get));
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/ping.json"));
        }

        [Test]
        public void PingThrowsWithStatusCodeOnUnauthorized()
        {
            RespondWith(HttpStatusCode.Unauthorized, "{\"errors\":[{\"status\":\"401\",\"title\":\"Unauthorized\"}]}");

            var ex = Assert.Throws<SSSException>(() => Client.Ping());
            Assert.That(ex!.StatusCode, Is.EqualTo(401));
            Assert.That(ex.Errors[0].Title, Is.EqualTo("Unauthorized"));
        }

        [Test]
        public void CreatePostsHookParamsInTheQuery()
        {
            RespondWith(HttpStatusCode.Created, "{\"id\":123,\"parent_id\":456}");

            Hook hook = Client.Hooks.Create("https://example.com/hook", "N", 456);

            Assert.That(LastMessage.Method, Is.EqualTo(NetHttpMethod.Post));
            Assert.That(LastUrl, Is.EqualTo(Host +
                "/api/hooks.json?target_url=https%3A%2F%2Fexample.com%2Fhook&event=N&parent_id=456"));
            Assert.That(Handler.LastRequest.Body, Is.Null);
            Assert.That(hook.Id, Is.EqualTo(123));
            Assert.That(hook.ParentId, Is.EqualTo(456));
        }

        [Test]
        public void DeleteSendsHookIdAndParentId()
        {
            Client.Hooks.Delete(456, 123);

            Assert.That(LastMessage.Method, Is.EqualTo(NetHttpMethod.Delete));
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/hooks.json?id=456&parent_id=123"));
        }

        [Test]
        public void InputFieldListOfASchedule()
        {
            RespondWith(HttpStatusCode.OK,
                "[{\"name\":\"full_name\",\"type\":\"text\",\"label\":\"Name\",\"required\":true}]");

            FieldList[] fields = Client.Schedules.InputFieldList(123);

            Assert.That(LastUrl, Is.EqualTo(Host + "/api/input_field_list.json?schedule_id=123"));
            Assert.That(fields, Has.Length.EqualTo(1));
            Assert.That(fields[0].Name, Is.EqualTo("full_name"));
            Assert.That(fields[0].Type, Is.EqualTo("text"));
            Assert.That(fields[0].Label, Is.EqualTo("Name"));
            Assert.That(fields[0].Required, Is.True);
        }

        [Test]
        public void InputFieldListWithoutScheduleRequestsUserFields()
        {
            Client.Schedules.InputFieldList();
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/input_field_list.json"));
        }

        [Test]
        public void SlotNullAttributesAreNotSent()
        {
            Client.Slots.Update(1, 2, new Dictionary<string, string> { { "title", "x" }, { "capacity", null! } });

            Assert.That(Encoding.UTF8.GetString(Handler.LastRequest.Body!), Is.EqualTo("{\"slot\":{\"title\":\"x\"}}"));
        }
    }
}
