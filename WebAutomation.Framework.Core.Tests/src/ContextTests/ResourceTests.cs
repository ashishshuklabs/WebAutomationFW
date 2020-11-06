using System.IO;
using WebAutomation.Framework.Core.BaseSetup;
using WebAutomation.Framework.Core.Default;
using WebAutomation.Framework.Core.Utilities;
using NUnit.Framework;
using System.Threading;
using WebAutomation.Framework.Core.Tests.Utilities;

namespace WebAutomation.Framework.Core.Tests.ContextTests {
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("Integration Tests")]
    public class ResourceTests : DriverSetup {
        public override void FixtureSetup() {
            DefaultTestRunParameters.Get();
            DefaultTestRunParameters.Set(nameof(Constants.RS_BrowserName), "chrome");
            DefaultTestRunParameters.Set(nameof(Constants.RS_LocalExecution), "true");
            DefaultTestRunParameters.Set(nameof(Constants.RS_LocalExecutionAsService), "true");
            DefaultTestRunParameters.Set(nameof(Constants.RS_ServerHost), "localhost");
            DefaultTestRunParameters.Set(nameof(Constants.RS_ServerPort), "4278");
            DefaultTestRunParameters.Set(nameof(Constants.RS_DriverServerExePath), TestContext.CurrentContext.TestDirectory);

            base.FixtureSetup();
        }
        private string screenshotFile, logfile;
        /// <summary>
        /// ToDo: This test is leaving a stray chrome driver behind.
        /// </summary>
        [Test]

        public void CreateAndAttachScreenshotPasses() {
            System.Console.WriteLine($"Thread used in test: {Thread.CurrentThread.ManagedThreadId}");
            Screenshot.Attach();
            Assert.True(File.Exists(WebTestContext.Get<string>(Constants.ScreenshotFileKey)), "Screenshot file not found");
            screenshotFile = WebTestContext.Get<string>(Constants.ScreenshotFileKey);
        }
        [Test]

        public void RunningATestCreatesLogFileByDefault() {
            System.Console.WriteLine($"Thread used in test: {Thread.CurrentThread.ManagedThreadId}");
            TestLogs.Attach();
            Assert.True(File.Exists(WebTestContext.Get<string>(Constants.TestLogFileKey)), "Test Log file not found.");
            logfile = WebTestContext.Get<string>(Constants.TestLogFileKey);
        }
    }
}
