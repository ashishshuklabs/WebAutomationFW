namespace WebAutomation.Framework.Core.Default {
    public sealed class BrowserType {
        public static string CHROME = "Chrome";
        public static string SAFARI = "Safari";
        public static string EDGE = "Edge";
        public static string IE = "IE";

    }
    public class Constants {
        public const string TestLogFolder = "TestLogs";
        public const string ScreenshotFolder = "Screenshots";
        public const string TestLogFileKey = "TestLogFile";
        public const string ScreenshotFileKey = "ScreenshotFile";
        public const string LogsKey = "Logs";
        public const string WaitTimeKey = "WaitTime";
        public const string BrowserCapabilityKey = "BrowserCapability";
        public const string ProxyKey = "Proxy";
        public const int DefaultWaitTime = 10;
        public const string RS_BrowserName = "chrome";
        public const string RS_ImplicitWaitTime = "0";
        public const string RS_ServerPort = "4278";
        public const string RS_ServerHost = "localhost";
        public const string RS_ServerResource = "/wd/hub/";
        public const string RS_DeviceGroup = "chrome;windows";
        public const string AdditionalOptionsKey = "AdditionalOptions";
        public const string DevelopmentMode = "false";
        public const string DriverServiceKey = "DriverService";
        public const string RS_LocalExecution = "false";
        public const string RS_LocalExecutionAsService = "false";
        public const string RS_DriverServerExePath = "some folder with server exe";
        internal static string TestEnvironmentKey =  "TestEnvironmentParameters";
    }
}
