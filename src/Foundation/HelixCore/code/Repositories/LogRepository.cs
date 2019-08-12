using System;
using NLog;
using ScHelix.Foundation.HelixCore.DI;
using Sitecore.Diagnostics;
using Logger = NLog.Logger;

namespace ScHelix.Foundation.HelixCore.Repositories {
    [Service(typeof(ILogRepository))]
    public class LogRepository : ILogRepository {
        private readonly Logger _logger;

        public LogRepository() => _logger = LogManager.GetLogger("ScAppLog");

        public LogRepository(string logName) => _logger = LogManager.GetLogger(logName);

        public void Debug(string message, Exception e = null) {
            Log.Debug(message, this);

            if (e != null) {
                _logger.Debug(e, message);
            } else {
                _logger.Debug(message);
            }
        }

        public void Info(string message, Exception e = null) {
            Log.Info(message, this);

            if (e != null) {
                _logger.Info(e, message);
            } else {
                _logger.Info(message);
            }
        }

        public void Warn(string message, Exception e = null) {
            if (e != null) {
                Log.Warn(message, e, this);
                _logger.Warn(e, message);
            } else {
                Log.Warn(message, this);
                _logger.Warn(message);
            }
        }

        public void SingleWarn(string message) {
            Log.SingleWarn(message, this);
            _logger.Warn(message);
        }

        public void Error(string message, Exception e = null) {
            if (e != null) {
                Log.Error(message, e, this);
                _logger.Error(e, message);
            } else {
                Log.Error(message, this);
                _logger.Error(message);
            }
        }

        public void SingleError(string message) {
            Log.SingleError(message, this);
            _logger.Error(message);
        }

        public void Fatal(string message, Exception e = null) {
            if (e != null) {
                Log.Fatal(message, e, this);
                _logger.Fatal(e, message);
            } else {
                Log.Fatal(message, this);
                _logger.Fatal(message);
            }
        }

        public void SingleFatal(string message, Exception e = null) {
            Log.SingleFatal(message, e, this);

            if (e != null) {
                _logger.Fatal(e, message);
            } else {
                _logger.Fatal(message);
            }
        }
    }
}
