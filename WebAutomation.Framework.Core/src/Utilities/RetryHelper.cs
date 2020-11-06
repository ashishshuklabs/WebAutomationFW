using System;
using System.Diagnostics;
using WebAutomation.Framework.Core.Default;
using NUnit.Framework;
using OpenQA.Selenium;

namespace WebAutomation.Framework.Core.Utilities {
    public class RetryHelper {
        private const double DefaultInterval = 0.2;
        public static T Retry<T>(Func<T> function, int maxWaitTime = 0, double retryInterval = DefaultInterval) {
            Exception exception = null;
            Stopwatch stopwatch = Stopwatch.StartNew();

            if(maxWaitTime == 0) {
                maxWaitTime = Constants.DefaultWaitTime;
            }

            while(stopwatch.Elapsed.TotalSeconds < maxWaitTime) {
                try {
                    return function();
                } catch(Exception ex) {
                    exception = ex;
                    if(ex is AssertionException
                        || ex is InvalidOperationException
                        || ex is StaleElementReferenceException
                        || ex is ElementNotVisibleException
                        || ex is NoSuchElementException
                        || ex is NullReferenceException) {
                        Wait.Seconds(retryInterval);
                    } else {
                        throw;
                    }
                }
            }

            stopwatch.Stop();

            throw new AssertionException($"Retry timed out while trying to execute - {function.Method.Name}. Time elapsed: {stopwatch.Elapsed.TotalSeconds:0.##}. Exception message: {exception.Message}", exception);
        }

        public static void Retry(Action function, int maxWaitTime = 0, double retryInterval = DefaultInterval) {
            Retry(() => { function(); return true; }, maxWaitTime, retryInterval);
        }
    }
}
