using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace WebAutomation.Framework.Core.BaseSetup {
    public class RunParameterUpdater {
        public RunParameterUpdater() {
            this.runSettings = new Dictionary<string, object>();
        }

        private Dictionary<string, object> runSettings;
        /// <summary>
        /// Add additional Capabilities
        /// </summary>
        /// <param name="key">capability key</param>
        /// <param name="value">value for capability</param>
        /// <returns></returns>
        public RunParameterUpdater Update(string key, object value) {
            if(string.IsNullOrEmpty(key)) {
                throw new ArgumentNullException("Key cannot be empty/null");
            }
            if(this.runSettings.ContainsKey(key)) {
                TestContext.WriteLine($"Key [{key}] already exists with value [{this.runSettings[key]}], overwriting with [{value}] ");
                this.runSettings.Remove(key);
            }
            this.runSettings.Add(key, value);
            return this;
        }
                
        /// <summary>
        /// Get overriding entries
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> Get() {
            return this.runSettings;
        }
    }
    

}