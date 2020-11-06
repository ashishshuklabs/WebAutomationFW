using System;
using System.Collections.Generic;
using System.IO;
using WebAutomation.Framework.Core.BaseSetup;
using WebAutomation.Framework.Core.Default;
using NUnit.Framework;

namespace WebAutomation.Framework.Core.Utilities {
    public static class TestLogs {
        public static void Attach() {
            if(!WebTestContext.ContainsKey(Constants.TestLogFileKey)) {
                TestContext.WriteLine($"No test logs generated, test logs will not be attached");
                return;
            }
            if(!File.Exists(WebTestContext.Get<string>(Constants.TestLogFileKey))) {
                TestLogs.Write("No test log file exists, no screenshot will be attached.");
                return;
            }
            TestContext.AddTestAttachment(WebTestContext.Get<string>(Constants.TestLogFileKey), "Test Log File");
        }
        private static void WriteToFile() {
            List<string> contents = WebTestContext.Get<List<string>>(Constants.LogsKey);
            if(contents == null || contents.Count == 0) {
                return;
            }
            File.AppendAllLines(WebTestContext.Get<string>(Constants.TestLogFileKey), contents);
        }
        /// <summary>
        /// Write content to the test log file
        /// </summary>
        /// <param name="content"></param>
        public static void Write(string content) {
            if(Convert.ToBoolean(WebTestContext.Get<string>(nameof(Constants.DevelopmentMode), false))) {
                return;
            }
            List<string> log = new List<string>() { content };
            Write(log);
            
        }
        /// <summary>
        /// Write content to the test log file
        /// </summary>
        /// <param name="content"></param>
        public static void Write(List<string> content) {
            if(Convert.ToBoolean(WebTestContext.Get<string>(nameof(Constants.DevelopmentMode), false))) {
                return;
            }
            IEnumerable<string> log = content;

            if(content == null) {
                return;
            }
            File.AppendAllLines(WebTestContext.Get<string>(Constants.TestLogFileKey), log);
        }
        /// <summary>
        /// Write content to file. File name with be appended to the test name.
        /// </summary>
        /// <param name="fileName">file name suffix</param>
        /// <param name="content">content to write to file</param>
        public static string Write(string fileName, string content) {
            if(Convert.ToBoolean(WebTestContext.Get<string>(nameof(Constants.DevelopmentMode), false))) {
                return string.Empty;
            }
            IEnumerable<string> log = new List<string>() { content };

            if(content == null) {
                return string.Empty;
            }
            string basePath = Path.Combine(Resources.BasePath, Constants.TestLogFolder, TestContext.CurrentContext.Test.Name + $"_{fileName}.log");

            if(File.Exists(basePath)) {
                File.Delete(basePath);
            }
            File.AppendAllLines(basePath, log);
            return basePath;
        }
        /// <summary>
        /// Write content within the specified section and close section when done
        /// </summary>
        /// <param name="tag">tag/title of the section</param>
        /// <param name="content">content to write within section. Generally speaking this section should have a call to Write method for logs to appear here.</param>
        public static void WriteLogSection(string tag, Action content) {
            AddSection(tag);
            content();
            AddSection(tag + " Ends");

        }
        /// <summary>
        /// Add a new section in log file
        /// </summary>
        /// <param name="section"></param>
        public static void AddSection(string section) {
            Write($"\n<-----{section}----->\n");
        }
        /// <summary>
        /// Write Device Logs
        /// </summary>
        //public static List<string> WriteSupportedLogs() {
            /*List<string> writtenFiles = new List<string>();
            ServerCommandExecutor executor = new ServerCommandExecutor();
            RemoteWebDriver driver = WebDriver.Instance;
            IReadOnlyCollection<string> availableContexts = driver.Manage().Logs.AvailableLogTypes;
            //Available contexts
            Write($"Available Contexts [{string.Join(',', availableContexts)}]");
            //Switch contexts and write logs to files for all types supported in context.
            foreach(string context in availableContexts) {
                driver.SwitchContextTo(context);
                Response preCheck = executor.Execute(StandardCommands.GetLogTypes);
                if(preCheck.Status != WebDriverResult.Success) {
                    continue;
                }
                string jsonToString = preCheck.ToJson().Split(']')[0].Split("\"Value\":[")[1].Replace("\"", " ").Replace(" ", "");
                string[] rawLogTypes = jsonToString.Split(',');//This should return appropriate string array
                string[] supportedLogTypes = rawLogTypes.FindAll(x => !x.Contains("bugreport"));//Not including bug report as it fails in some scenarios, weird. Found no use so far anyways. Look into this in future if need be.
                Response content = null;
                foreach(string logtype in supportedLogTypes) {
                    Write($"Fetching Log type [{logtype}]");
                    content = executor.Execute(StandardCommands.GetLog, new Dictionary<string, object> { { "type", $"{logtype}" } });
                    string rawContent = content.ToJson();
                    if(rawContent == null || rawContent.Contains("\"Value\":[]")) { //Empty logs with no useful information, discard
                        continue;
                    }
                    string formattedContent = FormattedDeviceLogs(rawContent);
                    writtenFiles.Add(Write(logtype, formattedContent));
                }

            }
            return writtenFiles;*/
        //}
    }
}
