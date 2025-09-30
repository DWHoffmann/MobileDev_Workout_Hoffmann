using Microsoft.Extensions.Logging;
using Workout.Models;
using Workout.ViewModels;
using Workout.Views;

namespace Workout
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // Models and ViewModels stay as singletons if needed
            builder.Services.AddSingleton<DietModel>();
            builder.Services.AddSingleton<WorkoutModel>();
            builder.Services.AddSingleton<GoalsModel>();
            builder.Services.AddSingleton<DietViewModel>();
            builder.Services.AddSingleton<WorkoutViewModel>();
            builder.Services.AddSingleton<GoalViewModel>();

            // Views should be transient
            builder.Services.AddTransient<DietView>();
            builder.Services.AddTransient<WorkoutView>();
            builder.Services.AddTransient<GoalView>();

            // MainPage can remain singleton if it's your app shell root
            builder.Services.AddSingleton<MainPage>();



            return builder.Build();
        }
    }
}
