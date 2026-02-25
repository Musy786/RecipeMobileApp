using RecipeApp.Models;

namespace RecipeApp.Storage
{
    public static class RecipeStorage
    {
        private static List<Recipe> recipes = new();

        public static List<Recipe> GetAllRecipes()
        {
            return recipes;
        }

        public static void AddRecipe(Recipe recipe)
        {
            recipes.Add(recipe);
        }

        public static void ClearRecipes()
        {
            recipes.Clear();
        }
    }
}