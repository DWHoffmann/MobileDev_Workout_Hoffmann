using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Workout.Models;
using Workout.Services;

namespace Workout.ViewModels
{
    public class GoalViewModel : BindableObject
    {
        // Input fields
        private string selectedGoalType;
        public string SelectedGoalType
        {
            get => selectedGoalType;
            set { selectedGoalType = value; OnPropertyChanged(); UpdateVisibleFields(); }
        }

        public ObservableCollection<string> GoalTypes { get; } = new()
        {
            "Calories Lost",
            "Exercise Minutes",
            //"Muscle Gain",
            //"Lose Weight"
            //if have time and figure out how i want to use these add these, this is a self reminder
        };

        public int? TargetCalories { get; set; }
        public int? TargetExerciseMinutes { get; set; }
        public int? TargetMuscleWeight { get; set; }
        public string TargetMuscleAppearance { get; set; }
        public int? TargetWeight { get; set; }
        public DateTime? TargetDate { get; set; }
        public string Notes { get; set; }

        public bool ShowCaloriesInput { get; set; }
        public bool ShowExerciseInput { get; set; }
        public bool ShowMuscleInput { get; set; }
        public bool ShowWeightInput { get; set; }
        public bool ShowTargetDate { get; set; }

        public ObservableCollection<GoalsModel> Goals { get; } = Repository.Goals;

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public GoalViewModel()
        {
            SaveCommand = new Command(SaveGoal);
            DeleteCommand = new Command<GoalsModel>(DeleteGoal);

            Repository.WorkoutEntries.CollectionChanged += (_, __) => RefreshGoalProgress();
            Repository.DietEntries.CollectionChanged += (_, __) => RefreshGoalProgress();
        }

        private void UpdateVisibleFields()
        {
            ShowCaloriesInput = ShowExerciseInput = ShowMuscleInput = ShowWeightInput = false;
            ShowTargetDate = !string.IsNullOrWhiteSpace(SelectedGoalType);

            switch (SelectedGoalType)
            {
                case "Calories Lost": ShowCaloriesInput = true; break;
                case "Exercise Minutes": ShowExerciseInput = true; break;
            }

            OnPropertyChanged(nameof(ShowCaloriesInput));
            OnPropertyChanged(nameof(ShowExerciseInput));
            OnPropertyChanged(nameof(ShowMuscleInput));
            OnPropertyChanged(nameof(ShowWeightInput));
            OnPropertyChanged(nameof(ShowTargetDate));
        }

        private void SaveGoal()
        {
            if (string.IsNullOrWhiteSpace(SelectedGoalType))
                return;

            var newGoal = new GoalsModel
            {
                GoalType = SelectedGoalType,
                TargetCalories = TargetCalories,
                TargetExerciseMinutes = TargetExerciseMinutes,
                TargetMuscleWeight = TargetMuscleWeight,
                TargetMuscleAppearance = TargetMuscleAppearance,
                TargetWeight = TargetWeight,
                TargetDate = TargetDate,
                Notes = Notes
            };

            Goals.Add(newGoal);

            // Reset input fields
            SelectedGoalType = null;
            TargetCalories = TargetExerciseMinutes = TargetMuscleWeight = TargetWeight = null;
            TargetMuscleAppearance = Notes = string.Empty;
            TargetDate = null;

            OnPropertyChanged(nameof(TargetCalories));
            OnPropertyChanged(nameof(TargetExerciseMinutes));
            OnPropertyChanged(nameof(TargetMuscleWeight));
            OnPropertyChanged(nameof(TargetWeight));
            OnPropertyChanged(nameof(TargetMuscleAppearance));
            OnPropertyChanged(nameof(Notes));
            OnPropertyChanged(nameof(SelectedGoalType));
            OnPropertyChanged(nameof(TargetDate));

            FileLogger.Log("Saved Goal Entry");

        }

        private void DeleteGoal(GoalsModel goal)
        {
            if (Goals.Contains(goal))
                Goals.Remove(goal);

            FileLogger.Log("Deleted Goal Entry");

        }

        private void RefreshGoalProgress()
        {
            foreach (var goal in Goals)
            {
                goal.RefreshProgress();

            }


        }
    }
}
