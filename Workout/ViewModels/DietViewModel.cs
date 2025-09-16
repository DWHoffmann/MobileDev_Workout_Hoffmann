using System.Collections.ObjectModel;
using System.Windows.Input;
using Workout.Models;
using Workout.Services;
using Microsoft.Maui.Controls;

namespace Workout.ViewModels
{
    public class DietViewModel : BindableObject
    {
        // Input fields
        private string meal;
        public string Meal
        {
            get => meal;
            set { meal = value; OnPropertyChanged(); }
        }

        private int? calories;
        public int? Calories
        {
            get => calories;
            set { calories = value; OnPropertyChanged(); }
        }

        private int? protien;
        public int? Protien
        {
            get => protien;
            set { protien = value; OnPropertyChanged(); }
        }

        private int? carbs;
        public int? Carbs
        {
            get => carbs;
            set { carbs = value; OnPropertyChanged(); }
        }

        private int? servings;
        public int? Servings
        {
            get => servings;
            set { servings = value; OnPropertyChanged(); }
        }


        // The shared repository collection
        public ObservableCollection<DietModel> DietEntries => Repository.DietEntries;

        // Commands
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public DietViewModel()
        {
            SaveCommand = new Command(SaveEntry);
            DeleteCommand = new Command<DietModel>(DeleteEntry);
        }

        private void SaveEntry()
        {
            if (!string.IsNullOrWhiteSpace(Meal))
            {
                Repository.DietEntries.Add(new DietModel
                {
                    Meal = Meal,
                    Calories = Calories ?? 0,   // use 0 if null
                    Protien = Protien ?? 0,
                    Carbs = Carbs ?? 0,
                    Servings = Servings ?? 0
                });


                // Reset input fields
                Meal = string.Empty;
                Calories = 0;
                Protien = 0;
                Carbs = 0;
                Servings = 0;
            }

            FileLogger.Log("Saved Diet Entry");
        }

        private void DeleteEntry(DietModel entry)
        {
            if (Repository.DietEntries.Contains(entry))
                Repository.DietEntries.Remove(entry);

            FileLogger.Log("Deleted Diet Entry");

        }
    }
}
