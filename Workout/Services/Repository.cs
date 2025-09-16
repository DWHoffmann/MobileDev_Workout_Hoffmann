using System;
using System.Collections.ObjectModel;
using Workout.Models;

namespace Workout.Services
{
    public static class Repository
    {
        // Pre-made diet entry: 10 calories, 1 serving, 1 protein, 1 carb
        public static ObservableCollection<DietModel> DietEntries { get; } = new()
        {
            new DietModel
            {
                Meal = "Pre-made Snack",
                Calories = 10,
                Protien = 1,
                Carbs = 1,
                Servings = 1
            }
        };

        // Pre-made workout entry: 100 calories, 60 minutes
        public static ObservableCollection<WorkoutModel> WorkoutEntries { get; } = new()
        {
            new WorkoutModel
            {
                WorkoutType = "Running",
                Date = DateTime.Now,
                MinutesWorkout = 60,
                CalroiesBurnt = 100
            }
        };

        // Pre-made goal: Calories Lost, goal 110 calories
        public static ObservableCollection<GoalsModel> Goals { get; } = new()
        {
            new GoalsModel
            {
                GoalType = "Calories Lost",
                TargetCalories = 110,
                TargetDate = DateTime.Now.AddDays(7),
                Notes = "Pre-made goal entry"
            }
        };
    }
}
