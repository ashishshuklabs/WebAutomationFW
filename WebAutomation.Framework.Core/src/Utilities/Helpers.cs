using WebAutomation.Framework.Core.Default;
using System;

namespace WebAutomation.Framework.Core.Utilities {

    public class Helpers {
        public static string SetBrowserType(string browserName) {
            switch(browserName.ToUpper()) {
                case "CHROME":
                    return BrowserType.CHROME;
                case "IE":
                    return BrowserType.IE;
                case "SAFARI":
                    return BrowserType.SAFARI;
                case "EDGE":
                    return BrowserType.EDGE;
                default:
                    throw new NotSupportedException($"Browser [{browserName}] is not supported by the FW.");
            }
        }
        /// <summary>
        /// Utility to create URI using provided hostName, port and resource
        /// </summary>
        /// <param name="hostName">Hostname</param>
        /// <param name="port">port</param>
        /// <param name="resource">resource</param>
        /// <returns></returns>
        public static Uri CreateUri(string hostName, string port, string resource) {
            return new Uri(@"http://" + hostName + ":" +
                        port + resource);
        }
        /// <summary>
        /// Check if the specific instance is null or empty. Throws on exception
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="type">instance whos value is to be validated</param>
        /// <param name="customMessageInException">Optional custom message to be be added to default exception.</param>
        public static void IsNullOrEmpty<T>(T type, string customMessageInException = "") {
            IsNullOrEmpty(type, true, customMessageInException);
        }
        /// <summary>
        /// Check if the specific instance is null or empty. Throws on exception
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="type">instance whos value is to be validated</param>
        /// <param name="throwOnFailure">flag to indicate if exception to be thrown on failure, default true.</param>
        /// <param name="customMessageInException">optional custom exception to be added to exception</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(T type, bool throwOnFailure, string customMessageInException = "") {
            bool isFailCheck = false;
            if(type == null) {
                isFailCheck = true;
                if(!throwOnFailure) {
                    return isFailCheck;
                }
                throw new ArgumentNullException($"\n{customMessageInException}\nArgument [{typeof(T).Name}] is null.");
            }

            if(typeof(T).Name.Equals("string", StringComparison.OrdinalIgnoreCase)) {
                if(!string.IsNullOrEmpty(type.ToString())) {
                    return isFailCheck;
                }
                isFailCheck = true;
                if(!throwOnFailure) {
                    return isFailCheck;
                }
                throw new ArgumentNullException($"\n{customMessageInException}\nArgument [{typeof(T).Name}] is Empty.");
            }
            return isFailCheck;
        }
    }
}
