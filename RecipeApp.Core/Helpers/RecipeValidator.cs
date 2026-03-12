namespace RecipeApp.Core.Helpers;

// Centralised validation logic for recipe forms
// Used by both AddRecipeViewModel and EditRecipeViewModel to ensure consistent rules
public static class RecipeValidator
{
    public const int MaxNameLength = 100;
    public const int MaxDescriptionLength = 200;
    public const int MaxIngredientsLength = 500;
    public const int MaxStepsLength = 1000;

    // Validates all recipe fields and returns a list of error messages
    // Returns an empty list if all fields are valid
    public static List<string> Validate(string name, string description, string ingredients, string steps, string prepTime, string cookTime)
    {
        var errors = new List<string>();

        // Required fields
        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Recipe name is required.");
        else if (name.Length > MaxNameLength)
            errors.Add($"Recipe name must be {MaxNameLength} characters or less.");

        if (string.IsNullOrWhiteSpace(description))
            errors.Add("Description is required.");
        else if (description.Length > MaxDescriptionLength)
            errors.Add($"Description must be {MaxDescriptionLength} characters or less.");

        if (string.IsNullOrWhiteSpace(ingredients))
            errors.Add("Ingredients are required.");
        else if (ingredients.Length > MaxIngredientsLength)
            errors.Add($"Ingredients must be {MaxIngredientsLength} characters or less.");

        if (string.IsNullOrWhiteSpace(steps))
            errors.Add("Steps are required.");
        else if (steps.Length > MaxStepsLength)
            errors.Add($"Steps must be {MaxStepsLength} characters or less.");

        // Numeric fields must be positive whole (integer) numbers
        if (!string.IsNullOrWhiteSpace(prepTime))
        {
            if (prepTime.Contains('.') || prepTime.Contains(','))
                errors.Add("Prep time must be a whole number (no decimals).");
            else if (!int.TryParse(prepTime, out int prep) || prep < 0)
                errors.Add("Prep time must be a valid positive number.");
        }

        if (!string.IsNullOrWhiteSpace(cookTime))
        {
            if (cookTime.Contains('.') || cookTime.Contains(','))
                errors.Add("Cook time must be a whole number (no decimals).");
            else if (!int.TryParse(cookTime, out int cook) || cook < 0)
                errors.Add("Cook time must be a valid positive number.");
        }

        return errors;
    }
}