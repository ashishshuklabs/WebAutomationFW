using WebAutomation.Framework.Core.Default;
using WebAutomation.Framework.Core.Utilities;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using System;

namespace WebAutomation.Framework.Core.BaseSetup {
    public class DriverService {
        /// <summary>
        /// Start Browser Server for local execution. Updates the ref parameter <seealso cref=" TestEnvironmentParameters.ServerUri"/> accordingly and sets up the WebTestContext with parameter with key <see cref="Constants.DriverServiceKey"/> of type <see cref="OpenQA.Selenium.DriverService"/>
        /// </summary>
        /// <param name="parameters">Test environment parameters</param>
        public static void Start(TestEnvironmentParameters parameters) {
            if(!parameters.RS_LocalExecution) {
                return;
            }
            if(!parameters.RS_LocalExecutionAsService) {
                return;
            }
            if(string.IsNullOrEmpty(parameters.RS_BrowserName)) {
                throw new ArgumentNullException($" [{nameof(parameters.RS_BrowserName)}] is mandatory for local execution");
            }
            if(string.IsNullOrEmpty(parameters.RS_DriverServerExePath)) {
                throw new ArgumentNullException($" [{nameof(parameters.RS_DriverServerExePath)}] is mandatory for local execution");
            }
            if(string.IsNullOrEmpty(parameters.RS_ServerHost)) {
                throw new ArgumentNullException($" [{nameof(parameters.RS_ServerHost)}] is mandatory for local execution");
            }
            if(string.IsNullOrEmpty(parameters.RS_ServerPort)) {
                throw new ArgumentNullException($" [{nameof(parameters.RS_ServerPort)}] is mandatory for local execution");
            }
            StartBrowserService(parameters);
            
        }
        /// <summary>
        /// Start browser specific driver service
        /// </summary>
        /// <param name="parameters"><see cref="TestEnvironmentParameters"/></param>
        private static void StartBrowserService(TestEnvironmentParameters parameters) {

            OpenQA.Selenium.DriverService service;
            
            switch(parameters.RS_BrowserName.ToUpper()) {
                case "CHROME":
                    service = ChromeDriverService.CreateDefaultService(parameters.RS_DriverServerExePath);
                    service.HostName = parameters.RS_ServerHost;
                    service.Port = Convert.ToInt32(parameters.RS_ServerPort);
                  
                    Get(() => service.Start(),"Chrome");
                    break;
                case "IE":
                    service = InternetExplorerDriverService.CreateDefaultService(parameters.RS_DriverServerExePath);
                    service.HostName = parameters.RS_ServerHost;
                    service.Port = Convert.ToInt32(parameters.RS_ServerPort);
                    Get(() => service.Start(), "ie");
                    break;
                case "EDGE":
                    service = EdgeDriverService.CreateDefaultService(parameters.RS_DriverServerExePath);
                    service.HostName = parameters.RS_ServerHost;
                    service.Port = Convert.ToInt32(parameters.RS_ServerPort);
                    Get(() => service.Start(), "Edge");
                    break;
                case "SAFARI":
                    service = SafariDriverService.CreateDefaultService(parameters.RS_DriverServerExePath);
                    service.HostName = parameters.RS_ServerHost;
                    service.Port = Convert.ToInt32(parameters.RS_ServerPort);
                    Get(() => service.Start(), "Safari");
                    break;
                default:
                    throw new DriverServiceException($"Driver for {parameters.RS_BrowserName} not supported.");
            }

            WebTestContext.Set(Constants.DriverServiceKey, service);
            parameters.ServerUri = service.ServiceUrl;
        }

        private static void Get(Action method, string browser) {
            try {
                method();
            } catch(Exception e) {
                throw new DriverServiceException($"Cannot start {browser} driver service.Exception:\n{e}");
            }
        }

        public static void Dispose() {
            WebTestContext.Get<OpenQA.Selenium.DriverService>(Constants.DriverServiceKey, false)?.Dispose();
        }
    }
}
