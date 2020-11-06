using System;
using Newtonsoft.Json;

namespace WebAutomation.Framework.Core.BaseSetup {
    public class TestEnvironmentParameters {
        public bool RS_LocalExecution { get; set; }
        public bool RS_LocalExecutionAsService { get; set; }

        public string RS_DeviceGroup { get; set; }
        public Uri ServerUri { get; set; }
       
        public string RS_BrowserName { get; set; }
        public string RS_ImplicitWaitTime { get; set; }
        
        public string RS_ServerHost { get; set; }
        public string RS_ServerPort { get; set; }
        
        public string RS_DriverServerExePath { get; set; }
        public string RS_ServerResource { get; set; }
        public override string ToString() {
            return JsonConvert.SerializeObject(this).Replace(",", ",\n  ").Replace("{", "{\n  ").Replace("}", "\n}");
        }
    }
}