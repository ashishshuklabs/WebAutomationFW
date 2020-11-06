using WebAutomation.Framework.Core.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;

namespace WebAutomation.Framework.Core.BaseSetup {
    public class CustomRemoteWebDriver : RemoteWebDriver {
        public CustomRemoteWebDriver(Uri remoteAddress, DriverOptions options) : base(remoteAddress, options) {
        }
        protected override Response Execute(string driverCommandToExecute, Dictionary<string, object> parameters) {
            
            bool logResponse = false;
            if(!driverCommandToExecute.Contains("newSession") && !driverCommandToExecute.Contains("setTimeouts") && !driverCommandToExecute.Contains("screenshot")) {
                if(parameters != null) {
                    TestLogs.Write($"Server Request => [{driverCommandToExecute}] with parameters:");
                    foreach(KeyValuePair<string, object> parameter in parameters) {
                        TestLogs.Write($"[{parameter.Key}" + " : " + $"{parameter.Value.ToString()}]");

                    }
                } else {
                    TestLogs.Write($"Server Request => [{driverCommandToExecute}].");
                }
                logResponse = true;
            }
            Response result = base.Execute(driverCommandToExecute, parameters);
            if(logResponse) {
                TestLogs.Write($"Server Response:\n[{result.ToJson()}]");
            }
            return result;
        }
    }
}