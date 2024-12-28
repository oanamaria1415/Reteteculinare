using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Reteteculinare.Data;
using Reteteculinare.Models;

namespace Reteteculinare.Pages.Recipes
{
    public class CreateModel : PageModel
    {
        private readonly Reteteculinare.Data.ReteteculinareContext _context;

        public CreateModel(Reteteculinare.Data.ReteteculinareContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Recipe Recipe { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Prelucrează ingredientele trimise din formular
            if (Recipe.Ingredients == null || Recipe.Ingredients.Count == 0)
            {
                Recipe.Ingredients = new List<Ingredient>();
            }

            foreach (var ingredient in Recipe.Ingredients)
            {
                _context.Ingredients.Add(ingredient); // Adaugă ingredientele în baza de date
            }

            _context.Recipe.Add(Recipe);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
