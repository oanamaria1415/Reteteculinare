using System.ComponentModel.DataAnnotations;

namespace Reteteculinare.Models
{
    public class Recipe
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Recipe name is required.")]
        [StringLength(100, ErrorMessage = "Recipe name cannot be longer than 100 characters.")]
        [Display(Name = "Recipe Name")]
        public string RecipeName { get; set; }

        [Required(ErrorMessage = "Chef name is required.")]
        [StringLength(50, ErrorMessage = "Chef name cannot be longer than 50 characters.")]
        [Display(Name = "Chef Name")]
        public string ChefName { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(50, ErrorMessage = "Category cannot be longer than 50 characters.")]


       
        public string Category { get; set; } // Categorie introdusă manual

       

        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        [Required(ErrorMessage = "Instructions are required.")]
        public string Instructions { get; set; }

        public string ImagePath { get; set; } = "/images/default-recipe.jpg"; // Valoare implicită

        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    }
}
