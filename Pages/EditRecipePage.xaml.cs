using RecipeApp.ViewModels;
using RecipeApp.Storage;

namespace RecipeApp.Pages;

[QueryProperty(nameof(RecipeId), "RecipeId")]
public partial class EditRecipePage : ContentPage
{
    private readonly EditRecipeViewModel _viewModel;
    private readonly RecipeDatabase _database;

    public string RecipeId { get; set; } = string.Empty;

    public EditRecipePage(EditRecipeViewModel viewModel, RecipeDatabase database)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _database = database;
        BindingContext = _viewModel;
    }

    // OnNavigatedTo fires AFTER Shell has set the QueryProperty values
    // This guarantees RecipeId is available when we try to load the recipe
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (!int.TryParse(RecipeId, out int id))
            return;

        // Load the recipe from the database
        var recipe = await _database.GetRecipeByIdAsync(id);

        if (recipe == null)
            return;

        // Set the ViewModel properties for save logic
        await _viewModel.LoadRecipeAsync(id);

        // Directly set the Entry fields to guarantee they display the values
        recipeNameEntry.Text = recipe.Name;
        descriptionEntry.Text = recipe.Description;
        ingredientsEntry.Text = recipe.Ingredients;
        stepsEntry.Text = recipe.Steps;
        prepTimeEntry.Text = recipe.PrepTime.ToString();
        cookTimeEntry.Text = recipe.CookTime.ToString();
    }
}