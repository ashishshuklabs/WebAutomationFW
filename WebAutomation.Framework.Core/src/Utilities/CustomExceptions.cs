using System;
using System.Runtime.Serialization;

namespace WebAutomation.Framework.Core.Utilities {
    [Serializable]
    public class MethodExecutionFailedException : Exception {
        public MethodExecutionFailedException() { }
        public MethodExecutionFailedException(string message) : base(message) { }
        public MethodExecutionFailedException(string message, Exception inner) : base(message, inner) { }
        protected MethodExecutionFailedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
    [Serializable]
    public class ElementNotFoundException : Exception {
        public ElementNotFoundException() { }
        public ElementNotFoundException(string message) : base(message) { }
        public ElementNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ElementNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
    [Serializable]
    public class DriverServiceException : Exception {
        public DriverServiceException() { }
        public DriverServiceException(string message) : base(message) { }
        public DriverServiceException(string message, Exception inner) : base(message, inner) { }
        protected DriverServiceException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
    [Serializable]
    public class WebDriverInitializeException : Exception {
        public WebDriverInitializeException() {
        }

        public WebDriverInitializeException(string message) : base(message) {
        }

        public WebDriverInitializeException(string message, Exception innerException) : base(message, innerException) {
        }

        protected WebDriverInitializeException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
    [Serializable]
    public class InvalidCapabilityException : Exception {
        public InvalidCapabilityException() {
        }

        public InvalidCapabilityException(string message) : base(message) {
        }

        public InvalidCapabilityException(string message, Exception innerException) : base(message, innerException) {
        }

        protected InvalidCapabilityException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
