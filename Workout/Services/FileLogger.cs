using System;
using System.Diagnostics;

namespace Workout.Services
{
    public static class FileLogger
    {
       
        public static void AppStarted()
        {
            Log("=== App Started ===");
        }

       
        public static void AppClosed()
        {
            Log("=== App Closed ===");
        }

       
        public static void Log(string message)
        {
            try
            {
                var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

                // Write to Output window in Visual Studio
                Debug.WriteLine(logEntry);
            }
            catch
            {
                // ignore logging errors
                //reminder to self, cant save toa file while running an emulator so have anything you add go to the output as its running
            }
        }
    }
}
