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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Găsim rețeta inclusiv ingredientele
            var recipe = await _context.Recipe
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (recipe != null)
            {
                // Ștergem întâi ingredientele asociate
                _context.Ingredients.RemoveRange(recipe.Ingredients);

                // Ștergem rețeta
                _context.Recipe.Remove(recipe);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
