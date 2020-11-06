using WebAutomation.Framework.Core.BaseSetup;
using WebAutomation.Framework.Core.Default;
using WebAutomation.Framework.Core.Utilities;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;

namespace WebAutomation.Framework.Core.Tests.IntegrationTests {
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("Integration Tests")]
    public class WebDriverTests {
        [SetUp]
        public void TestSetup() {
            WebTestContext.Set(nameof(Constants.DevelopmentMode), "true");
        }
        [Test]
        public void SupplyingAcceptableCapabilitiesForDriverServiceLaunchesChromeBrowser() {
            TestEnvironmentParameters @params = new TestEnvironmentParameters() {
                RS_LocalExecution = true,
                RS_LocalExecutionAsService = true,
                RS_DriverServerExePath = TestContext.CurrentContext.TestDirectory,
                RS_BrowserName = "chrome",
                RS_ServerHost = "localhost",
                RS_ServerPort = "4278",
                RS_ImplicitWaitTime = "0" //Write unit test for testing empty properties
            };
            DriverService.Start(@params);
            DriverCapabilities caps = new DriverCapabilities(@params);
            caps.GetDriverOptions<ChromeOptions>().AddArgument("--headless"); //Test this in unit test
            WebDriver.Initialize(caps);
            Assert.That(WebDriver.Instance != null, "Driver should have been instantiated successfully");
            Assert.AreEqual("data:,", WebDriver.Instance.Url, "Empty url should have been opened");
            WebDriver.Instance.Navigate().GoToUrl("https://google.com");
            Assert.AreEqual("https://www.google.com/", WebDriver.Instance.Url,"Should have navigated to google.com");
            WebDriver.Dispose();
            DriverService.Dispose();
        }
        
    }
}
