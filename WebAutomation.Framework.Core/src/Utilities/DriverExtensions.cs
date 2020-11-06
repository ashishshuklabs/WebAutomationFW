using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using WebAutomation.Framework.Core.BaseSetup;
using WebAutomation.Framework.Core.Default;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;
namespace WebAutomation.Framework.Core.Utilities {
    public static class DriverExtensions {
        public static IWebElement FindVisibleElement(this RemoteWebDriver driver, By by) {
            return FindVisibleElements(driver, by).First();
        }
        /// <summary>
        /// Extension for finding visible elements, embeds synchronization based on implicit wait time.
        /// </summary>
        /// <param name="driver">Driver</param>
        /// <param name="by">Selector/identifier for the control</param>
        /// <returns></returns>
        public static ReadOnlyCollection<IWebElement> FindVisibleElements(this RemoteWebDriver driver, By by) {
            const double pollInterval = 0.25;
            double maxWait = Constants.DefaultWaitTime;
            TimeSpan originalImplicitWait = TimeSpan.FromSeconds(0);
            WebDriver.Instance.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;

            for(double elapsed = 0; elapsed <= maxWait; elapsed += pollInterval) {
                IEnumerable<IWebElement> webElements = driver.FindElements(by).Where(e => e.Displayed && e.Enabled);
                if(webElements.Any()) {
                    WebDriver.Instance.Manage().Timeouts().ImplicitWait = originalImplicitWait;
                    return webElements.ToList().AsReadOnly();
                }
                Wait.Seconds(pollInterval);
            }
            WebDriver.Instance.Manage().Timeouts().ImplicitWait = originalImplicitWait;
            throw new AssertionException($"Failed to find visible element via: {by} in {maxWait} seconds");
        }
        /// <summary>
        /// Find element specified by selector <see cref="By"/>. Tries searching for element in the DOM until <see cref="timeout"/> is reached
        /// or elements found.
        /// </summary>
        /// <param name="driver">web driver instance</param>
        /// <param name="by">selector for the element of interest</param>
        /// <param name="timeout">timeout in seconds for searching elements</param>
        /// <param name="throwIfNotFound">throw exception if element not found within specified time above, default true</param>
        /// <returns></returns>
        public static IWebElement FindElement(this RemoteWebDriver driver, By by, int timeout,
            bool throwIfNotFound = true) {
            return FindElements(driver, by, timeout, throwIfNotFound).First();

        }
        /// <summary>
        /// Find elements specified by selector <see cref="By"/>. Tries searching for element in the DOM until <see cref="timeout"/> is reached
        /// or elements found.
        /// </summary>
        /// <param name="driver">web driver instance</param>
        /// <param name="by">selector for the element of interest</param>
        /// <param name="timeout">timeout in seconds for searching elements</param>
        /// <param name="throwIfNotFound">throw exception if element not found within specified time above, default true</param>
        /// <returns></returns>
        public static IReadOnlyCollection<IWebElement> FindElements(this RemoteWebDriver driver, By by, int timeout,
            bool throwIfNotFound = true) {
            if(by == null) {
                throw new ArgumentNullException($"Parameter {nameof(by)} cannot be null or empty");
            }

            IReadOnlyCollection<IWebElement> result = null;

            Stopwatch watch = new Stopwatch();
            watch.Restart();
            do {
                try {
                    result = driver.FindElements(by);
                    if(result.Count > 0) {
                        return result;
                    }
                    Wait.Seconds(0.5);
                } catch(Exception) {
                    Wait.Seconds(0.5);
                }
            } while(watch.Elapsed.Seconds < timeout);

            if(throwIfNotFound) {
                throw new NotFoundException($"Element(s) with selector [{by}] not found within [{timeout}] seconds.");
            }

            return null;
        }
        /// <summary>
        /// Check if element(s) exist in DOM
        /// </summary>
        /// <param name="driver">web driver instance</param>
        /// <param name="by">search criteria for finding elements</param>
        /// <param name="elements">web elements if found, null otherwise</param>
        /// <param name="timeout"></param>
        /// <returns>boolean depending on the element found/not</returns>
        public static bool DoElementsExist(this RemoteWebDriver driver, By by,
            out IReadOnlyCollection<IWebElement> elements, int timeout = Constants.DefaultWaitTime) {
            IReadOnlyCollection<IWebElement> results = FindElements(driver, by, timeout, false);
            if(results == null) {
                elements = null;
                return false;
            }

            elements = results;
            return true;
        }
        /// <summary>
        /// Check if element exists.code returns true/false
        /// based on element found or not.
        /// </summary>
        /// <param name="driver">appium driver</param>
        /// <param name="by">search criteria</param>
        /// <param name="element">out parameter representing the element</param>
        /// <param name="timeout">timeout for element search</param>
        /// <returns></returns>
        public static bool DoesElementExist(this RemoteWebDriver driver, By by,
            out IWebElement element, int timeout = 10) {
            IReadOnlyCollection<IWebElement> elements;
            bool result = DoElementsExist(driver, by, out elements, timeout);
            element = elements?.FirstOrDefault();
            return result;
        }
        /// <summary>
        /// Wait until the element is visible and enabled(clickable)
        /// </summary>
        /// <param name="by">Mobile by search criteria</param>
        /// <param name="timeoutInSeconds">timeout, defaults to <see cref="Constants.DefaultWaitTime"/></param>
        /// <returns></returns>
        public static IWebElement FindVisibleAndEnabled(this RemoteWebDriver driver, By by, int timeoutInSeconds = Constants.DefaultWaitTime) {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            try {
                return wait.Until(ExpectedConditions.ElementToBeClickable(by));
            } catch(WebDriverTimeoutException) {
                throw new AssertionException($"Failed to find element while waiting for visibity via: {by} for {timeoutInSeconds} seconds");
            }
        }

    }
}
