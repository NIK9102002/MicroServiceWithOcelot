
using Serilog;

namespace eCommerce.SharedLibrary.Logging
{
    public static class LogException
    {
        public static void LogExceptions(Exception ex)
        {
            LogToFile(ex);
            LogToConsole(ex);
            LogToDebugger(ex);
        }

        private static void LogToFile(Exception ex)
        {
            Log.Information(ex.Message);
        }

        private static void LogToConsole(Exception ex)
        {
            Log.Warning(ex.Message);
        }

        private static void LogToDebugger(Exception ex)
        {
            Log.Debug(ex.Message);
        }
    }
}
