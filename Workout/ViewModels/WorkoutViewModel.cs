using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Workout.Models;
using Workout.Services;
using Microsoft.Maui.Controls;

namespace Workout.ViewModels
{
    public class WorkoutViewModel : BindableObject
    {
        // Input fields for adding new workouts
        private string workoutType;
        public string WorkoutType { get => workoutType; set { workoutType = value; OnPropertyChanged(); } }

        private int minutesWorkout;
        public int MinutesWorkout { get => minutesWorkout; set { minutesWorkout = value; OnPropertyChanged(); } }

        private DateTime date = DateTime.Now;
        public DateTime Date { get => date; set { date = value; OnPropertyChanged(); } }

        private int caloriesBurnt;
        public int CaloriesBurnt { get => caloriesBurnt; set { caloriesBurnt = value; OnPropertyChanged(); } }

        // Collection of workouts
        public ObservableCollection<WorkoutModel> WorkoutEntries => Repository.WorkoutEntries;

        // Modal editing fields
        private bool isEditModalVisible;
        public bool IsEditModalVisible { get => isEditModalVisible; set { isEditModalVisible = value; OnPropertyChanged(); } }

        private string editWorkoutType;
        public string EditWorkoutType { get => editWorkoutType; set { editWorkoutType = value; OnPropertyChanged(); } }

        private int editMinutes;
        public int EditMinutes { get => editMinutes; set { editMinutes = value; OnPropertyChanged(); } }

        private DateTime editDate;
        public DateTime EditDate { get => editDate; set { editDate = value; OnPropertyChanged(); } }

        private int editCalories;
        public int EditCalories { get => editCalories; set { editCalories = value; OnPropertyChanged(); } }

        private WorkoutModel selectedWorkout;

        // Commands
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveEditCommand { get; }
        public ICommand CancelEditCommand { get; }

        public WorkoutViewModel()
        {
            SaveCommand = new Command(SaveEntry);
            DeleteCommand = new Command<WorkoutModel>(DeleteEntry);
            EditCommand = new Command<WorkoutModel>(OpenEditModal);
            SaveEditCommand = new Command(SaveEditEntry);
            CancelEditCommand = new Command(CloseEditModal);
        }

        private void SaveEntry()
        {
            if (!string.IsNullOrWhiteSpace(WorkoutType) && MinutesWorkout > 0)
            {
                Repository.WorkoutEntries.Add(new WorkoutModel
                {
                    WorkoutType = WorkoutType,
                    Date = Date,
                    MinutesWorkout = MinutesWorkout,
                    TimeSpan = TimeSpan.FromMinutes(MinutesWorkout),
                    CalroiesBurnt = CaloriesBurnt
                });

                // Reset fields
                WorkoutType = string.Empty;
                MinutesWorkout = 0;
                Date = DateTime.Now;
                CaloriesBurnt = 0;

                FileLogger.Log("Saved Workout Entry");
            }
        }

        private void DeleteEntry(WorkoutModel entry)
        {
            if (entry != null && WorkoutEntries.Contains(entry))
            {
                WorkoutEntries.Remove(entry);
                FileLogger.Log("Deleted Workout Entry");
            }
        }

        private void OpenEditModal(WorkoutModel entry)
        {
            if (entry == null) return;

            selectedWorkout = entry;

            // Load entry data into modal fields
            EditWorkoutType = entry.WorkoutType;
            EditMinutes = entry.MinutesWorkout;
            EditDate = entry.Date;
            EditCalories = entry.CalroiesBurnt;

            IsEditModalVisible = true;
        }

        private void SaveEditEntry()
        {
            if (selectedWorkout != null)
            {
                selectedWorkout.WorkoutType = EditWorkoutType;
                selectedWorkout.MinutesWorkout = EditMinutes;
                selectedWorkout.Date = EditDate;
                selectedWorkout.TimeSpan = TimeSpan.FromMinutes(EditMinutes);
                selectedWorkout.CalroiesBurnt = EditCalories;

                // Refresh CollectionView
                var index = WorkoutEntries.IndexOf(selectedWorkout);
                WorkoutEntries.RemoveAt(index);
                WorkoutEntries.Insert(index, selectedWorkout);
            }
            FileLogger.Log("Edited Workout Entry");
            CloseEditModal();
        }

        private void CloseEditModal()
        {
            IsEditModalVisible = false;
            selectedWorkout = null;
        }
    }
}
