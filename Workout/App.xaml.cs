using Microsoft.Maui.Controls;
using Workout.Resources.Styles;
using Workout.Services;

namespace Workout
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Apply the saved theme BEFORE creating any pages
            ThemeManager.ApplyTheme(ThemeManager.IsLightTheme);

            // Set the main page after the correct theme is applied
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
