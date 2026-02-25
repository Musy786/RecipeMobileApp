namespace RecipeApp.Models
{
    public class Recipe
    {
        public string Name {get;set;}         // Get and set, just in case for future features
        public string Description {get;set;}
        public string Ingredients {get;set;} // comma-separated string
        public string Steps {get;set;}
        public int PrepTime {get;set;} // in minutes
        public int CookTime {get;set;} // in minutes

        public Recipe()  // Recipe Object
        {
            Name = string.Empty;
            Description = string.Empty;
            Ingredients = string.Empty;
            Steps = string.Empty;
            PrepTime = 0;
            CookTime = 0;
        }
    }
}