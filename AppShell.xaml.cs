using RecipeApp.Pages;

namespace RecipeApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute(nameof(RecipesPage), typeof(RecipesPage));
        Routing.RegisterRoute(nameof(AddRecipePage), typeof(AddRecipePage));
        Routing.RegisterRoute(nameof(EditRecipePage), typeof(EditRecipePage));
    }
}