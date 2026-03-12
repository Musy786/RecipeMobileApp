using RecipeApp.Core.Helpers;

namespace RecipeApp.Tests;

public class RecipeValidatorTests
{
    // Valid input shared across tests
    private const string ValidName = "Spaghetti Bolognese";
    private const string ValidDescription = "Classic Italian pasta dish";
    private const string ValidIngredients = "pasta, mince, tomato sauce, onion, garlic";
    private const string ValidSteps = "Cook pasta. Brown mince. Add sauce. Combine.";
    private const string ValidPrepTime = "15";
    private const string ValidCookTime = "30";

    [Fact]
    public void Validate_AllFieldsValid_ReturnsNoErrors()
    {
        var errors = RecipeValidator.Validate(ValidName, ValidDescription, ValidIngredients, ValidSteps, ValidPrepTime, ValidCookTime);

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_EmptyRequiredFields_ReturnsErrorForEach()
    {
        var errors = RecipeValidator.Validate("", "", "", "", "", "");

        // Should have errors for name, description, ingredients, and steps (4 required fields)
        Assert.Equal(4, errors.Count);
        Assert.Contains(errors, e => e.Contains("name", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(errors, e => e.Contains("description", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(errors, e => e.Contains("ingredients", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(errors, e => e.Contains("steps", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_NameExceedsMaxLength_ReturnsError()
    {
        string longName = new string('A', RecipeValidator.MaxNameLength + 1);

        var errors = RecipeValidator.Validate(longName, ValidDescription, ValidIngredients, ValidSteps, ValidPrepTime, ValidCookTime);

        Assert.Single(errors);
        Assert.Contains("100", errors[0]);
    }

    [Fact]
    public void Validate_NegativePrepTime_ReturnsError()
    {
        var errors = RecipeValidator.Validate(ValidName, ValidDescription, ValidIngredients, ValidSteps, "-5", ValidCookTime);

        Assert.Single(errors);
        Assert.Contains("Prep time", errors[0], StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Validate_InvalidCookTimeText_ReturnsError()
    {
        var errors = RecipeValidator.Validate(ValidName, ValidDescription, ValidIngredients, ValidSteps, ValidPrepTime, "abc");

        Assert.Single(errors);
        Assert.Contains("Cook time", errors[0], StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Validate_DecimalPrepTime_ReturnsError()
    {
        var errors = RecipeValidator.Validate(ValidName, ValidDescription, ValidIngredients, ValidSteps, "12.5", ValidCookTime);

        Assert.Single(errors);
        Assert.Contains("whole number", errors[0], StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Validate_DecimalCookTime_ReturnsError()
    {
        var errors = RecipeValidator.Validate(ValidName, ValidDescription, ValidIngredients, ValidSteps, ValidPrepTime, "30,5");

        Assert.Single(errors);
        Assert.Contains("whole number", errors[0], StringComparison.OrdinalIgnoreCase);
    }
}