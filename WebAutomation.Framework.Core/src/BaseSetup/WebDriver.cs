using System;
using WebAutomation.Framework.Core.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace WebAutomation.Framework.Core.BaseSetup {
    public class WebDriver {
        /// <summary>
        /// Web Driver Instance
        /// </summary>
        public static RemoteWebDriver Instance {
            get => WebTestContext.Get<RemoteWebDriver>(nameof(WebDriver));
            set => WebTestContext.Set(nameof(WebDriver), value);
        }


        /// <summary>
        /// Initialize the Webdriver
        /// </summary>
        /// <param name="capabilities">Driver capabilities required</param>
        /// <param name="waitTime">Time for Webdriver Waits to use</param>
        public static void Initialize(DriverCapabilities capabilities) {
            var instance = Get(capabilities);
            var context = capabilities.GetEnvironmentContext();
            int implicitWaitTime = Convert.ToInt32(context.RS_ImplicitWaitTime);
            TestContext.WriteLine("Session Id:" + instance.SessionId);
            TimeSpan implicitWaitTimeSpan = TimeSpan.FromSeconds(implicitWaitTime);
            instance.Manage().Timeouts().ImplicitWait = implicitWaitTimeSpan;
            instance.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            instance.Manage().Window.Maximize();
            Instance = instance;
        }
        /// <summary>
        /// Get the Remote web driver instance based on the <see cref="DriverCapabilities"/> setup
        /// </summary>
        /// <returns></returns>
        public static RemoteWebDriver Get(DriverCapabilities driverCapabilities) {
            TestEnvironmentParameters context = driverCapabilities.GetEnvironmentContext();
            Helpers.IsNullOrEmpty(context.ServerUri, $"[{nameof(context.ServerUri)}] cannot be null.");
            DriverOptions options;
            switch(context.RS_BrowserName.ToUpper()) {
                case "CHROME":
                    options = driverCapabilities.GetDriverOptions<ChromeOptions>();
                    return GetDriver(() => new CustomRemoteWebDriver(context.ServerUri, (ChromeOptions)options));
                case "SAFARI":
                    options = driverCapabilities.GetDriverOptions<SafariOptions>();
                    return GetDriver(() => new CustomRemoteWebDriver(context.ServerUri, (SafariOptions)options));
                case "EDGE":
                    options = driverCapabilities.GetDriverOptions<EdgeOptions>();
                    return GetDriver(() => new CustomRemoteWebDriver(context.ServerUri, (EdgeOptions)options));
                case "IE":
                    options = driverCapabilities.GetDriverOptions<InternetExplorerOptions>();
                    return GetDriver(() => new CustomRemoteWebDriver(context.ServerUri, (InternetExplorerOptions)options));
                default:
                    throw new ArgumentOutOfRangeException($"{context.RS_BrowserName}", "This option is not supported");
            }
        }
        /// <summary>
        /// Exception Wrapper creating driver
        /// </summary>
        /// <param name="method">method to execute inside the wrapper</param>
        /// <returns></returns>
        private static RemoteWebDriver GetDriver(Func<RemoteWebDriver> method) {
            try {
                return method();
            } catch(Exception e) {
                TestLogs.Write(e.ToString());
                throw new WebDriverException($"Cannot Create Driver Instance. Exception:\n{e}");
            }

        }

        public static void Dispose() {
            try {
                Instance?.Quit();//Calls dispose behind the scenes.
                                 // Instance?.Dispose();
                WebTestContext.Dispose();
            } catch(Exception ex) {
                TestContext.WriteLine($"Failed to quit session: {ex.Message}");
            }
        }

    }


}