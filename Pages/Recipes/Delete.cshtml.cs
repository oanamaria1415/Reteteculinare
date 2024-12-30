using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Reteteculinare.Data;
using Reteteculinare.Models;

namespace Reteteculinare.Pages.Recipes
{
    public class DeleteModel : PageModel
    {
        private readonly ReteteculinareContext _context;

        public DeleteModel(ReteteculinareContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Recipe Recipe { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include ingredientele în interogare
            Recipe = await _context.Recipe
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Recipe == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .Include(r => r.Ingredients) // Dacă există relații
                .FirstOrDefaultAsync(r => r.ID == id); // Găsește rețeta după ID

            if (recipe == null)
            {
                return NotFound();
            }

            // Șterge doar rețeta găsită
            _context.Recipe.Remove(recipe);

            // Șterge ingredientele asociate (opțional)
            if (recipe.Ingredients != null)
            {
                _context.Ingredients.RemoveRange(recipe.Ingredients);
            }

            await _context.SaveChangesAsync(); // Aplică modificările
            TempData["Message"] = "Rețeta a fost ștearsă cu succes!";
            return RedirectToPage("./Index");
        }

    }
}
