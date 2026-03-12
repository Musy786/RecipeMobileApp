using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RecipeApp.Core.Helpers;
using RecipeApp.Core.Models;
using RecipeApp.Storage;

namespace RecipeApp.ViewModels;

// ViewModel for managing the state and logic of adding a new recipe
// So user input, validation, and saving the recipe to the database
// Uses ObservableProperties for data binding and RelayCommand for the save action
public partial class AddRecipeViewModel : ObservableObject
{
    private readonly RecipeDatabase _database;

    [ObservableProperty]
    private string recipeName = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string ingredients = string.Empty;

    [ObservableProperty]
    private string steps = string.Empty;

    [ObservableProperty]
    private string prepTime = string.Empty;

    [ObservableProperty]
    private string cookTime = string.Empty;

    [ObservableProperty]
    private bool isLoading = false;

    // Event to notify the page when a recipe has been saved successfully
    public event Action OnRecipeSaved;

    public AddRecipeViewModel(RecipeDatabase database)
    {
        _database = database;
    }

    // Save recipe to database
    [RelayCommand]
    public async Task SaveRecipeAsync()
    {
        try
        {
            // Validation using centralised RecipeValidator
            var errors = RecipeValidator.Validate(recipeName, description, ingredients, steps, prepTime, cookTime);

            if (errors.Count > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error",
                    string.Join("\n", errors), "OK");
                return;
            }

            // Confirmation before saving
            bool confirmed = await Application.Current.MainPage.DisplayAlert("Confirm",
                "Are you sure you want to save this recipe?", "Yes", "No");

            if (!confirmed)
                return;

            isLoading = true;

            // Create recipe object
            var recipe = new Recipe
            {
                Name = recipeName,
                Description = description,
                Ingredients = ingredients,
                Steps = steps,
                PrepTime = int.TryParse(prepTime, out int prep) ? prep : 0,  // Defaults to 0 if invalid
                CookTime = int.TryParse(cookTime, out int cook) ? cook : 0,
                CreatedDate = DateTime.Now
            };

            // Save to database
            await _database.AddRecipeAsync(recipe);

            await Application.Current.MainPage.DisplayAlert("Saved!",
                "Recipe added successfully!", "OK");

            // Notify the page that the save was successful so the UI can clear
            OnRecipeSaved?.Invoke();
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                $"Failed to save recipe: {ex.Message}", "OK");
        }
        finally
        {
            isLoading = false;
        }
    }
}
