using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace WebAutomation.Framework.Core.BaseSetup {
    public class RunParameterSetter {
        /// <summary>
        /// When set will override runsettings parameters and will let users
        /// add individual values using TestRunParameters.Set method.
        /// </summary>
        public static bool isDevelopmentMode = false;
        private static List<string> setKey = new List<string>();
        /// <summary>
        /// Get value corresponding to the key specified in either runsettings file or set
        /// directly by the user <see cref="isDevelopmentMode"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key) {
            if(isDevelopmentMode) {
                return Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process) ?? throw new KeyNotFoundException($"Required Parameter with key [{key}] not found!! Environemnt Details: [{nameof(isDevelopmentMode)} = {isDevelopmentMode}]");
            }
            return TestContext.Parameters.Get(key) ?? throw new KeyNotFoundException($"Required Parameter with key [{key}] not found!! Environemnt Details: [{nameof(isDevelopmentMode)} = {isDevelopmentMode}]");
        }
        /// <summary>
        /// Set new environment variable. Valid only in development mode and requires setting <see cref="isDevelopmentMode"/>
        /// </summary>
        /// <param name="key">Environment variable key</param>
        /// <param name="value">Environment variable value</param>
        /// /// <param name="overrideExisting">Override existing variable, default false</param>
        public static void Set(string key, string value, bool overrideExisting = false) {
            if(!isDevelopmentMode) {
                Console.WriteLine($"Development mode flag [{nameof(isDevelopmentMode)} = {isDevelopmentMode}]. Not allowed to set environment variables!!! ");
                return;
            }

            if(!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process))) {
                Console.WriteLine($"Environment variable:{key} is already set.");
                if(!overrideExisting) {
                    Console.WriteLine($"Ignore Environment variable set call with value [{value}].");
                    return;
                }
                Console.WriteLine($"Overriding Environment variable with value [{value}].");
            }
            Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);
            setKey.Add(key);
        }

        public static void Dispose() {
            if(setKey.Count == 0) {
                return;
            }
            foreach(string @key in setKey) {
                Environment.SetEnvironmentVariable(@key, null, EnvironmentVariableTarget.Process);
            }
        }
    }
}