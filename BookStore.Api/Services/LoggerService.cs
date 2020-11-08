using BookStore.Api.Contracts;
using NLog;

namespace BookStore.Api.Services
{
    public class LoggerService : ILoggerService
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        public void LogDebug(string mesagge)
            => _logger.Debug(mesagge);

        public void LogError(string mesagge)
            => _logger.Error(mesagge);

        public void LogInfo(string mesagge)
            => _logger.Info(mesagge);

        public void LogWarn(string mesagge)
            => _logger.Warn(mesagge);
    }
}
