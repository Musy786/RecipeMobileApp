using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RecipeApp.Core.Helpers;
using RecipeApp.Core.Models;
using RecipeApp.Storage;

namespace RecipeApp.ViewModels;

// ViewModel for editing an existing recipe
// Loads the recipe from the database and allows the user to update its fields before saving
public partial class EditRecipeViewModel : ObservableObject
{
    private readonly RecipeDatabase _database;

    // Store the recipe ID for saving later
    private int _recipeId;

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

    public EditRecipeViewModel(RecipeDatabase database)
    {
        _database = database;
    }

    // Load recipe details from the database and populate the form fields
    // Called from the page's OnAppearing with the recipe ID from the query parameter
    public async Task LoadRecipeAsync(int id)
    {
        try
        {
            _recipeId = id;
            isLoading = true;

            var recipe = await _database.GetRecipeByIdAsync(_recipeId);

            if (recipe == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    "Recipe not found.", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            // Populate the form fields with the existing recipe data
            recipeName = recipe.Name;
            description = recipe.Description;
            ingredients = recipe.Ingredients;
            steps = recipe.Steps;
            prepTime = recipe.PrepTime.ToString();
            cookTime = recipe.CookTime.ToString();
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", 
                $"Failed to load recipe: {ex.Message}", "OK");
        }
        finally
        {
            isLoading = false;
        }
    }

    // Save the updated recipe back to the database
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

            // Confirmation before saving changes
            bool confirmed = await Application.Current.MainPage.DisplayAlert("Confirm",
                "Are you sure you want to save these changes?", "Yes", "No");

            if (!confirmed)
                return;

            isLoading = true;

            // Get the existing recipe from the database so we keep the original CreatedDate
            var existingRecipe = await _database.GetRecipeByIdAsync(_recipeId);

            if (existingRecipe == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Recipe no longer exists.", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            // Update the recipe object with the new values
            existingRecipe.Name = recipeName;
            existingRecipe.Description = description;
            existingRecipe.Ingredients = ingredients;
            existingRecipe.Steps = steps;
            existingRecipe.PrepTime = int.TryParse(prepTime, out int prep) ? prep : 0;
            existingRecipe.CookTime = int.TryParse(cookTime, out int cook) ? cook : 0;

            // Save changes to the database
            await _database.UpdateRecipeAsync(existingRecipe);

            await Application.Current.MainPage.DisplayAlert("Updated!",
                "Recipe updated successfully!", "OK");

            // Navigate back to the recipes list
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                $"Failed to update recipe: {ex.Message}", "OK");
        }
        finally
        {
            isLoading = false;
        }
    }
}
