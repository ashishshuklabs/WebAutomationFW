using WebAutomation.Framework.Core.BaseSetup;
using WebAutomation.Framework.Core.Utilities;
using NUnit.Framework;
using System;

namespace WebAutomation.Framework.Core.Tests.UnitTests {
    [TestFixture]
    [Category("Unit Tests")]
    public class DriverServiceTests {
        [Test]
        public void NoLocalExecutionSetShouldNotStartService() {
            TestEnvironmentParameters paramerters = new TestEnvironmentParameters {
                RS_LocalExecution = false
            };
            DriverService.Start(paramerters);
            Assert.Null(paramerters.ServerUri, "Service should not be started when LocalExecution is not set.");
        }
        [Test]
        public void NoLocalExecutionAsServiceSetShouldNotStartService() {
            TestEnvironmentParameters paramerters = new TestEnvironmentParameters {
                RS_LocalExecution = true,
                RS_LocalExecutionAsService = false
            };
            DriverService.Start(paramerters);
            Assert.Null(paramerters.ServerUri, "Service should not be started when LocalExecutionAsService is not set.");
        }
        [Test]
        public void NoHostNameShouldThrowOnLocalExecutionAsService() {
            TestEnvironmentParameters paramerters = new TestEnvironmentParameters {
                RS_LocalExecution = true,
                RS_LocalExecutionAsService = true,
                RS_DriverServerExePath = "some random path",
                RS_BrowserName = "chrome"
            };
            var exception = Assert.Catch<ArgumentNullException>(() => DriverService.Start(paramerters), "Service should not be started when LocalExecutionAsService is not set.");
            Assert.That(exception.Message.Contains("RS_ServerHost"), Is.True, "Host name exception should have been thrown.");
        }
        [Test]
        public void NoPortNumberShouldThrowOnLocalExecutionAsService() {
            TestEnvironmentParameters paramerters = new TestEnvironmentParameters {
                RS_LocalExecution = true,
                RS_LocalExecutionAsService = true,
                RS_DriverServerExePath = "some random path",
                RS_ServerHost = "localhost",
                RS_BrowserName = "chrome"
            };
            var exception = Assert.Catch<ArgumentNullException>(() => DriverService.Start(paramerters), "Service should not be started when LocalExecutionAsService is not set.");
            Assert.That(exception.Message.Contains("RS_ServerPort"), Is.True, "Port number related exception should have been thrown.");
        }
        [Test]
        public void NoDriverExePathShouldThrowOnLocalExecutionAsService() {
            TestEnvironmentParameters paramerters = new TestEnvironmentParameters {
                RS_LocalExecution = true,
                RS_LocalExecutionAsService = true,
                RS_BrowserName = "chrome"
            };
            var exception = Assert.Catch<ArgumentNullException>(() => DriverService.Start(paramerters), "Service should not be started when DriverExe path is not set.");
            Assert.That(exception.Message.Contains("RS_DriverServerExePath"), Is.True, "Driver Server related exception should have been thrown.");
        }
        [Test]
        public void NoBrowserNameShouldThrowOnLocalExecutionAsService() {
            TestEnvironmentParameters paramerters = new TestEnvironmentParameters {
                RS_LocalExecution = true,
                RS_LocalExecutionAsService = true,
                
            };
            var exception = Assert.Catch<ArgumentNullException>(() => DriverService.Start(paramerters), "Service should not be started when browser name is not set.");
            Assert.That(exception.Message.Contains("RS_BrowserName"), Is.True, "Browser name exception should have been thrown.");
        }
        [Test]
        public void InvalidBrowserNameShouldThrowOnLocalExecutionAsService() {
            TestEnvironmentParameters paramerters = new TestEnvironmentParameters {
                RS_LocalExecution = true,
                RS_LocalExecutionAsService = true,
                RS_BrowserName = "Bionix",
                RS_DriverServerExePath = "Blah",
                RS_ServerHost = "Localhost",
                RS_ServerPort = "2334"
            };
            var exception = Assert.Catch<DriverServiceException>(() => DriverService.Start(paramerters), "Service should not be started when invalid browser name is provided");
            Assert.That(exception.Message.Contains("Bionix"), Is.True, "Browser name exception should have been thrown.");
        }
    }
}
