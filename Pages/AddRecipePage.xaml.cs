using RecipeApp.ViewModels;

namespace RecipeApp.Pages;

public partial class AddRecipePage : ContentPage
{
    private readonly AddRecipeViewModel _viewModel;

    public AddRecipePage(AddRecipeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    
    // Clear the Entry fields when page appears
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ClearFormFields();
        
        // Subscribe to the save event so the page can clear the entry fields directly
        _viewModel.OnRecipeSaved += ClearFormFields;
    }

    // Clear all entry fields
    private void ClearFormFields()
    {
        recipeNameEntry.Text = string.Empty;
        descriptionEntry.Text = string.Empty;
        ingredientsEntry.Text = string.Empty;
        stepsEntry.Text = string.Empty;
        prepTimeEntry.Text = string.Empty;
        cookTimeEntry.Text = string.Empty;
    }
}