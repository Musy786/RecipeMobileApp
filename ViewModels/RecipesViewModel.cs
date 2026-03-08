using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RecipeApp.Models;
using RecipeApp.Storage;

namespace RecipeApp.ViewModels;

// ViewModel for managing recipes list and interactions
// So loading, deleting, and clearing recipes from the database
// Uses ObservableCollection for dynamic UI updates and RelayCommands for user actions
public partial class RecipesViewModel : ObservableObject
{
    private readonly RecipeDatabase _database;

    [ObservableProperty]
    private ObservableCollection<Recipe> recipes = new();

    [ObservableProperty]
    private bool isLoading = false;

    public RecipesViewModel(RecipeDatabase database)
    {
        _database = database;
    }

    // Load all recipes from database when page is initialized
    [RelayCommand]
    public async Task LoadRecipesAsync()
    {
        try
        {
            isLoading = true;
            
            var recipeList = await _database.GetAllRecipesAsync();
            
            recipes.Clear();
            foreach (var recipe in recipeList)
            {
                recipes.Add(recipe);
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load recipes: {ex.Message}", "OK");
        }
        finally
        {
            isLoading = false;
        }
    }

    // Delete a specific recipe
    [RelayCommand]
    public async Task DeleteRecipeAsync(Recipe recipe)
    {
        if (recipe == null)
            return;

        try
        {
            bool confirmed = await Application.Current.MainPage.DisplayAlert("Confirm Delete", 
            $"Are you sure you want to delete '{recipe.Name}'?", "Yes", "No");

            if (!confirmed)
                return;

            await _database.DeleteRecipeAsync(recipe.Id);
            recipes.Remove(recipe);

            await Application.Current.MainPage.DisplayAlert("Deleted", "Recipe removed successfully!", "OK");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to delete recipe: {ex.Message}", "OK");
        }
    }

    // Clear all recipes
    [RelayCommand]
    public async Task ClearAllRecipesAsync()
    {
        try
        {
            bool confirmed = await Application.Current.MainPage.DisplayAlert("Confirm", 
            "Are you sure you want to delete all recipes?", "Yes", "No");

            if (!confirmed)
                return;

            await _database.DeleteAllRecipesAsync();
            recipes.Clear();

            await Application.Current.MainPage.DisplayAlert("Cleared", "All recipes have been removed!", "OK");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to clear recipes: {ex.Message}", "OK");
        }
    }
}
