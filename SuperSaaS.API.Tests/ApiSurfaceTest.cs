using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace SuperSaaS.API.Tests
{
    /// <summary>
    /// Guards the public API: the assembly's public and protected surface must match the snapshot taken from
    /// v1.0.1 (PublicApi.approved.txt). If an intentional, additive change is made, copy the generated
    /// PublicApi.received.txt over the approved file.
    /// </summary>
    [TestFixture]
    public class ApiSurfaceTest
    {
        [Test]
        public void PublicApiMatchesApprovedSnapshot()
        {
            string directory = TestContext.CurrentContext.TestDirectory;
            string approved = File.ReadAllText(Path.Combine(directory, "PublicApi.approved.txt")).Replace("\r\n", "\n");
            string actual = ApiSurface.Describe(typeof(Client).Assembly);

            if (!string.Equals(approved, actual, StringComparison.Ordinal))
            {
                string received = Path.Combine(directory, "PublicApi.received.txt");
                File.WriteAllText(received, actual);
                string[] approvedLines = approved.Split('\n');
                string[] actualLines = actual.Split('\n');
                string missing = string.Join("\n", approvedLines.Except(actualLines).Select(l => "- " + l));
                string added = string.Join("\n", actualLines.Except(approvedLines).Select(l => "+ " + l));
                Assert.Fail("Public API differs from PublicApi.approved.txt (full output: " + received + ")\n" +
                            missing + "\n" + added);
            }
        }
    }
}
