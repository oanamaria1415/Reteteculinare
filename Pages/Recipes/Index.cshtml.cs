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
    public class IndexModel : PageModel
    {
        private readonly Reteteculinare.Data.ReteteculinareContext _context;

        public IndexModel(Reteteculinare.Data.ReteteculinareContext context)
        {
            _context = context;
        }

        public IList<Recipe> Recipe { get; set; } = new List<Recipe>();
        public List<string> ChefList { get; set; } = new List<string>();
        public List<string> CategoryList { get; set; } = new List<string>();

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string ChefFilter { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string CategoryFilter { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            // Populează listele pentru filtre
            ChefList = await _context.Recipe
                .Select(r => r.ChefName)
                .Distinct()
                .ToListAsync();

            CategoryList = await _context.Recipe
                .Select(r => r.Category)
                .Distinct()
                .ToListAsync();

            // Filtrare și căutare
            var query = _context.Recipe.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                query = query.Where(r => r.RecipeName.Contains(SearchQuery));
            }

            if (!string.IsNullOrWhiteSpace(ChefFilter))
            {
                query = query.Where(r => r.ChefName == ChefFilter);
            }

            if (!string.IsNullOrWhiteSpace(CategoryFilter))
            {
                query = query.Where(r => r.Category == CategoryFilter);
            }

            Recipe = await query.ToListAsync();
        }
    }
}
