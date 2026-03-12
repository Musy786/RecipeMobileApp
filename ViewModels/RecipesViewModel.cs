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

    // Stores the full unfiltered list from the database for search/filtering
    private List<Recipe> _allRecipes = new();

    [ObservableProperty]
    private ObservableCollection<Recipe> recipes = new();

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private string searchText = string.Empty;

    public RecipesViewModel(RecipeDatabase database)
    {
        _database = database;
    }

    // Filter recipes whenever the search text changes using LINQ
    partial void OnSearchTextChanged(string value)
    {
        FilterRecipes(value);
    }

    // Filter the recipes list based on the search text
    private void FilterRecipes(string searchTerm)
    {
        recipes.Clear();

        var filtered = string.IsNullOrWhiteSpace(searchTerm)
            ? _allRecipes
            : _allRecipes.Where(r => r.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     r.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     r.Ingredients.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                         .ToList();

        foreach (var recipe in filtered)
        {
            recipes.Add(recipe);
        }
    }

    // Load all recipes from database when page is initialized
    [RelayCommand]
    public async Task LoadRecipesAsync()
    {
        try
        {
            isLoading = true;
            
            var recipeList = await _database.GetAllRecipesAsync();
            
            // Store the full list for search filtering
            _allRecipes = recipeList;
            
            // Apply current search filter (or show all if no search text)
            FilterRecipes(searchText);
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

    // Navigate to edit page for a specific recipe (used by swipe action)
    [RelayCommand]
    public async Task EditRecipeAsync(Recipe recipe)
    {
        if (recipe == null)
            return;

        await Shell.Current.GoToAsync($"{nameof(Pages.EditRecipePage)}?RecipeId={recipe.Id}");
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
            _allRecipes.Remove(recipe);
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
            _allRecipes.Clear();
            recipes.Clear();
            searchText = string.Empty;

            await Application.Current.MainPage.DisplayAlert("Cleared", "All recipes have been removed!", "OK");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to clear recipes: {ex.Message}", "OK");
        }
    }
}
