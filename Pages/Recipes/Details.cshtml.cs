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
    public class DetailsModel : PageModel
    {
        private readonly ReteteculinareContext _context;

        public DetailsModel(ReteteculinareContext context)
        {
            _context = context;
        }

        public Recipe Recipe { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include ingredientele în interogare
            Recipe = await _context.Recipe
                .Include(r => r.Ingredients) // Include lista de ingrediente asociate
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Recipe == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
