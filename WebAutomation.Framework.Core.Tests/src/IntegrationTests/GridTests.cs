using WebAutomation.Framework.Core.BaseSetup;
using WebAutomation.Framework.Core.Default;
using WebAutomation.Framework.Core.Tests.Utilities;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;

namespace WebAutomation.Framework.Core.Tests.IntegrationTests {
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("Integration Tests")]
    public class GridTests: DriverSetup {
        public override void FixtureSetup() {
            DefaultTestRunParameters.Get();
            DefaultTestRunParameters.Set(nameof(Constants.RS_LocalExecution), "true");
            DefaultTestRunParameters.Set(nameof(Constants.RS_ServerPort),"4444");
            DefaultTestRunParameters.Set(nameof(Constants.RS_BrowserName),"chrome");
            base.FixtureSetup();
        }

        [Test]
        public void BasicTestLaunchesChromeAndSearchesQRSInGrid() {
            var driver = WebDriver.Instance;
            driver.Navigate().GoToUrl("https://www.google.com");
            driver.Manage().Window.Maximize();
            driver.FindElement(By.Name("q")).SendKeys("May be enter QRS this on a remote machine");
        }
        [Test]
        public void BasicTestLaunchesChromeAndSearchesABCInGrid() {
            var driver = WebDriver.Instance;
            driver.Navigate().GoToUrl("https://www.google.com");
            driver.Manage().Window.Maximize();
            driver.FindElement(By.Name("q")).SendKeys("I Want to enter ABC on this");

        }

    }
}
