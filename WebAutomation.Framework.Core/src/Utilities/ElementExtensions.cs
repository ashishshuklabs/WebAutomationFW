using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using OpenQA.Selenium;

namespace WebAutomation.Framework.Core.Utilities {
    public static class ElementExtensions {
        /// <summary>
        /// Find element specified by selector <see cref="By"/>. Tries searching for element in the DOM until <see cref="timeout"/> is reached
        /// or elements found.
        /// </summary>
        /// <param name="element">parent element containing the child ealement to be searched for</param>
        /// <param name="by">selector for the element of interest</param>
        /// <param name="timeout">timeout in seconds for searching elements</param>
        /// <param name="throwIfNotFound">throw exception if element not found within specified time above, default true</param>
        /// <returns></returns>
        public static IWebElement FindElement(this IWebElement element, By by, int timeout,
            bool throwIfNotFound = true) {
            return FindElements(element, by, timeout, throwIfNotFound).First();
        }
        /// <summary>
        /// Find elements specified by selector <see cref="By"/>. Tries searching for element in the DOM until <see cref="timeout"/> is reached
        /// or elements found.
        /// </summary>
        /// <param name="element">web driver instance</param>
        /// <param name="by">selector for the element of interest</param>
        /// <param name="timeout">timeout in seconds for searching elements</param>
        /// <param name="throwIfNotFound">throw exception if element not found within specified time above, default true</param>
        /// <returns></returns>
        public static IReadOnlyCollection<IWebElement> FindElements(this IWebElement element, By by, int timeout,
            bool throwIfNotFound = true) {
            if(by == null) {
                throw new ArgumentNullException($"Parameter {nameof(by)} cannot be null or empty");
            }

            ReadOnlyCollection<IWebElement> result;
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            do {
                try {
                    result = element.FindElements(by);
                    if(result.Count > 0) {
                        return result;
                    }
                    Wait.Seconds(0.5);
                    return element.FindElements(by);
                } catch(Exception) {
                    Wait.Seconds(0.5);
                }
            } while(watch.Elapsed.Seconds < timeout);

            if(throwIfNotFound) {
                throw new NotFoundException($"Element(s) with selector [{by}] not found within [{timeout}] seconds.");
            }

            return null;
        }
    }
}
