using Microsoft.Extensions.Logging;
using RecipeApp.Pages;
using RecipeApp.ViewModels;
using RecipeApp.Storage;

namespace RecipeApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register database service
        builder.Services.AddSingleton<RecipeDatabase>();

        // Register ViewModels
        // Transient lifetime for ViewModels since they are page-specific and can be recreated each time the page is navigated to
        builder.Services.AddTransient<RecipesViewModel>();
        builder.Services.AddTransient<AddRecipeViewModel>();

        // Register Pages
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<RecipesPage>();
        builder.Services.AddTransient<AddRecipePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}