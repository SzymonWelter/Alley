using System.Diagnostics;
using Serilog;

namespace Alley.Utils
{
    public class AlleyLogger : IAlleyLogger
    {
        private readonly ILogger _logger;

        public AlleyLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Error(string message)
        {
            _logger.Error(GetErrorMessage(message));
        }

        public void Information(string message)
        {
            _logger.Information(GetInformationMessage(message));
        }

        public void Warning(string message)
        {
            _logger.Information(GetWarningMessage(message));
        }

        private string GetErrorMessage(string errorMessage)
        {
#if DEBUG
            var stackTrace = new StackTrace(1).ToString();
#endif

            return $"Error occured: {errorMessage} {stackTrace}";
        }

        private string GetInformationMessage(string message)
        {
            return $"Info: {message}";
        }

        private string GetWarningMessage(string message)
        {
            return $"Warning: {message}";
        }
    }
}