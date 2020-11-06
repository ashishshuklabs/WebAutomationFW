using System;
using System.IO;
using WebAutomation.Framework.Core.Default;
using WebAutomation.Framework.Core.Utilities;
using NUnit.Framework;

namespace WebAutomation.Framework.Core.BaseSetup {
    public class Resources {
        internal static string BasePath = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string testLogFolder = Path.Combine(BasePath, Constants.TestLogFolder);
        private static readonly string screenshotFolder = Path.Combine(testLogFolder, Constants.ScreenshotFolder);
        

        public static void SetupFolders() {

            if(!Directory.Exists(testLogFolder)) {
                Directory.CreateDirectory(testLogFolder);
            }

            if(!Directory.Exists(screenshotFolder)) {
                Directory.CreateDirectory(screenshotFolder);
            }
        }
        public static void SetupTestArtifacts() {
            if(Convert.ToBoolean(WebTestContext.Get<string>(nameof(Constants.DevelopmentMode), false))) {
                return;
            }
            SetupTestLog();
            SetupScreenshotFileName();
        }
        private static void SetupTestLog() {
            string testFileNameFullPath = Path.Combine(testLogFolder, TestContext.CurrentContext.Test.Name + ".log");
            if(File.Exists(testFileNameFullPath)) {
                try {
                    File.Delete(testFileNameFullPath);
                } catch(Exception e) {
                    Console.WriteLine($"Cannot cleanup the log file: {e}");
                }
            }

            File.Create(testFileNameFullPath).Close();

            WebTestContext.Set(Constants.TestLogFileKey, testFileNameFullPath);
        }

        private static void SetupScreenshotFileName() {
            string testScreenshotFullPath = Path.Combine(screenshotFolder, TestContext.CurrentContext.Test.Name + ".jpeg");
            if(File.Exists(testScreenshotFullPath)) {
                try {
                    File.Delete(testScreenshotFullPath);
                } catch(UnauthorizedAccessException e) {
                    Console.WriteLine("Cannot cleanup test screenshot file", e);
                } catch(Exception ex) {
                    Console.WriteLine("Cannot cleanup test screenshot file", ex);
                }
            }
            WebTestContext.Set(Constants.ScreenshotFileKey, Path.Combine(screenshotFolder, TestContext.CurrentContext.Test.Name + ".jpeg"));
        }

    }
}