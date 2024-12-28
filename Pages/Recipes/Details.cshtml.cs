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
        private readonly Reteteculinare.Data.ReteteculinareContext _context;

        public DetailsModel(Reteteculinare.Data.ReteteculinareContext context)
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
            Recipe = await _context.Recipe
           .Include(r => r.Ingredients) // Include ingredientele
           .FirstOrDefaultAsync(m => m.ID == id);


            var recipe = await _context.Recipe.FirstOrDefaultAsync(m => m.ID == id);
            if (recipe == null)
            {
                return NotFound();
            }
            else
            {
                Recipe = recipe;
            }
            return Page();
        }
    }
}
