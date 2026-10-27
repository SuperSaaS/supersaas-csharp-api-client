using System;
using NUnit.Framework;

namespace SuperSaaS.API.Tests
{
    [TestFixture]
    public class FormsTest : TestModeFixture
    {
        [Test]
        public void GetTest()
        {
            Assert.DoesNotThrow(() => Client.Forms.Get(123));
        }

        [Test]
        public void ListTest()
        {
            Assert.DoesNotThrow(() => Client.Forms.List(123, DateTime.Now));
        }
    }
}
