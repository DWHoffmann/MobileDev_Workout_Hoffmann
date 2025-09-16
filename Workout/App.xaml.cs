using Microsoft.Maui.Controls;
using Workout.Services;

namespace Workout
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Set the main page
            MainPage = new NavigationPage(new MainPage());

            // Log that the app has started
            FileLogger.AppStarted();
        }

        protected override void OnStart()
        {
            // This is called when the app starts
            FileLogger.Log("OnStart called");
        }

        protected override void OnSleep()
        {
            // This is called when the app goes to background or closes
            FileLogger.AppClosed();
        }

        protected override void OnResume()
        {
            // This is called when the app resumes from background
            FileLogger.Log("App resumed from background");
        }
    }
}
