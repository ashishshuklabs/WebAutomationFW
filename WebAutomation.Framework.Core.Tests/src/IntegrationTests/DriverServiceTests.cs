using WebAutomation.Framework.Core.BaseSetup;
using WebAutomation.Framework.Core.Default;
using WebAutomation.Framework.Core.Utilities;
using NUnit.Framework;
using System.IO;
using DriverService = OpenQA.Selenium.DriverService;
using BaseDriverService = WebAutomation.Framework.Core.BaseSetup.DriverService;

namespace WebAutomation.Framework.Core.Tests.IntegrationTests {
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("Integration Tests")]
    public class DriverServiceTests {
        private TestEnvironmentParameters parameter;
        [SetUp]
        
        public void TestSetup() {
            parameter = new TestEnvironmentParameters {
                RS_DriverServerExePath =  Path.Combine(TestContext.CurrentContext.TestDirectory),
                RS_LocalExecution = true,
                RS_LocalExecutionAsService = true,
                RS_ServerHost = "localhost",
                RS_ServerResource = Constants.RS_ServerResource,
                RS_ServerPort = "5546",
                RS_ImplicitWaitTime = Constants.RS_ImplicitWaitTime,
                RS_BrowserName = "chrome"
            };
            

        }
        [Test]
        public void  CanStartServiceWhenAllRequiredParamsSet() {
            BaseDriverService.Start(parameter);
            Assert.NotNull(WebTestContext.Get<DriverService>(Constants.DriverServiceKey), "Service should have started and context set");
            var service = WebTestContext.Get<DriverService>(Constants.DriverServiceKey);
            Assert.True(service.IsRunning,"Service must be running");
            Assert.AreEqual(service.ServiceUrl, parameter.ServerUri, "Service URL should have been updated");
            
        }
        [TearDown]
        public void TestCleanup() {
            BaseDriverService.Dispose();
        }
    }
}
