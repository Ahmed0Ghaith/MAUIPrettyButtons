using Microsoft.Extensions.DependencyInjection;

namespace MauiPrettyButtons;

/// <summary>
/// Extension methods to register MauiPrettyButtons with the MAUI app builder.
/// </summary>
public static class MauiAppBuilderExtensions
{
    /// <summary>
    /// Adds MauiPrettyButtons controls to the MAUI application.
    /// Call this in your MauiProgram.cs:
    /// <code>
    /// builder.UseMauiPrettyButtons();
    /// </code>
    /// </summary>
    public static MauiAppBuilder UseMauiPrettyButtons(this MauiAppBuilder builder)
    {
        // Register font resources if bundled
        builder.ConfigureFonts(fonts =>
        {
            // Fonts can be added here if needed in future versions
        });

        return builder;
    }
}
