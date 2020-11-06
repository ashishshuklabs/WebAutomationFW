using WebAutomation.Framework.Core.BaseSetup;
using WebAutomation.Framework.Core.Default;
using WebAutomation.Framework.Core.Utilities;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using System;

namespace WebAutomation.Framework.Core.Tests.UnitTests {
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("Unit Tests")]
    public class DriverCapabilityUnitTests {
        [SetUp]
        public void Start() {
            WebTestContext.Set(nameof(Constants.DevelopmentMode), "true");
        }
        [Test]
        public void PassingInvalidBrowserThrows() {
            var context = new TestEnvironmentParameters {
                RS_BrowserName = "blowfish"
            };
            
            
            Assert.Throws<InvalidCapabilityException>(() => {
                DriverCapabilities cap = new DriverCapabilities(context);
            },
                $" Browser {context.RS_BrowserName} is not supported");
        }

        [Test]
        public void PassingValidBrowserPasses() {
            var context = new TestEnvironmentParameters {
                RS_BrowserName = "Chrome"
            };
           
            Assert.DoesNotThrow(() => {
                DriverCapabilities cap = new DriverCapabilities(context);
            },
                $"Valid browser should not throw");
        }
        [Test]
        public void PassingArgumentToChromePasses() {
            var context = new TestEnvironmentParameters {
                RS_BrowserName = "Chrome",
                ServerUri = Helpers.CreateUri("localhost", "1234", "/wd/hub/")
            };
            DriverCapabilities cap = new DriverCapabilities(context);
            RunParameterUpdater opt = new RunParameterUpdater();
            string argumentToAdd = "--somerandomStuff";
            cap.GetDriverOptions<ChromeOptions>().AddArgument(argumentToAdd);
            cap.MergeCapabilities(opt);
            Assert.True(cap.GetDriverOptions<ChromeOptions>().Arguments.IndexOf(argumentToAdd) > 0, "Argument should have been added");
            
        }
        [Test]
        public void PassingCapabilityToChromePasses() {
            var context = new TestEnvironmentParameters {
                RS_BrowserName = "Chrome",
                ServerUri = Helpers.CreateUri("localhost", "1234", "/wd/hub/")
            };
            DriverCapabilities cap = new DriverCapabilities(context);
            var c = cap.GetDriverOptions<ChromeOptions>();
            c.PlatformName = "WIN10";
            Assert.AreEqual(cap.GetDriverOptions<ChromeOptions>().PlatformName.ToUpper(), "WIN10", "Argument should have been added from Chrome");

        }
        [Test]
        public void PassingCapabilityToEdgePasses() {
            var context = new TestEnvironmentParameters {
                RS_BrowserName = "edge",
                ServerUri = Helpers.CreateUri("localhost", "1234", "/wd/hub/")
            };
            DriverCapabilities cap = new DriverCapabilities(context);
            var c = cap.GetDriverOptions<EdgeOptions>();
            c.UseInPrivateBrowsing = true;
            Assert.AreEqual(cap.GetDriverOptions<EdgeOptions>().UseInPrivateBrowsing,true,  "Capability should have been added/updated for Edge");

        }
        [Test]
        public void PassingCapabilityToSafariPasses() {
            var context = new TestEnvironmentParameters {
                RS_BrowserName = "safari",
                ServerUri = Helpers.CreateUri("localhost", "1234", "/wd/hub/")
            };
            DriverCapabilities cap = new DriverCapabilities(context);
            var c = cap.GetDriverOptions<SafariOptions>();
            c.EnableAutomaticInspection = true;
            Assert.AreEqual(cap.GetDriverOptions<SafariOptions>().EnableAutomaticInspection, true, "Capability should have been added for Safari Options");

        }
        [Test]
        public void PassingCapabilityToIEPasses() {
            var context = new TestEnvironmentParameters {
                RS_BrowserName = "IE",
                ServerUri = Helpers.CreateUri("localhost", "1234", "/wd/hub/")
            };
            DriverCapabilities cap = new DriverCapabilities(context);
            var c = cap.GetDriverOptions<InternetExplorerOptions>();
            c.EnableNativeEvents = true;
            Assert.AreEqual(cap.GetDriverOptions<InternetExplorerOptions>().EnableNativeEvents, true, "Capabilitye should have been added for IEOptions");

        }
        [Test]
        public void PassingAdditionalOptionsToOverrideRunSettingsDriverParamsPasses() {

            var context = new TestEnvironmentParameters {
                RS_BrowserName = "edge",
                RS_DeviceGroup = "edge;win10",
                RS_DriverServerExePath = "somePath",
                RS_ImplicitWaitTime = "100",
                RS_LocalExecution = true,
                RS_LocalExecutionAsService = true
            };
          
            DriverCapabilities cap = new DriverCapabilities(context);
            RunParameterUpdater options = new RunParameterUpdater();
            options.Update(nameof(Constants.RS_BrowserName), Constants.RS_BrowserName)
                .Update(nameof(Constants.RS_DeviceGroup), Constants.RS_DeviceGroup)
                .Update(nameof(Constants.RS_DriverServerExePath), Constants.RS_DriverServerExePath)
                .Update(nameof(Constants.RS_ImplicitWaitTime), Constants.RS_ImplicitWaitTime)
                .Update(nameof(Constants.RS_LocalExecution), Convert.ToBoolean(Constants.RS_LocalExecution))
                .Update(nameof(Constants.RS_LocalExecutionAsService), Convert.ToBoolean(Constants.RS_LocalExecutionAsService));
            cap.OverrideRunSettingsParams(options);
            Assert.AreEqual(context.RS_BrowserName, Constants.RS_BrowserName,$"{nameof(Constants.RS_BrowserName)} should have been updated.");
            Assert.AreEqual(context.RS_DeviceGroup, Constants.RS_DeviceGroup,$"{nameof(Constants.RS_DeviceGroup)} should have been updated.");
            Assert.AreEqual(context.RS_DriverServerExePath, Constants.RS_DriverServerExePath, $"{nameof(Constants.RS_DriverServerExePath)} should have been updated.");
            Assert.AreEqual(context.RS_ImplicitWaitTime, Constants.RS_ImplicitWaitTime, $"{nameof(Constants.RS_ImplicitWaitTime)} should have been updated.");
            Assert.AreEqual(context.RS_LocalExecution, Convert.ToBoolean(Constants.RS_LocalExecution), $"{nameof(Constants.RS_LocalExecution)} should have been updated.");
            Assert.AreEqual(context.RS_LocalExecutionAsService, Convert.ToBoolean(Constants.RS_LocalExecutionAsService), $"{nameof(Constants.RS_LocalExecutionAsService)} should have been updated.");


        }
        [Test]
        public void PassingAdditionalOptionsToOverrideRunSettingsServerParamsPasses() {

            var context = new TestEnvironmentParameters {
                RS_BrowserName = "chrome",
                RS_ServerHost = "someHost",
                RS_ServerPort = "1234",
                RS_ServerResource = "/abd/def"
            };

            DriverCapabilities cap = new DriverCapabilities(context);
            RunParameterUpdater options = new RunParameterUpdater();
            options
                .Update(nameof(Constants.RS_ServerHost), Constants.RS_ServerHost)
                .Update(nameof(Constants.RS_ServerPort), Constants.RS_ServerPort)
                .Update(nameof(Constants.RS_ServerResource), Constants.RS_ServerResource);
            cap.OverrideRunSettingsParams(options);
           
            Assert.AreEqual(context.RS_ServerHost, Constants.RS_ServerHost, $"{nameof(Constants.RS_ServerHost)} should have been updated.");
            Assert.AreEqual(context.RS_ServerPort,Constants.RS_ServerPort, $"{nameof(Constants.RS_ServerPort)} should have been updated.");
            Assert.AreEqual(context.RS_ServerResource, Constants.RS_ServerResource, $"{nameof(Constants.RS_ServerResource)} should have been updated.");
            context.ServerUri = Helpers.CreateUri(context.RS_ServerHost, context.RS_ServerPort, context.RS_ServerResource);
            Assert.AreEqual(context.ServerUri.Host,Constants.RS_ServerHost, $"{nameof(context.ServerUri.Host)} should have been updated.");
            Assert.AreEqual(context.ServerUri.Port, Convert.ToInt32(Constants.RS_ServerPort), $"{nameof(context.ServerUri.Port)} should have been updated.");
            Assert.AreEqual(context.ServerUri.PathAndQuery, Constants.RS_ServerResource, $"{nameof(context.ServerUri.PathAndQuery)} should have been updated.");



        }
        [Test]
        public void NullBrowserNameAtInstantiationShouldThrow() {
            TestEnvironmentParameters @params = new TestEnvironmentParameters();
            Assert.Throws<InvalidCapabilityException>(() => new DriverCapabilities(@params), "should not be allowed to create instance if browser name is null or empty" );
        }
        [Test]
        public void NullBrowserNameOnOverridingShouldThrow() {
            TestEnvironmentParameters @params = new TestEnvironmentParameters() {
                RS_BrowserName = "chrome"
            };
            var cap = new DriverCapabilities(@params);
            RunParameterUpdater opts = new RunParameterUpdater();
            opts.Update(nameof(Constants.RS_BrowserName), "");
            Assert.Throws<InvalidCapabilityException>(() => cap.MergeCapabilities(opts), "Should not be allowed to create instance if browser name is null or empty");
        }
        [Test]
        public void NullServerHostOnOverridingShouldThrow() {
            TestEnvironmentParameters @params = new TestEnvironmentParameters() {
                RS_BrowserName = "chrome"
            };
            var cap = new DriverCapabilities(@params);
            RunParameterUpdater opts = new RunParameterUpdater();
            opts.Update(nameof(@params.ServerUri), null);
            Assert.Throws<ArgumentNullException>(() => cap.MergeCapabilities(opts), "Should not be allowed to create instance if host is null or empty");
        }
        [Test]
        public void PassingEdgeBrowserNameCreatesEdgeCapability() {
            TestEnvironmentParameters @params = new TestEnvironmentParameters() {
                RS_BrowserName = "edge"
            };
            var cap = new DriverCapabilities(@params);
            Assert.NotNull(cap.GetDriverOptions<EdgeOptions>(), "Should have created edge options.");
            Assert.Null(cap.GetDriverOptions<ChromeOptions>(),"Invalid option should not be resolved.");
        }
        [Test]
        public void PassingSafariBrowserNameCreatesSafariCapability() {
            TestEnvironmentParameters @params = new TestEnvironmentParameters() {
                RS_BrowserName = "safari"
            };
            var cap = new DriverCapabilities(@params);
            Assert.NotNull(cap.GetDriverOptions<SafariOptions>(), "Should have created safari options.");
            Assert.Null(cap.GetDriverOptions<ChromeOptions>(), "Invalid option should not be resolved.");
        }
        [Test]
        public void PassingIEBrowserNameCreatesIECapability() {
            TestEnvironmentParameters @params = new TestEnvironmentParameters() {
                RS_BrowserName = "ie"
            };
            var cap = new DriverCapabilities(@params);
            Assert.NotNull(cap.GetDriverOptions<InternetExplorerOptions>(), "Should have created IE options.");
            Assert.Null(cap.GetDriverOptions<ChromeOptions>(), "Invalid option should not be resolved.");
        }
        //Write tests here and in mobile framework to validate if null server details or other required parameters are not supplied.
    }
}
