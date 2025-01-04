using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Reteteculinare.Data;
using Reteteculinare.Models;
using Microsoft.AspNetCore.Identity;

namespace Reteteculinare.Pages.Recipes
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ReteteculinareContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(ReteteculinareContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Recipe> Recipe { get; set; } = new List<Recipe>();
        public List<string> ChefList { get; set; } = new List<string>();
        public List<string> CategoryList { get; set; } = new List<string>();

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string ChefFilter { get; set; } = "All Chefs";

        [BindProperty(SupportsGet = true)]
        public string CategoryFilter { get; set; } = "All Categories";

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

            // Filtrare după bucătar
            if (!string.IsNullOrWhiteSpace(ChefFilter) && ChefFilter != "All Chefs")
            {
                query = query.Where(r => r.ChefName == ChefFilter);
            }

            // Filtrare după categorie
            if (!string.IsNullOrWhiteSpace(CategoryFilter) && CategoryFilter != "All Categories")
            {
                query = query.Where(r => r.Category == CategoryFilter);
            }

            // Bara de căutare (case-insensitive)
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                query = query.Where(r => r.RecipeName.ToLower().Contains(SearchQuery.ToLower()));
            }

            Recipe = await query.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var recipe = await _context.Recipe
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.ID == id);

            if (recipe != null)
            {
                if (recipe.Ingredients != null)
                {
                    _context.Ingredients.RemoveRange(recipe.Ingredients);
                }

                _context.Recipe.Remove(recipe);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Rețeta a fost ștearsă cu succes!";
            }
            else
            {
                TempData["Error"] = "Rețeta nu a fost găsită!";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddToFavoritesAsync(int id)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // Verifică dacă rețeta este deja în favorite
            var existingFavorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == id);

            if (existingFavorite == null)
            {
                // Adaugă rețeta la favorite
                var favorite = new Favorite
                {
                    UserId = userId,
                    RecipeId = id
                };

                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Rețeta a fost adăugată la favorite!";
            }
            else
            {
                TempData["Error"] = "Rețeta este deja în lista de favorite!";
            }

            return RedirectToPage();
        }
    }
}
