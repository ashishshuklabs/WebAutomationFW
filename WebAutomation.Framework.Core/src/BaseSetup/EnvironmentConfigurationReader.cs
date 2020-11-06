using WebAutomation.Framework.Core.Default;
using WebAutomation.Framework.Core.Utilities;
using System;

namespace WebAutomation.Framework.Core.BaseSetup {
    public class EnvironmentConfigurationReader {
        private string RS_BrowserName { get; }
        private string ImplicitWaitTime { get; }
        private string RS_ServerHost { get; }
        private string RS_ServerResource { get; }

        private string RS_ServerPort { get; }
        private string RS_DeviceGroup { get; }
        private string RS_LocalExecution { get; }
        private string RS_LocalExecutionAsService { get; }
        private string RS_DriverServerExePath { get; }
        public EnvironmentConfigurationReader() {
            this.RS_BrowserName = RunParameterSetter.Get(nameof(Constants.RS_BrowserName)) ?? Constants.RS_BrowserName;
            this.ImplicitWaitTime = RunParameterSetter.Get(nameof(Constants.RS_ImplicitWaitTime)) ?? Constants.RS_ImplicitWaitTime;
            this.RS_ServerPort = RunParameterSetter.Get(nameof(Constants.RS_ServerPort)) ?? Constants.RS_ServerPort;
            this.RS_ServerHost = RunParameterSetter.Get(nameof(Constants.RS_ServerHost)) ?? Constants.RS_ServerHost;
            this.RS_ServerResource = RunParameterSetter.Get(nameof(Constants.RS_ServerResource)) ?? Constants.RS_ServerResource;
            this.RS_DeviceGroup = RunParameterSetter.Get(nameof(Constants.RS_DeviceGroup)) ?? Constants.RS_DeviceGroup;
            this.RS_LocalExecution = RunParameterSetter.Get(nameof(Constants.RS_LocalExecution)) ?? Constants.RS_LocalExecution;
            this.RS_LocalExecutionAsService = RunParameterSetter.Get(nameof(Constants.RS_LocalExecutionAsService)) ?? Constants.RS_LocalExecutionAsService;
            this.RS_DriverServerExePath = RunParameterSetter.Get(nameof(Constants.RS_DriverServerExePath)) ?? Constants.RS_DriverServerExePath;
        }

        /// <summary>
        /// Get Environment Details upon reading runsettings file
        /// EnvironmentContext and prefixed setting on default.
        /// </summary>
        /// <returns></returns>
        public TestEnvironmentParameters Get() {
            return new TestEnvironmentParameters {
                RS_BrowserName = Helpers.SetBrowserType(this.RS_BrowserName),
                ServerUri = Helpers.CreateUri(this.RS_ServerHost, this.RS_ServerPort, this.RS_ServerResource),
                RS_ImplicitWaitTime = this.ImplicitWaitTime,
                RS_ServerPort = this.RS_ServerPort,
                RS_ServerHost = this.RS_ServerHost,
                RS_ServerResource = this.RS_ServerResource,
                RS_DeviceGroup = this.RS_DeviceGroup,
                RS_LocalExecution = Convert.ToBoolean(this.RS_LocalExecution),
                RS_DriverServerExePath = this.RS_DriverServerExePath,
                RS_LocalExecutionAsService = Convert.ToBoolean(RS_LocalExecutionAsService)
            };
        }

    }
}