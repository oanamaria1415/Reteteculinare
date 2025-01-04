using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Reteteculinare.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // ID-ul utilizatorului

        [Required]
        public int RecipeId { get; set; } // ID-ul rețetei

        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; } // Relația cu rețeta
    }
}
