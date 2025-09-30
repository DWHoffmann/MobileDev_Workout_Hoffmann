using Microsoft.Maui.Controls;
using System.Linq;

namespace Workout.Resources.Styles
{
    public static class ThemeManager
    {
        public static bool IsLightTheme { get; private set; } = false;

        public static void ApplyTheme(bool light)
        {
            var merged = Application.Current.Resources.MergedDictionaries;

            // Remove existing theme dictionary
            var existingTheme = merged.FirstOrDefault(d => d is DarkTheme || d is LightTheme);
            if (existingTheme != null)
                merged.Remove(existingTheme);

            // Add the selected theme
            merged.Add(light ? new LightTheme() : new DarkTheme());

            IsLightTheme = light;
        }
    }
}
