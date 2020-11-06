using System;
using WebAutomation.Framework.Core.Default;
using Castle.DynamicProxy;

namespace WebAutomation.Framework.Core.Utilities {
    public class CreateInstance {
        private static ProxyGenerator generator;
        static CreateInstance() {
            generator = WebTestContext.Get<ProxyGenerator>(Constants.ProxyKey, false);
            if(generator == null) {
                WebTestContext.Set(Constants.ProxyKey, new ProxyGenerator());
                generator = WebTestContext.Get<ProxyGenerator>(Constants.ProxyKey);
            }
        }
        /// <summary>
        /// Create a proxied instance of a class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T Of<T>(params object[] @params) where T : class {
            LogInterceptor interceptor = new LogInterceptor();

            try {
                return generator.CreateClassProxy(typeof(T), @params, interceptor) as T;
            } catch(Exception ex) {
                string parameters = @params != null && @params.Length > 0 ? string.Join(',', @params) : "Empty";
                throw new Exception($"Cannot create instance of [{typeof(T).Name}] with parameters [{parameters}]. Exception : \n {ex}");
            }

        }

        private class LogInterceptor : IInterceptor {
            /// <summary>
            /// Intercept Method Calls
            /// </summary>
            /// <param name="invocation"></param>
            /// <exception cref="Exception"></exception>
            public void Intercept(IInvocation invocation) {
                BeforeInvocation(invocation);
                try {
                    invocation.Proceed();
                } catch(Exception e) {

                    string logcontent = $"[{invocation.Method.DeclaringType}.{invocation.Method.Name}], with" +
                        $"\nError:{e.Message} \n {e.StackTrace}";
                    TestLogs.Write("---------Script Call Failed =>" + logcontent + "---------");

                    throw new MethodExecutionFailedException($"Execution of method [{invocation.Method.Name}] failed", e);
                }
                AfterInvocation(invocation);
            }

            private void BeforeInvocation(IInvocation invocation) {
                if(invocation.Arguments.Length > 0) {
                    string log = $"[{invocation.Method.DeclaringType}.{invocation.Method.Name}] with Arguments:" +
                        $"[{BuildArgumentString(invocation, ",")}]";
                    TestLogs.Write("---------Script Call Initiated=>" + log + "---------");

                } else {
                    string log = $"[{invocation.Method.DeclaringType}.{invocation.Method.Name}]";
                    TestLogs.Write($"---------Script Call Initiated=>" + log + "---------");
                }
            }

            private void AfterInvocation(IInvocation invocation) {
                string logcontent = $"---------Script Call Successful =>[{invocation.Method.DeclaringType}.{invocation.Method.Name}] ";
                TestLogs.Write(logcontent + "---------");
            }

            private string BuildArgumentString(IInvocation invocation) {
                return BuildArgumentString(invocation, ".");
            }
            private string BuildArgumentString(IInvocation invocation, string separator) {
                string result = string.Empty;
                if(invocation.Arguments.Length > 0) {
                    foreach(object arg in invocation.Arguments) {
                        if(arg is string[]) {
                            foreach(string element in arg as string[]) {
                                result += element + separator;
                            }
                            return result.TrimEnd(separator.ToCharArray());
                        }

                        result += arg + separator;
                    }
                }

                return result.TrimEnd(separator.ToCharArray());
            }
        }

    }
}