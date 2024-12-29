using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reteteculinare.Data;
using Reteteculinare.Models;

namespace Reteteculinare.Pages.Recipes
{
    public class CreateModel : PageModel
    {
        private readonly ReteteculinareContext _context;

        public CreateModel(ReteteculinareContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Recipe Recipe { get; set; } = default!;

        [BindProperty]
        public string IngredientsText { get; set; } = string.Empty;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Adaugă rețeta principală în baza de date
            _context.Recipe.Add(Recipe);
            await _context.SaveChangesAsync(); // Salvează pentru a genera un ID pentru rețetă

            // Prelucrează ingredientele trimise din textarea
            if (!string.IsNullOrWhiteSpace(IngredientsText))
            {
                var ingredients = IngredientsText.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(line =>
                    {
                        var parts = line.Split(',', StringSplitOptions.TrimEntries);
                        if (parts.Length == 3)
                        {
                            return new Ingredient
                            {
                                Name = parts[0],
                                Quantity = double.TryParse(parts[1], out var quantity) ? quantity : 0,
                                Unit = parts[2],
                                RecipeID = Recipe.ID
                            };
                        }
                        return null;
                    })
                    .Where(i => i != null)
                    .ToList();

                // Adaugă ingredientele asociate în baza de date
                if (ingredients != null && ingredients.Count > 0)
                {
                    _context.Ingredients.AddRange(ingredients);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
