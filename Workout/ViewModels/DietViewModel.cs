using System.Collections.ObjectModel;
using System.Windows.Input;
using Workout.Models;
using Workout.Services;
using Microsoft.Maui.Controls;

namespace Workout.ViewModels
{
    public class DietViewModel : BindableObject
    {
        // Input fields for adding new meals
        private string meal;
        public string Meal { get => meal; set { meal = value; OnPropertyChanged(); } }

        private int calories;
        public int Calories { get => calories; set { calories = value; OnPropertyChanged(); } }

        private int protien;
        public int Protien { get => protien; set { protien = value; OnPropertyChanged(); } }

        private int carbs;
        public int Carbs { get => carbs; set { carbs = value; OnPropertyChanged(); } }

        private int servings;
        public int Servings { get => servings; set { servings = value; OnPropertyChanged(); } }

        private bool isEditModalVisible;
        public bool IsEditModalVisible { get => isEditModalVisible; set { isEditModalVisible = value; OnPropertyChanged(); } }

        private string editMealText;
        public string EditMealText { get => editMealText; set { editMealText = value; OnPropertyChanged(); } }

        private int editCalories;
        public int EditCalories { get => editCalories; set { editCalories = value; OnPropertyChanged(); } }

        private int editProtien;
        public int EditProtien { get => editProtien; set { editProtien = value; OnPropertyChanged(); } }

        private int editCarbs;
        public int EditCarbs { get => editCarbs; set { editCarbs = value; OnPropertyChanged(); } }

        private int editServings;
        public int EditServings { get => editServings; set { editServings = value; OnPropertyChanged(); } }

        private DietModel selectedMeal; //i could have dne this better probably

        // Shared repository collection
        public ObservableCollection<DietModel> DietEntries => Repository.DietEntries;

        // Commands
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveEditCommand { get; }
        public ICommand CancelEditCommand { get; }

        public DietViewModel()
        {
            SaveCommand = new Command(SaveMeal);
            DeleteCommand = new Command<DietModel>(DeleteMeal);
            EditCommand = new Command<DietModel>(OpenEditModal);
            SaveEditCommand = new Command(SaveEditMeal);
            CancelEditCommand = new Command(CloseEditModal);
        }

        private void SaveMeal()
        {
            if (string.IsNullOrWhiteSpace(Meal))
                return;

            DietEntries.Add(new DietModel
            {
                Meal = Meal,
                Calories = Calories,
                Protien = Protien,
                Carbs = Carbs,
                Servings = Servings
            });

            ClearForm();
        }

        private void DeleteMeal(DietModel meal)
        {
            if (meal != null && DietEntries.Contains(meal))
                DietEntries.Remove(meal);
        }

        private void OpenEditModal(DietModel meal)
        {
            if (meal == null) return;

            selectedMeal = meal;

            // Load meal data into modal fields
            EditMealText = meal.Meal;
            EditCalories = meal.Calories;
            EditProtien = meal.Protien;
            EditCarbs = meal.Carbs;
            EditServings = meal.Servings;

            IsEditModalVisible = true;
        }

        private void SaveEditMeal()
        {
            if (selectedMeal != null)
            {
                selectedMeal.Meal = EditMealText;
                selectedMeal.Calories = EditCalories;
                selectedMeal.Protien = EditProtien;
                selectedMeal.Carbs = EditCarbs;
                selectedMeal.Servings = EditServings;

                var index = DietEntries.IndexOf(selectedMeal);
                DietEntries.RemoveAt(index);
                DietEntries.Insert(index, selectedMeal);
            }
            FileLogger.Log("Edited Meal Entry");
            CloseEditModal();
        }

        private void CloseEditModal()
        {
            IsEditModalVisible = false;
            selectedMeal = null;
        }

        private void ClearForm()
        {
            Meal = string.Empty;
            Calories = 0;
            Protien = 0;
            Carbs = 0;
            Servings = 0;
        }
    }
}
