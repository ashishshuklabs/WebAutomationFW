using System;
using System.IO;
using WebAutomation.Framework.Core.BaseSetup;
using WebAutomation.Framework.Core.Default;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace WebAutomation.Framework.Core.Utilities {
    public class Screenshot {
        public static void Attach() {
            if(!WebTestContext.ContainsKey(Constants.ScreenshotFileKey)) {
                TestLogs.Write("No screenshot file exists, no screenshot will be attached.");
                return;
            }

            string screenShotName = WebTestContext.Get<string>(Constants.ScreenshotFileKey);
            CaptureScreenshot(screenShotName);
            if(!File.Exists(screenShotName)) {
                TestLogs.Write("No screenshot file exists, no screenshot will be attached.");
                return;
            }
            TestContext.AddTestAttachment(WebTestContext.Get<string>(Constants.ScreenshotFileKey), $"Error screenshot");
        }
        private static void CaptureScreenshot(string screenShotName) {
            try {
                WebDriver.Instance.TakeScreenshot().SaveAsFile(screenShotName, ScreenshotImageFormat.Png);
            } catch(Exception e) {
                TestLogs.Write($"cannot capture screenshot of the application. {e.InnerException}");
            }

        }

    }
}
