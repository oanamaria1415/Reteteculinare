namespace Reteteculinare.Models
{
    public class Ingredient
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }
    }

}
