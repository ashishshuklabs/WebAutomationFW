using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using WebAutomation.Framework.Core.BaseSetup;
using WebAutomation.Framework.Core.Default;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace WebAutomation.Framework.Core.Utilities {
    public class Wait {
        /// <summary>
        /// wait for zillion seconds.
        /// </summary>
        /// <param name="seconds"></param>
        public static void Seconds(double seconds) {
            int waitMs = (int)(seconds * 1000);
            Thread.Sleep(waitMs);
        }

        public static void UntilElementNotFound(By by) {
            UntilNotVisible(by);
        }

        public static void UntilElementNotFound(By by, int waitTime) {
            UntilNotVisible(by, waitTime);
        }

        public static void UntilNotVisible(By by) {
            UntilNotVisible(by, Constants.DefaultWaitTime);
        }

        public static void UntilInvisibilityOfAllElements(By by) {
            UntilInvisibilityOfAllElements(by, Constants.DefaultWaitTime);
        }

        // Create the function which is similar to invisibilityOfAllElements in Java for checking all elements filtered to be invisible
        public static void UntilInvisibilityOfAllElements(By by, int waitTime) {
            WebDriverWait wait = new WebDriverWait(WebDriver.Instance, TimeSpan.FromSeconds(waitTime)) {
                Timeout = new TimeSpan(0, 0, waitTime),
            };
            try {
                wait.Until(driver => driver.FindElements(by).All(e => !e.Displayed));
            } catch(WebDriverTimeoutException) {
                throw new AssertionException($"Element was continually found when waiting for lack of visibility via: {by}");
            }
        }

        public static void UntilVisible(By by, int timeoutInSeconds = 10) {
            WebDriverWait wait = new WebDriverWait(WebDriver.Instance, TimeSpan.FromSeconds(timeoutInSeconds));
            try {
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(by));
            } catch(WebDriverTimeoutException) {
                throw new AssertionException($"Element was not found when waiting for visibility via: {by} for {timeoutInSeconds} seconds");
            }

        }

        public static void UntilVisible(IWebElement element, int timeoutInSeconds = 10) {
            WebDriverWait wait = new WebDriverWait(WebDriver.Instance, TimeSpan.FromSeconds(timeoutInSeconds));
            List<IWebElement> elements = new List<IWebElement> { element };
            ReadOnlyCollection<IWebElement> collection = new ReadOnlyCollection<IWebElement>(elements);
            try {
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(collection));
            } catch(WebDriverTimeoutException) {
                throw new AssertionException($"Element  was not found when waiting for visibility for {timeoutInSeconds} seconds");
            }

        }
        /// <summary>
        /// Wait 10 seconds for a loading shade to appear incase it takes an unusual amount of time, 
        /// if we dont see a loading spinner in the first 10 second, assume it has come and gone and all is well,
        /// if we do see one, store it and wait for that specific shade to disapear 
        /// </summary>
        /// <param name="by"></param>
        /// <param name="appearTime"></param>
        /// <param name="disappearTime"></param>
        /// <param name="pollingInterval"></param>
        /// <param name="throwException"></param>
        public static void UntilVisibleThenNotVisible(By by, int appearTime = 10, int disappearTime = 20, double pollingInterval = 0.1, bool throwException = false) {
            IWebElement element;
            try {
                WebDriverWait wait = new WebDriverWait(new SystemClock(), WebDriver.Instance, TimeSpan.FromSeconds(appearTime), TimeSpan.FromSeconds(pollingInterval));
                element = wait.Until(ExpectedConditions.ElementIsVisible(by));
            } catch(WebDriverTimeoutException) {
                TestContext.WriteLine($"Element with selector[{by}] still not visible after [{appearTime}]");
                return;
            }

            if(element == null) {
                return;
            }
            UntilNotVisible(by, disappearTime, throwException);
        }

        public static void UntilNotVisible(By by, int waitTime, bool throwException = true) {
            WebDriverWait wait = new WebDriverWait(WebDriver.Instance, TimeSpan.FromSeconds(waitTime)) {
                Timeout = new TimeSpan(0, 0, waitTime),
            };
            try {
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(by));
            } catch(WebDriverTimeoutException) {
                if(throwException) {
                    throw new AssertionException(
                        $"Element was continually found when waiting for lack of visibility via: {by}");
                }
                TestContext.WriteLine($"Element with selector[{by}] still visible after [{waitTime}], no exception thrown as {nameof(throwException)} flag is [{throwException}]");
            }
        }


        public static IWebElement UntilVisibleAndEnabled(By by) {
            return UntilVisibleAndEnabled(by, Constants.DefaultWaitTime);
        }

        public static IWebElement UntilVisibleAndEnabled(By by, int waitTime) {
            WebDriverWait wait = new WebDriverWait(WebDriver.Instance, TimeSpan.FromSeconds(waitTime)) {
                Timeout = new TimeSpan(0, 0, waitTime),
            };
            try {
                return wait.Until(ExpectedConditions.ElementToBeClickable(by));
            } catch(WebDriverTimeoutException) {
                throw new AssertionException($"Failed to find element while waiting for visibity via: {by}");
            }
        }

        /// <summary>
        /// Retry an action until condition satisfied or until timeout.
        /// </summary>
        /// <param name="function">action</param>
        /// <param name="maxWaitTime">timeout</param>
        public static void For(Action function, int maxWaitTime = 0) {
            RetryHelper.Retry(function, maxWaitTime);
        }
        /// <summary>
        /// Retry an action until condition satisfied or until timeout.
        /// </summary>
        /// <param name="function">action</param>
        /// <param name="maxWaitTime">timout</param>
        public static void For(Func<bool> function, int maxWaitTime = 0) {
            RetryHelper.Retry(function, maxWaitTime);
        }
        /// <summary>
        /// Wait for the page ready state to be complete
        /// </summary>
        /// <param name="timeoutInSeconds">timeout, default 5 seconds</param>
        public static void ForPageToLoad(int timeoutInSeconds = 5) {
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            while(WebDriver.Instance.ExecuteScript("return document.readyState;").ToString() != "complete") {
                if(watch.Elapsed.TotalSeconds > timeoutInSeconds) {
                    break;
                }
                Thread.Sleep(1000);
            }

            if(WebDriver.Instance.ExecuteScript("return document.readyState;").ToString() != "complete") {
                throw new InvalidElementStateException($"Page did not load successfully in [{timeoutInSeconds}] seconds");
            }
        }
    }
}