namespace Workout.Models
{
    public class DietModel
    {
        public string Meal { get; set; }
        public int Calories { get; set; }
        public int Protien { get; set; }
        public int Carbs { get; set; }
        public int Servings { get; set; }
        public int TotalCalories => Calories * Servings;
    }
}
