using System;
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
            "Exercise Minutes"
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

        // Edit modal fields
        private bool isEditModalVisible;
        public bool IsEditModalVisible
        {
            get => isEditModalVisible;
            set { isEditModalVisible = value; OnPropertyChanged(); }
        }

        private GoalsModel goalBeingEdited;

        public string EditGoalType { get; set; }
        public int? EditTargetCalories { get; set; }
        public int? EditTargetExerciseMinutes { get; set; }
        public DateTime? EditTargetDate { get; set; }
        public string EditNotes { get; set; }

        public bool EditShowCaloriesInput { get; set; }
        public bool EditShowExerciseInput { get; set; }

        public ICommand EditCommand { get; }
        public ICommand SaveEditCommand { get; }
        public ICommand CancelEditCommand { get; }

        public GoalViewModel()
        {
            SaveCommand = new Command(SaveGoal);
            DeleteCommand = new Command<GoalsModel>(DeleteGoal);

            EditCommand = new Command<GoalsModel>(OpenEditModal);
            SaveEditCommand = new Command(SaveEdit);
            CancelEditCommand = new Command(CancelEdit);

            Repository.WorkoutEntries.CollectionChanged += (_, __) => RefreshGoalProgress();
            Repository.DietEntries.CollectionChanged += (_, __) => RefreshGoalProgress();

            RefreshGoalProgress();
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
            newGoal.RefreshProgress();

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
                goal.RefreshProgress();
        }

        // ----------- Edit Modal Logic -----------

        private void OpenEditModal(GoalsModel goal)
        {
            if (goal == null) return;
            goalBeingEdited = goal;

            EditGoalType = goal.GoalType;
            EditTargetCalories = goal.TargetCalories;
            EditTargetExerciseMinutes = goal.TargetExerciseMinutes;
            EditTargetDate = goal.TargetDate;
            EditNotes = goal.Notes;

            UpdateEditVisibleFields();

            OnPropertyChanged(nameof(EditGoalType));
            OnPropertyChanged(nameof(EditTargetCalories));
            OnPropertyChanged(nameof(EditTargetExerciseMinutes));
            OnPropertyChanged(nameof(EditTargetDate));
            OnPropertyChanged(nameof(EditNotes));

            IsEditModalVisible = true;
        }

        private void UpdateEditVisibleFields()
        {
            EditShowCaloriesInput = EditShowExerciseInput = false;
            switch (EditGoalType)
            {
                case "Calories Lost": EditShowCaloriesInput = true; break;
                case "Exercise Minutes": EditShowExerciseInput = true; break;
            }

            OnPropertyChanged(nameof(EditShowCaloriesInput));
            OnPropertyChanged(nameof(EditShowExerciseInput));
        }

        private void SaveEdit()
        {
            if (goalBeingEdited == null) return;

            goalBeingEdited.GoalType = EditGoalType;
            goalBeingEdited.TargetCalories = EditShowCaloriesInput ? EditTargetCalories : null;
            goalBeingEdited.TargetExerciseMinutes = EditShowExerciseInput ? EditTargetExerciseMinutes : null;
            goalBeingEdited.TargetDate = EditTargetDate;
            goalBeingEdited.Notes = EditNotes;

            goalBeingEdited.RefreshProgress();
            OnPropertyChanged(nameof(Goals));

            IsEditModalVisible = false;
            goalBeingEdited = null;
        }

        private void CancelEdit()
        {
            IsEditModalVisible = false;
            goalBeingEdited = null;
        }
    }
}
