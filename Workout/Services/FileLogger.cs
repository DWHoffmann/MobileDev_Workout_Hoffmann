using System;
using System.Diagnostics;

namespace Workout.Services
{
    public static class FileLogger
    {
        public static void AppStarted()
        {
            Log("=== APP STARTED ===");
        }

        public static void AppClosed()
        {
            Log("=== APP CLOSED ===");
        }

        public static void Log(string message)
        {
            try
            {
                // Format message to stand out in Output window
                var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] >>> {message} <<<";

                // Write to Output window in Visual Studio
                Debug.WriteLine(logEntry);
            }
            catch
            {
                // ignore logging errors
            }
        }
    }
}
