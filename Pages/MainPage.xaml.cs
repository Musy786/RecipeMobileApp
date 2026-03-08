namespace RecipeApp.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    
    private async void OnViewAllRecipesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RecipesPage));
    }

    private async void OnAddNewRecipeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddRecipePage));
    }
}