using RecipeApp.Models;
using RecipeApp.Storage;

namespace RecipeApp.Pages;

public partial class AddRecipePage : ContentPage
{
    public AddRecipePage()
    {
        InitializeComponent();
    }
    
    private async void OnSaveRecipeClicked(object sender, EventArgs e)
    {
        var recipe = new Recipe
        {
            Name = recipeName.Text,
            Description = description.Text,
            Ingredients = ingredients.Text,
            Steps = steps.Text,
            PrepTime = int.TryParse(prepTime.Text, out int prep) ? prep : 0,  // Just in case string is put in - Defaults to 0
            CookTime = int.TryParse(cookTime.Text, out int cook) ? cook : 0
        };
        
        if (string.IsNullOrWhiteSpace(recipeName.Text) ||        // Checking for any empty spaces for each entry
            string.IsNullOrWhiteSpace(description.Text) ||       // Not one of them can be empty otherwise an alert will display
            string.IsNullOrWhiteSpace(ingredients.Text) ||
            string.IsNullOrWhiteSpace(steps.Text))
        {
            await DisplayAlert("Missing Information!", "Please fill in all fields.", "OK");
            return;
        }

        // Confirmation before saving just in case they are not finished
        bool confirmation = await DisplayAlert("Confirm", "Are you sure you want to save this recipe?", "Yes", "No");
        if (confirmation == false)
            return;
        
        RecipeStorage.AddRecipe(recipe);
        await DisplayAlert("Saved!", "Recipe added successfully!", "OK");
        
        // After each recipe saved, clears all the entries for customer to add in another recipe without having to clear it themselves
        recipeName.Text = "";
        description.Text = "";
        ingredients.Text = "";
        steps.Text = "";
        prepTime.Text = "";
        cookTime.Text = "";
    }
}