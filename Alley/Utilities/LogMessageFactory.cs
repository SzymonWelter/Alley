using System.Diagnostics;

namespace Alley.Utilities
{
    internal static class LogMessageFactory
    {
        private const int SkipFrames = 1;
        public static string Create(string errorMessage)
        {
            var stackFrame = new StackTrace(SkipFrames);
            return $"Error: {errorMessage}, in: {stackFrame}";
        }
    }
}