using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSaaS.API;
using SuperSaaS.API.Models;

#nullable enable

namespace SuperSaaS.API.Tests
{
    [TestClass]
    public class FormsTest
    {
        private Client? Client { get; set; }
        private readonly int formId = 123;
        private readonly int templateFormId = 456;
        private readonly string userId = "789";
        private readonly DateTime testTime = new DateTime(2025, 1, 1, 12, 0, 0);

        [TestInitialize]
        public void Init()
        {
            Configuration config = new Configuration
            {
                AccountName = "accnt",
                ApiKey = "xxxxxxxxxxxxxxxxxxxx",
                Test = true
            };
            Client = new Client(config);
        }

        [TestMethod]
        public void GetTest()
        {
            var form = Client!.Forms.Get(formId);
            Assert.IsInstanceOfType(form, typeof(Form));
        }

        [TestMethod]
        public void ListTest()
        {
            var forms = Client!.Forms.List(
                templateFormId: templateFormId,
                fromTime: testTime,
                user: userId,
                limit: 10,
                offset: 0
            );
            Assert.IsNotNull(forms);
            Assert.IsInstanceOfType(forms, typeof(Form[]));
            Assert.AreEqual(0, forms.Length);
        }

        [TestMethod]
        public void GetFormsTest()
        {
            var forms = Client!.Forms.GetForms();
            Assert.IsNotNull(forms);
            Assert.IsInstanceOfType(forms, typeof(SuperForm[]));
            Assert.AreEqual(0, forms.Length);

        }
    }
}