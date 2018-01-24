using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace PricePoller
{
    public class MyLogger
    {
        private readonly ILogger _logger;

        public MyLogger(IConfiguration configuration)
        {
            _logger = new LoggerFactory()
                .AddConsole(configuration.GetSection("Logging"))
                .CreateLogger("CryptoBot.PricePoller");
        }

        public void LogInformation(string message)
        {
            Log(LogLevel.Information, message);
        }

        public void LogWarning(string warning)
        {
            Log(LogLevel.Warning, warning);
        }

        public void LogError(string error, Exception exception)
        {
            Log(LogLevel.Error, error, exception);
        }

        public void Log(LogLevel logLevel, string message)
        {
            Log(logLevel, message, null);
        }

        public void Log(LogLevel logLevel, string message, Exception exception)
        {
            _logger.Log(logLevel, 0, message, exception, (text, ex) => $"{DateTime.Now}: {text}");
        }
    }
}
