using System;
using System.Runtime.Serialization;

namespace YouBeatMapper {
    [Serializable]
    internal class SaveValidationException : Exception {
        public SaveValidationException() {
        }

        public SaveValidationException(string message) : base(message) {
        }

        public SaveValidationException(string message, Exception innerException) : base(message, innerException) {
        }

        protected SaveValidationException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}