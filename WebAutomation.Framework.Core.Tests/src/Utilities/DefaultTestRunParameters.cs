using WebAutomation.Framework.Core.BaseSetup;
using WebAutomation.Framework.Core.Default;
using System;

namespace WebAutomation.Framework.Core.Tests.Utilities {
    public class DefaultTestRunParameters {

        private static void SetDevModeDefaultParams() {
            RunParameterSetter.isDevelopmentMode = true;
            RunParameterSetter.Set(nameof(Constants.RS_ServerHost), Constants.RS_ServerHost);
            RunParameterSetter.Set(nameof(Constants.RS_ServerPort), Constants.RS_ServerPort);
            RunParameterSetter.Set(nameof(Constants.RS_ServerResource), Constants.RS_ServerResource);
            RunParameterSetter.Set(nameof(Constants.RS_ImplicitWaitTime), Constants.RS_ImplicitWaitTime);
            RunParameterSetter.Set(nameof(Constants.RS_BrowserName), Constants.RS_BrowserName);
            RunParameterSetter.Set(nameof(Constants.RS_DeviceGroup), Constants.RS_DeviceGroup);
            RunParameterSetter.Set(nameof(Constants.RS_LocalExecution), Constants.RS_LocalExecution);
            RunParameterSetter.Set(nameof(Constants.RS_LocalExecutionAsService), Constants.RS_LocalExecutionAsService);
            RunParameterSetter.Set(nameof(Constants.RS_DriverServerExePath), Constants.RS_DriverServerExePath);
        }
        /// <summary>
        /// Get Test Run Parameters. Ensure you call EnvironmentConfiguration's Get method again after this step
        /// for the changes to take effect in FW tests..
        /// </summary>
        public static void Get() {
            SetDevModeDefaultParams();
        }
        /// <summary>
        /// Set/override default test run parameters.
        /// </summary>
        /// <param name="key">parameter key</param>
        /// <param name="value">parameter value</param>
        public static void Set(string key, string value) {
            if(!RunParameterSetter.isDevelopmentMode) {
                throw new InvalidOperationException("Cannot update when default values not set. Call Get method first(and only once) before updating default values.");
            }
            if(string.IsNullOrEmpty(key)) {
                return;
            }
            RunParameterSetter.Set(key, value, true);
        }
    }
}
