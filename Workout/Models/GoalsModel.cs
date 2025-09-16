using System;
using Microsoft.Maui.Controls;
using Workout.Services;
using System.Linq;

namespace Workout.Models
{
    public class GoalsModel : BindableObject
    {
        public string GoalType { get; set; }
        public int? TargetCalories { get; set; }
        public int? TargetExerciseMinutes { get; set; }
        public int? TargetMuscleWeight { get; set; }
        public string TargetMuscleAppearance { get; set; }
        public int? TargetWeight { get; set; }
        public DateTime? TargetDate { get; set; }
        public string Notes { get; set; }

        public int? CurrentMuscleWeight { get; set; }
        public int? CurrentWeight { get; set; }

        public double Progress
        {
            get
            {
                if (TargetDate.HasValue && TargetDate.Value < DateTime.Now)
                    return 1; 

                return GoalProgressCalculator.Calculate(this);
            }
        }

        public string ProgressText => GoalProgressCalculator.CalculateText(this);

        public void RefreshProgress()
        {
            OnPropertyChanged(nameof(Progress));
            OnPropertyChanged(nameof(ProgressText));
        }
    }

    public static class GoalProgressCalculator
    {
        public static double Calculate(GoalsModel goal)
        {
            double progress = 0;

            switch (goal.GoalType)
            {
                case "Calories Lost":
                    int totalBurnt = Repository.WorkoutEntries.Sum(w => w.CalroiesBurnt);
                    int totalConsumed = Repository.DietEntries.Sum(d => d.TotalCalories);
                    int netCalories = totalBurnt - totalConsumed;
                    if (goal.TargetCalories.HasValue && goal.TargetCalories.Value > 0)
                        progress = Math.Min((double)netCalories / goal.TargetCalories.Value, 1);
                    break;

                case "Exercise Minutes":
                    int totalMinutes = Repository.WorkoutEntries.Sum(w => w.MinutesWorkout);
                    if (goal.TargetExerciseMinutes.HasValue && goal.TargetExerciseMinutes.Value > 0)
                        progress = Math.Min((double)totalMinutes / goal.TargetExerciseMinutes.Value, 1);
                    break;

                case "Muscle Gain":
                    if (goal.CurrentMuscleWeight.HasValue && goal.TargetMuscleWeight.HasValue)
                        progress = Math.Min((double)goal.CurrentMuscleWeight.Value / goal.TargetMuscleWeight.Value, 1);
                    break;

                case "Lose Weight":
                    if (goal.CurrentWeight.HasValue && goal.TargetWeight.HasValue)
                        progress = Math.Min((double)goal.CurrentWeight.Value / goal.TargetWeight.Value, 1);
                    break;
            }

            return Math.Max(progress, 0);
        }

        public static string CalculateText(GoalsModel goal)
        {
            switch (goal.GoalType)
            {
                case "Calories Lost":
                    int totalBurnt = Repository.WorkoutEntries.Sum(w => w.CalroiesBurnt);
                    int totalConsumed = Repository.DietEntries.Sum(d => d.TotalCalories);
                    int netCalories = totalBurnt - totalConsumed;
                    return $"Progress: {Math.Max(netCalories, 0)}/{goal.TargetCalories ?? 0} cal";

                case "Exercise Minutes":
                    int totalMinutes = Repository.WorkoutEntries.Sum(w => w.MinutesWorkout);
                    return $"Progress: {totalMinutes}/{goal.TargetExerciseMinutes ?? 0} min";

                case "Muscle Gain":
                    return $"Progress: {goal.CurrentMuscleWeight ?? 0}/{goal.TargetMuscleWeight ?? 0} kg";

                case "Lose Weight":
                    return $"Progress: {goal.CurrentWeight ?? 0}/{goal.TargetWeight ?? 0} kg";
            }

            return "Progress: 0";
        }
    }
}
//AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH!!!!!!!!!!!!!!!!!!