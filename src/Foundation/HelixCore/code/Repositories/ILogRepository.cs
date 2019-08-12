using System;

namespace ScHelix.Foundation.HelixCore.Repositories {
    public interface ILogRepository {
        void Debug(string message, Exception e = null);
        void Info(string message, Exception e = null);
        void Warn(string message, Exception e = null);
        void SingleWarn(string message);
        void Error(string message, Exception e = null);
        void SingleError(string message);
        void Fatal(string message, Exception e = null);
        void SingleFatal(string message, Exception e = null);
    }
}
