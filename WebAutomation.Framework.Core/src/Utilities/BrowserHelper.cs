using System;
using WebAutomation.Framework.Core.BaseSetup;
using NUnit.Framework;

namespace WebAutomation.Framework.Core.Utilities {
    public enum TabOperation {
        SwitchTab,
        CloseTab,
        CloseMultipleTabsExceptThis
    }
    public class BrowserHelper {
        /// <summary>
        /// Perform Operation on Browser window/tabs.
        /// Sample Usage:
        /// <para>BrowserOperation(TabOperation.SwitchTab, () => WebDriver.Instance.Url.Contains("WebCase"));</para>
        /// <para>BrowserOperation(TabOperation.CloseTab, () => WebDriver.Instance.Title.Contains("Case Administration"));</para>
        /// <para>BrowserOperation(TabOperation.CloseMultipleTabsExceptThis, () => !WebDriver.Instance.Url.Contains("WebCase"));//closes
        /// all tabs except the one containing above url</para>
        /// </summary>
        /// <param name="operation">Operation to perform <see cref="TabOperation"/></param>
        /// <param name="searchCriteria">Condition searchText need to satisfy</param>
        /// <returns> boolean flag indicating whether operation was succesfully performed or not.</returns>
        public static bool Operation(TabOperation operation, Func<bool> searchCriteria) {
            bool isOperationSuccessful = false;
            switch(operation) {
                case TabOperation.SwitchTab:
                    isOperationSuccessful = SwitchingBrowserTab(searchCriteria);
                    break;
                case TabOperation.CloseTab:
                    isOperationSuccessful = CloseTab(searchCriteria);
                    break;
                case TabOperation.CloseMultipleTabsExceptThis:
                    isOperationSuccessful = RepeatedOperation(() => CloseTab(searchCriteria),
                        () => WebDriver.Instance.WindowHandles.Count > 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }

            return isOperationSuccessful;
        }

        /// <summary>
        /// Perform repeated operation on browser tabs
        /// </summary>
        /// <param name="repeatOperationName">Operation to perform</param>
        /// <param name="conditionForOperation">Condition to be satisfied for performing operation</param>
        /// <param name="numberOfInterations">number of times the operation needs to be performed(Exit criteria)</param>
        /// <returns></returns>
        private static bool RepeatedOperation(Func<bool> repeatOperationName, Func<bool> conditionForOperation, int numberOfInterations = 5) {
            int count = 0;
            bool isOperationSuccessful = false;
            do {
                if(count > numberOfInterations) {
                    TestContext.WriteLine($"Too many tabs open(>{numberOfInterations}). Something is not right, see screenshot!");
                    break;
                }
                isOperationSuccessful = repeatOperationName();
                count++;

            } while(conditionForOperation());

            return isOperationSuccessful;
        }

        /// <summary>
        /// Switches to the expected browser tab based on condition and search text provided.
        /// </summary>
        /// <param name="searchText">text to search for</param>
        /// <param name="condition">condition to be satisfied for the </param>
        /// <returns></returns>
        private static bool SwitchingBrowserTab(Func<bool> condition) {
            bool isSwitchTabSuccessful = false;
            int tabCount = WebDriver.Instance.WindowHandles.Count;
            if(condition()) {
                TestContext.WriteLine("No need to switch tabs, already on the expected tab");
                isSwitchTabSuccessful = true;
                return isSwitchTabSuccessful;
            }

            for(int i = 0; i < tabCount; i++) {
                WebDriver.Instance.SwitchTo().Window(WebDriver.Instance.WindowHandles[i]);
                Wait.Seconds(0.25);
                if(condition()) {
                    isSwitchTabSuccessful = true;
                    return isSwitchTabSuccessful;
                }
            }
            if(!condition()) {
                TestContext.WriteLine($"Cannot switch to Browser tab with [{nameof(condition)}] criteria!");
            }

            return isSwitchTabSuccessful;

        }

        /// <summary>
        /// Closes a browser tab
        /// </summary>
        /// <param name="searchText">text to search for in the interested tab</param>
        /// <param name="condition">condition to find the searchText in the interested tab</param>
        /// <returns></returns>
        private static bool CloseTab(Func<bool> condition) {
            int startCount, count = 0;
            bool isOperationSuccessful = false;
            startCount = WebDriver.Instance.WindowHandles.Count;
            if(startCount <= 1) {
                TestContext.WriteLine($"Cannot perform operation as Browser Tab count is [{startCount}]." +
                                                    $"This would immediately close the browser.");
                return isOperationSuccessful;
            }

            int index = 0;
            do {
                if(count > 5) {
                    TestContext.WriteLine($"Cannot close tab, too many open [>5].");
                    break;
                }

                if(index < WebDriver.Instance.WindowHandles.Count) {
                    WebDriver.Instance.SwitchTo()
                        .Window(WebDriver.Instance.WindowHandles[index]);
                }

                if(condition()) {
                    WebDriver.Instance.SwitchTo().Window(WebDriver.Instance.CurrentWindowHandle).Close();
                    isOperationSuccessful = true;
                    int currentCount = WebDriver.Instance.WindowHandles.Count;
                    //Switch to first tab coz we don't know where we need to be anymore.
                    WebDriver.Instance.SwitchTo()
                        .Window(WebDriver.Instance.WindowHandles[WebDriver.Instance.WindowHandles.Count - 1]);
                    Wait.Seconds(0.25);
                    break;
                }
                count++;
                index++;

            } while(WebDriver.Instance.WindowHandles.Count > 1);

            return isOperationSuccessful;

        }

        /// <summary>
        /// Closes all tabs but one
        /// </summary>
        public static void CloseAllButOne() {
            WebDriver.Instance.SwitchTo().Window(WebDriver.Instance.WindowHandles[0]);
            while(WebDriver.Instance.WindowHandles.Count > 1) {
                WebDriver.Instance.SwitchTo().Window(WebDriver.Instance.WindowHandles[1]);
                WebDriver.Instance.SwitchTo().Window(WebDriver.Instance.CurrentWindowHandle)
                    .Close();
            }

            WebDriver.Instance.SwitchTo().Window(WebDriver.Instance.WindowHandles[0]);
        }
    }
}
