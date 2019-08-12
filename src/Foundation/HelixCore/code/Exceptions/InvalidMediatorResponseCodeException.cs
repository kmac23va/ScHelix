using System;

namespace ScHelix.Foundation.HelixCore.Exceptions {
    public class InvalidMediatorResponseCodeException : Exception {
        public InvalidMediatorResponseCodeException(string invalidMediatorCode)
            : base($"{Constants.MediatorResponse.InvalidCodeReturned}: {invalidMediatorCode}") {
        }
    }
}
