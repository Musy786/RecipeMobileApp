using RecipeApp.Models;
using RecipeApp.ViewModels;

namespace RecipeApp.Pages;

public partial class RecipesPage : ContentPage
{
    private readonly RecipesViewModel _viewModel;

    // Constructor with dependency injection of the ViewModel
    // ViewModel is responsible for managing the data and logic of the page, while the page itself is responsible for the UI
    public RecipesPage(RecipesViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    // Load recipes when page appears ensuring the list is updated if user adds or deletes recipes
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadRecipesAsync();
    }
}