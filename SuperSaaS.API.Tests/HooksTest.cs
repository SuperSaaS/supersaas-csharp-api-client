using System;
using System.Collections.Generic;
using NUnit.Framework;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Tests
{
    // Records the calls instead of performing them, so path, method and args can be asserted.
    public class RecordingClient : IClient
    {
        public string Method;
        public string Path;
        public NestedJsonArgs Body;
        public JsonArgs Query;

        private T Record<T>(string method, string path, NestedJsonArgs body, JsonArgs query)
        {
            Method = method; Path = path; Body = body; Query = query;
            return default(T);
        }

        public T Get<T>(string path, JsonArgs queryData = null) { return Record<T>("GET", path, null, queryData); }
        public T Post<T>(string path, NestedJsonArgs postData = null, JsonArgs queryData = null) { return Record<T>("POST", path, postData, queryData); }
        public T Put<T>(string path, NestedJsonArgs postData = null, JsonArgs queryData = null) { return Record<T>("PUT", path, postData, queryData); }
        public T Delete<T>(string path, NestedJsonArgs postData = null, JsonArgs queryData = null) { return Record<T>("DELETE", path, postData, queryData); }
        public int GetResourceIdFromHeader() { return 0; }
    }

    [TestFixture()]
    public class HooksTest
    {
        public Client Client;

        [SetUp]
        public void Init()
        {
            Configuration config = new Configuration();
            config.AccountName = "accnt";
            config.ApiKey = "xxxxxxxxxxxxxxxxxxxx";
            config.Test = true;

            this.Client = new Client(config);
        }

        [Test()]
        public void PingTest()
        {
            Assert.IsTrue(this.Client.Ping());
        }

        [Test()]
        public void CreateTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Hooks.Create("https://example.com/hook", "N", 123); });
            RecordingClient rec = new RecordingClient();
            new Api.Hooks(rec).Create("https://example.com/hook", "N", 123);
            Assert.AreEqual("POST", rec.Method);
            Assert.AreEqual("/hooks", rec.Path);
            Assert.AreEqual("https://example.com/hook", rec.Query["target_url"]);
            Assert.AreEqual("N", rec.Query["event"]);
            Assert.AreEqual("123", rec.Query["parent_id"]);
        }

        [Test()]
        public void DeleteTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Hooks.Delete(456, 123); });
            RecordingClient rec = new RecordingClient();
            new Api.Hooks(rec).Delete(456, 123);
            Assert.AreEqual("DELETE", rec.Method);
            Assert.AreEqual("/hooks", rec.Path);
            Assert.AreEqual("456", rec.Query["id"]);
            Assert.AreEqual("123", rec.Query["parent_id"]);
        }

        [Test()]
        public void InputFieldListTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Schedules.InputFieldList(123); });
            RecordingClient rec = new RecordingClient();
            Api.Schedules schedules = new Api.Schedules(rec);
            schedules.InputFieldList(123);
            Assert.AreEqual("GET", rec.Method);
            Assert.AreEqual("/input_field_list", rec.Path);
            Assert.AreEqual("123", rec.Query["schedule_id"]);
            schedules.InputFieldList();
            Assert.IsNull(rec.Query);
        }

        [Test()]
        public void DeserializesHookAndFieldListTest()
        {
            Hook hook = Newtonsoft.Json.JsonConvert.DeserializeObject<Hook>("{\"id\":123,\"parent_id\":456}");
            Assert.AreEqual(123, hook.Id);
            Assert.AreEqual(456, hook.ParentId);
            FieldList[] fields = Newtonsoft.Json.JsonConvert.DeserializeObject<FieldList[]>("[{\"name\":\"full_name\",\"type\":\"text\",\"label\":\"Name\",\"required\":true}]");
            Assert.AreEqual("full_name", fields[0].Name);
            Assert.AreEqual("Name", fields[0].Label);
            Assert.IsTrue(fields[0].Required);
        }

        [Test()]
        public void SlotNullAttributesNotSentTest()
        {
            RecordingClient rec = new RecordingClient();
            new Api.Slots(rec).Update(1, 2, new Dictionary<string, string> { { "title", "x" }, { "capacity", null } });
            Assert.IsTrue(rec.Body["slot"].ContainsKey("title"));
            Assert.IsFalse(rec.Body["slot"].ContainsKey("capacity"));
        }
    }
}
