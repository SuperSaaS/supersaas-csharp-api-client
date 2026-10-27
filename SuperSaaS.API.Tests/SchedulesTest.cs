using NUnit.Framework;

namespace SuperSaaS.API.Tests
{
    [TestFixture]
    public class SchedulesTest : TestModeFixture
    {
        [Test]
        public void ListTest()
        {
            Assert.DoesNotThrow(() => Client.Schedules.List());
        }

        [Test]
        public void ResourcesTest()
        {
            Assert.DoesNotThrow(() => Client.Schedules.Resources(12345));
        }
    }
}
