using WebAutomation.Framework.Core.BaseSetup;
using System;
using System.Collections.Generic;

namespace WebAutomation.Framework.Core.Utilities {
    public class WebTestContext {
        [ThreadStatic] private static Dictionary<string, object> properties;

        private static Dictionary<string, object> Properties {
            get {
                if(properties == null) {
                    properties = new Dictionary<string, object>();
                }

                return properties;
            }
        }

        /// <summary>
        /// Set the context variable
        /// </summary>
        /// <typeparam name="T">type of value to save</typeparam>
        /// <param name="key">key for stored value</param>
        /// <param name="value">value to store</param>
        public static void Set<T>(string key, T value) {
            //Add thread details if unavailable
            Helpers.IsNullOrEmpty(key, $"[{nameof(key)}] cannot be null or empty.");
            
            if(!Properties.ContainsKey(key)) {
                Properties.Add(key, value);
                return;
            }
            //Already contains a key, remove and refresh it with a new one.
            Properties.Remove(key);
            Properties.Add(key, value);

        }
        /// <summary>
        /// Get a stored context value
        /// </summary>
        /// <typeparam name="T">type of the value to be retrieved</typeparam>
        /// <param name="key">key corresponding to the value</param>
        /// <returns></returns>
        public static T Get<T>(string key) {
            return Get<T>(key, true);
        }

        public static T Get<T>(string key, bool throwOnKeyNotFound) {
            Helpers.IsNullOrEmpty(key, $"[{nameof(key)}] cannot be null or empty.");

            if(!Properties.ContainsKey(key)) {
                if(throwOnKeyNotFound) {
                    throw new KeyNotFoundException($"Key: [{key}] not found in the context.");
                }

                return default(T);
            }
            return (T)Properties[key];
        }
        /// <summary>
        /// Helper method to check if a key exists in the test context
        /// </summary>
        /// <param name="key">key to search for in test context</param>
        /// <returns></returns>
        public static bool ContainsKey(string key) {
            Helpers.IsNullOrEmpty(key, $"[{nameof(key)}] cannot be null or empty.");

            if(Properties.ContainsKey(key)) {
                return true;
            }

            return false;
        }
        
        public static void Dispose() {
            if(properties.ContainsKey(nameof(AderantWebDriver))) {
                properties[nameof(AderantWebDriver)] = null;
            }
            if(properties.ContainsKey(Constants.DriverServiceKey)) {
                AderantDriverService.Dispose();
                properties[Constants.DriverServiceKey] = null;
            }
            properties = null;

        }
    }
}
