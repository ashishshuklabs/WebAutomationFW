using System;
using System.Threading;
using WebAutomation.Framework.Core.BaseSetup;
using WebAutomation.Framework.Core.Default;
using WebAutomation.Framework.Core.Tests.Utilities;
using WebAutomation.Framework.Core.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

[assembly: LevelOfParallelism(8)]
namespace WebAutomation.Framework.Core.Tests.BrowserTests {
    
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("Integration Tests")]
    public class BrowserLaunchTests : DriverSetup {
        public override void FixtureSetup() {
            DefaultTestRunParameters.Get();
            DefaultTestRunParameters.Set(nameof(Constants.RS_LocalExecution), "true");
            DefaultTestRunParameters.Set(nameof(Constants.RS_LocalExecutionAsService), "true");
            DefaultTestRunParameters.Set(nameof(Constants.RS_ServerHost), "localhost");
            DefaultTestRunParameters.Set(nameof(Constants.RS_ServerPort), "4278");
            DefaultTestRunParameters.Set(nameof(Constants.RS_DriverServerExePath), TestContext.CurrentContext.TestDirectory);

            base.FixtureSetup();
        }
        public override void TestSetup() {
            ChromeOptions chromeOptions = this.Capabilities.GetDriverOptions<ChromeOptions>();
            if(chromeOptions != null) {
                chromeOptions.AddArgument("--headless");
            }
            EdgeOptions edgeOptions = this.Capabilities.GetDriverOptions<EdgeOptions>();
            if(edgeOptions != null) {
                edgeOptions.StartPage = "http://google.com";
            }
            SafariOptions safariOptions = this.Capabilities.GetDriverOptions<SafariOptions>();
            if(safariOptions != null) {
                safariOptions.EnableAutomaticProfiling = true;
            }
            base.TestSetup();
        }

        [Test]
        public void LaunchChromeBrowser() {
            Console.WriteLine($"Thread used in test: {Thread.CurrentThread.ManagedThreadId}");
            Assert.DoesNotThrow((() => WebTestContext.Get<RemoteWebDriver>(nameof(WebDriver))), "Driver should be set in the current context.");
            Assert.NotNull(WebTestContext.Get<RemoteWebDriver>(nameof(WebDriver)), $"Driver should not be null");
            Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId}");
            var driver = WebDriver.Instance;
            driver.Navigate().GoToUrl("http://google.com");
            Wait.ForPageToLoad();
            driver.FindVisibleAndEnabled(By.CssSelector("input[name='q']")).SendKeys("Hello world from Method1");
        }
        [Test]
        public void LaunchChromeBrowserAgain() {
            Assert.DoesNotThrow((() => WebTestContext.Get<RemoteWebDriver>(nameof(WebDriver))), "Driver should be set in the current context.");
            Assert.NotNull(WebTestContext.Get<RemoteWebDriver>(nameof(WebDriver)), $"Driver should not be null");
            Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId}");
            var driver = WebDriver.Instance;
            driver.Navigate().GoToUrl("http://google.com");
            Wait.ForPageToLoad();
            driver.FindVisibleAndEnabled(By.CssSelector("input[name='q']")).SendKeys("Hello world from Method2");
        }
    }
}
