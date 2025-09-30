using Microsoft.Maui.Controls;
using Workout.Resources.Styles;

namespace Workout;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        // Set switch to reflect current theme
        ThemeSwitch.IsToggled = ThemeManager.IsLightTheme;
    }

    private async void OnGoalsClicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new Views.GoalView());

    private async void OnWorkoutClicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new Views.WorkoutView());

    private async void OnDietaryClicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new Views.DietView());

    private void OnThemeToggled(object sender, ToggledEventArgs e)
    {
        ThemeManager.ApplyTheme(e.Value);
    }
}
