using SQLite;

namespace RecipeApp.Core.Models
{
    public class Recipe
    {
        [PrimaryKey, AutoIncrement]
        public int Id {get; set;}         // SQLite primary key
        
        public string Name {get; set;}         // Get and set, just in case for future features
        public string Description {get; set;}
        public string Ingredients {get; set;} // comma-separated string
        public string Steps {get; set;}
        public int PrepTime {get; set;}       // in minutes
        public int CookTime {get; set;}       // in minutes
        public DateTime CreatedDate {get; set;} // Track when recipe was created

        public Recipe()  // Recipe Object
        {
            Name = string.Empty;
            Description = string.Empty;
            Ingredients = string.Empty;
            Steps = string.Empty;
            PrepTime = 0;
            CookTime = 0;
            CreatedDate = DateTime.Now;
        }
    }
}