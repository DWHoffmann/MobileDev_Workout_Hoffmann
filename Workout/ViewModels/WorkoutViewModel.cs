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
        private string workoutType;
        public string WorkoutType
        {
            get => workoutType;
            set { workoutType = value; OnPropertyChanged(); }
        }

        private int? minutesWorkout;
        public int? MinutesWorkout
        {
            get => minutesWorkout;
            set { minutesWorkout = value; OnPropertyChanged(); }
        }

        private DateTime date = DateTime.Now;
        public DateTime Date
        {
            get => date;
            set { date = value; OnPropertyChanged(); }
        }

        private int? caloriesBurnt;
        public int? CaloriesBurnt
        {
            get => caloriesBurnt;
            set { caloriesBurnt = value; OnPropertyChanged(); }
        }

        public ObservableCollection<WorkoutModel> WorkoutEntries => Repository.WorkoutEntries;

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public WorkoutViewModel()
        {
            SaveCommand = new Command(SaveEntry);
            DeleteCommand = new Command<WorkoutModel>(DeleteEntry);
        }

        private void SaveEntry()
        {
            if (!string.IsNullOrWhiteSpace(WorkoutType) && MinutesWorkout.HasValue && MinutesWorkout.Value > 0)
            {
                Repository.WorkoutEntries.Add(new WorkoutModel
                {
                    WorkoutType = WorkoutType,
                    Date = Date,
                    MinutesWorkout = MinutesWorkout.Value,
                    TimeSpan = TimeSpan.FromMinutes(MinutesWorkout.Value),
                    CalroiesBurnt = CaloriesBurnt ?? 0
                });

                // Reset fields
                WorkoutType = string.Empty;
                MinutesWorkout = null;
                Date = DateTime.Now;
                CaloriesBurnt = null;
            }
            FileLogger.Log("Saved Workout Entry");

        }

        private void DeleteEntry(WorkoutModel entry)
        {
            if (Repository.WorkoutEntries.Contains(entry))
                Repository.WorkoutEntries.Remove(entry);
            FileLogger.Log("Deleted Workout Entry");

        }
    }
}
