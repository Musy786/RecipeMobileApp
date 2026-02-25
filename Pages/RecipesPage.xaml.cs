using RecipeApp.Storage;

namespace RecipeApp.Pages;

public partial class RecipesPage : ContentPage
{
    public RecipesPage()
    {
        InitializeComponent();

        var recipes = RecipeStorage.GetAllRecipes();        // As soon as the page is initialised, display the list of recipes
        recipesList.ItemsSource = recipes;
        
        recipesList.ItemsSource = recipes;
        emptyLabel.IsVisible =  !recipes.Any();
    }
    private async void OnClearRecipesClicked(object sender, EventArgs e)    // Clearing all recipes from page
    {
        // Confirmation before clearing, just in case it was an accident
        bool confirmation = await DisplayAlert("Confirm", "Are you sure you want to delete all recipes?", "Yes", "No");
        if (confirmation == false)
            return;
        
        RecipeStorage.ClearRecipes();
        recipesList.ItemsSource = null;
        recipesList.ItemsSource = RecipeStorage.GetAllRecipes();      // Once list is null, the page must show that it's empty so it is called again
        emptyLabel.IsVisible = true;       // So now the empty message will show
        await DisplayAlert("Cleared", "All recipes have been removed!", "OK");
    }
}