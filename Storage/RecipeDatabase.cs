using SQLite;
using RecipeApp.Models;

namespace RecipeApp.Storage;

public class RecipeDatabase
{
    private SQLiteAsyncConnection _database;
    private const string DatabaseFilename = "recipes.db3";

    // Get the path to the database file in the app's local storage
    private string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

    public RecipeDatabase()
    {
    }

    // Initialize the database connection and create tables if needed
    public async Task InitializeAsync()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(DatabasePath,
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

        await _database.CreateTableAsync<Recipe>();
    }

    // Get all recipes from the database
    public async Task<List<Recipe>> GetAllRecipesAsync()
    {
        await InitializeAsync();
        return await _database.Table<Recipe>().OrderByDescending(r => r.CreatedDate).ToListAsync();
    }

    // Get a specific recipe by ID
    public async Task<Recipe> GetRecipeByIdAsync(int recipeId)
    {
        await InitializeAsync();
        return await _database.Table<Recipe>()
            .FirstOrDefaultAsync(r => r.Id == recipeId);
    }

    // Add a new recipe
    public async Task<int> AddRecipeAsync(Recipe recipe)
    {
        await InitializeAsync();
        return await _database.InsertAsync(recipe);
    }

    // Update an existing recipe
    public async Task<int> UpdateRecipeAsync(Recipe recipe)
    {
        await InitializeAsync();
        return await _database.UpdateAsync(recipe);
    }

    // Delete a recipe by ID
    public async Task<int> DeleteRecipeAsync(int recipeId)
    {
        await InitializeAsync();
        return await _database.DeleteAsync<Recipe>(recipeId);
    }

    // Delete all recipes (for Clear All functionality)
    public async Task<int> DeleteAllRecipesAsync()
    {
        await InitializeAsync();
        return await _database.DeleteAllAsync<Recipe>();
    }

    // Get count of recipes
    public async Task<int> GetRecipeCountAsync()
    {
        await InitializeAsync();
        return await _database.Table<Recipe>().CountAsync();
    }
}
