using System;
using System.Runtime.CompilerServices;
using WebAutomation.Framework.Core.Default;
using WebAutomation.Framework.Core.Utilities;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
[assembly: InternalsVisibleTo("WebAutomation.Framework.Core.Tests")]
namespace WebAutomation.Framework.Core.BaseSetup {
    [TestFixture]
    public class DriverSetup {
        /// <summary>
        /// Environment parameters for the fixture
        /// </summary>
        protected TestEnvironmentParameters TestEnvironmentParameters { get; private set; }
        /// <summary>
        /// Driver capabilities for the tests.To update/add newer driver options use <see cref="DriverCapabilities.GetDriverOptions{T}"/> to get access to driver specific options instance. 
        /// </summary>
        public DriverCapabilities Capabilities { get; private set; }
        /// <summary>
        /// Used to specify/override driver capabilities
        /// </summary>
        public RunParameterUpdater RunParameterUpdater { get; set; }


        [OneTimeSetUp]
        public void OneTimeSetup() {
            Resources.SetupFolders();
            FixtureSetup();
        }

        public virtual void FixtureSetup() {
            TestEnvironmentParameters = new EnvironmentConfigurationReader().Get();

        }

        [SetUp]
        public void Setup() {
            SetupPreReq();
            TestSetup();
        }
        internal void SetupPreReq() {
            RunParameterUpdater = new RunParameterUpdater();
            Resources.SetupTestArtifacts();
            Capabilities = new DriverCapabilities(TestEnvironmentParameters);
        }
        public virtual void TestSetup() {
            //Start driver service
            DriverService.Start(this.TestEnvironmentParameters);
            //Merge user provided options to existing default capabilities.
            Capabilities.MergeCapabilities(RunParameterUpdater);
            // Write Runsettings to log file
            TestLogs.WriteLogSection("Original Test Run Parameters", () => TestLogs.Write(this.TestEnvironmentParameters.ToString()));
            WebDriver.Initialize(Capabilities);
            //Write device configuration to log file
            TestLogs.WriteLogSection("DUT Configuration", () => TestLogs.Write(WebDriver.Instance.Capabilities.ToString()));
            WebTestContext.Set(Constants.TestEnvironmentKey, TestEnvironmentParameters);
            TestLogs.AddSection($"Test {TestContext.CurrentContext.Test.Name} Starts");
        }

        [TearDown]
        public void TearDown() {
            TestTearDown();
        }

        public virtual void TestTearDown() {
            try {
                if(TestContext.CurrentContext.Result.Outcome != ResultState.Success) {
                    Screenshot.Attach();
                }
                TestLogs.AddSection($"Test {TestContext.CurrentContext.Test.Name} Ends");
                TestLogs.Attach();
                
            } catch(Exception e) {
                throw new Exception($"Teardown failed. Urgent attention required!!! Exception: \n {e}");
            } finally {
                WebDriver.Dispose();
                DriverService.Dispose();
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() {
            FixtureTearDown();
        }

        public virtual void FixtureTearDown() {
            
        }
    }
}
