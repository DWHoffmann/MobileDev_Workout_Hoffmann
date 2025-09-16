using Microsoft.Maui.Controls;

namespace Workout
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnGoalsClicked(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new Views.GoalView());
        }

        private async void OnWorkoutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.WorkoutView());
        }

        private async void OnDietaryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.DietView());
        }
    }
}
