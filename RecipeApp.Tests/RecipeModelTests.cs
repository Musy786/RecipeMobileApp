using RecipeApp.Core.Models;

namespace RecipeApp.Tests;

public class RecipeModelTests
{
    [Fact]
    public void Recipe_DefaultConstructor_SetsDefaultValues()
    {
        var recipe = new Recipe();

        Assert.Equal(string.Empty, recipe.Name);
        Assert.Equal(string.Empty, recipe.Description);
        Assert.Equal(string.Empty, recipe.Ingredients);
        Assert.Equal(string.Empty, recipe.Steps);
        Assert.Equal(0, recipe.PrepTime);
        Assert.Equal(0, recipe.CookTime);
        Assert.Equal(0, recipe.Id);
        Assert.True(recipe.CreatedDate <= DateTime.Now);
    }

    [Fact]
    public void Recipe_SetProperties_RetainsValues()
    {
        var recipe = new Recipe
        {
            Name = "Test Recipe",
            Description = "A test",
            Ingredients = "flour, sugar",
            Steps = "Mix and bake",
            PrepTime = 10,
            CookTime = 25
        };

        Assert.Equal("Test Recipe", recipe.Name);
        Assert.Equal("A test", recipe.Description);
        Assert.Equal("flour, sugar", recipe.Ingredients);
        Assert.Equal("Mix and bake", recipe.Steps);
        Assert.Equal(10, recipe.PrepTime);
        Assert.Equal(25, recipe.CookTime);
    }
}