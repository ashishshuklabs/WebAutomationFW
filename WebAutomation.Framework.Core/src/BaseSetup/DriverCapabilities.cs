using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using WebAutomation.Framework.Core.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
[assembly: InternalsVisibleTo("WebAutomation.Framework.Core.Tests")]
namespace WebAutomation.Framework.Core.BaseSetup {

    public class DriverCapabilities {
        private readonly TestEnvironmentParameters context;
        private string browserName;
        private InternetExplorerOptions internetExplorerOptions;
        private ChromeOptions chromeOptions;
        private EdgeOptions edgeOptions;
        private SafariOptions safariOptions;
        private DriverOptions options;
        private bool isCapabilityRefreshNeeded;

        public DriverCapabilities(TestEnvironmentParameters testEnvironmentParameters) {
            this.context = testEnvironmentParameters;
            SetupBaseCapabilities();
        }
        private void SetupBaseCapabilities() {
            this.browserName = this.context.RS_BrowserName;
            if(string.IsNullOrEmpty(this.context.RS_BrowserName)) {
                throw new InvalidCapabilityException($"{nameof(this.context.RS_BrowserName)} capability is mandatory cannot be null or empty");
            }
            
            this.options = SetDefaultCapabilities();
        }
        internal void OverrideRunSettingsParams(RunParameterUpdater @params) {
            TestLogs.WriteLogSection("Overriding RunSettings",
                () => {
                    foreach(KeyValuePair<string, object> runSetting in @params.Get().Where(k => k.Key.Contains("RS_"))) {
                        System.Reflection.PropertyInfo contextProperty = this.context.GetType().GetProperty(runSetting.Key);
                        if(contextProperty != null && contextProperty.CanWrite) {
                            contextProperty.SetValue(this.context, runSetting.Value);
                            isCapabilityRefreshNeeded = true;
                            TestLogs.Write($"Overriding RunSettings Key: [{runSetting.Key}], New Value = [{runSetting.Value}]");

                        }
                    }
                });

            RefreshCapabilities();
        }
        private void RefreshCapabilities() {
            if(!isCapabilityRefreshNeeded) {
                return;
            }
            TestLogs.Write("Runsetting parameter change detected,Refreshing set capabilities now !!!");
            this.options = null;
            SetupBaseCapabilities();
        }
        public TestEnvironmentParameters GetEnvironmentContext() {
            return context;
        }
              
        /// <summary>
        /// Set additional capabilities on driver
        /// </summary>
        /// <param name="additionalCapability">key value pair for additional capabilities.</param>
        internal void SetAdditionalOptions(Dictionary<string, object> additionalCapability = null) {

            if(additionalCapability != null && additionalCapability.Count > 0) {
                ICapabilities existingCaps = this.options.ToCapabilities();
                //Log Section
                TestLogs.WriteLogSection("Test Specific Capabilities",
                    () => {
                        foreach(KeyValuePair<string, object> capability in additionalCapability) {
                            LogOverrideSetCapability(capability, existingCaps);
                            options.AddAdditionalCapability(capability.Key, capability.Value);
                           
                        }
                    });
            }
            
        }
        
        private void LogOverrideSetCapability(KeyValuePair<string, object> capability, ICapabilities caps) {
            if(caps.HasCapability(capability.Key)) {
                TestLogs.Write($"Overriding Capability, Key:[{capability.Key}], OldValue:[{caps.GetCapability(capability.Key)}], NewValue:[{capability.Value}]");
                return;
            }
            TestLogs.Write($"New Capability, Key:[{capability.Key}], Value:[{capability.Value}]");
        }

        // <summary>
        // Set Commandline arguments to be supplied for the browser. Only supported by chrome at the moment.
        // </summary>
        // <param name="arguments">arguments to add for the browser support</param>
        internal void SetArguments(params string[] arguments) {
            if(!this.context.RS_BrowserName.Equals("chrome", StringComparison.InvariantCultureIgnoreCase)) {
                return;
            }
            if(arguments != null && arguments.Length > 0) {

                foreach(string argument in arguments) {
                    ((ChromeOptions)this.options).AddArgument(argument);

                }
            }
        }
        
        
         /// <summary>
        /// Get all the set driver options for driver. Returns null, if type not found.
        /// </summary>
        /// <returns></returns>
        public T GetDriverOptions<T>() where T : DriverOptions {
            
            return options as T;
        }
                
        /// <summary>
        /// Sets up default browser capabilities
        /// </summary>
        /// <returns></returns>
        private DriverOptions SetDefaultCapabilities() {
            DriverOptions cap = null;

            switch(this.browserName.ToUpper()) {
                case "IE":
                    internetExplorerOptions = new InternetExplorerOptions();
                    internetExplorerOptions.EnableNativeEvents = true;
                    internetExplorerOptions.EnableNativeEvents = false;
                    internetExplorerOptions.SetLoggingPreference(LogType.Browser, LogLevel.All);                    
                    cap = internetExplorerOptions;
                    break;
                case "EDGE":
                    edgeOptions = new EdgeOptions();
                    edgeOptions.PlatformName = "WIN10";
                    edgeOptions.AddAdditionalCapability(nameof(edgeOptions.BrowserName), edgeOptions.BrowserName);
                    edgeOptions.AddAdditionalCapability(nameof(edgeOptions.PlatformName), edgeOptions.PlatformName);
                    cap = edgeOptions;
                    break;
                case "SAFARI":
                    safariOptions = new SafariOptions();
                    safariOptions.PlatformName = "iOS";
                    safariOptions.AddAdditionalCapability(nameof(safariOptions.PlatformName), safariOptions.PlatformName);
                    cap = safariOptions;
                    break;
                case "CHROME":
                    chromeOptions = new ChromeOptions();
                    chromeOptions.SetLoggingPreference(LogType.Browser, LogLevel.All);
                    chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
                    chromeOptions.PageLoadStrategy = PageLoadStrategy.Normal;
                    chromeOptions.AddArguments("--disable-impl-side-painting");
                    chromeOptions.AddArguments("--no-sandbox");
                    cap = chromeOptions;
                    break;
                default:
                    throw new InvalidCapabilityException($"Browser type [{this.context.RS_BrowserName}] is not supported. Supported types [chrome, safari]");

            }

            return cap;

        }
        /// <summary>
        /// Merge additional capabilities supplied by the user
        /// along with the default capabilities. .see AdditionalDriverOptions
        /// </summary>
        /// <param name="options">.seealso AdditionalDriverOptions</param>
        public void MergeCapabilities(RunParameterUpdater options) {
            if(options == null) {
                return;
            }
            //Overriding RunSettings
            if(options.Get().Any(k => k.Key.Contains("RS_"))) {
                //Regenerate Driver Capabilities
                OverrideRunSettingsParams(options);

            }
            
            PreRequisiteCheck();
        }

        private void PreRequisiteCheck() {
            Helpers.IsNullOrEmpty(context.ServerUri,"Server URI is mandatory and cannot be null or empty.");
            Helpers.IsNullOrEmpty(context.RS_BrowserName, "Browser name is mandatory and cannot be null or empty.");
        }
    }
}