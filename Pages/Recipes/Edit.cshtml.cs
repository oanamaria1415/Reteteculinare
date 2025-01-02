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

namespace Reteteculinare.Pages.Recipes
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ReteteculinareContext _context;

        public EditModel(ReteteculinareContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Recipe Recipe { get; set; } = default!;

        [BindProperty]
        public string IngredientsText { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipe = await _context.Recipe
                .Include(r => r.Ingredients) // Include lista de ingrediente
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Recipe == null)
            {
                return NotFound();
            }

            // Inițializează IngredientsText cu ingredientele existente
            IngredientsText = string.Join("\n", Recipe.Ingredients.Select(i => $"{i.Name}, {i.Quantity}, {i.Unit}"));

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Preluăm rețeta existentă din baza de date
            var recipeToUpdate = await _context.Recipe
                .Include(r => r.Ingredients) // Include lista de ingrediente existente
                .FirstOrDefaultAsync(r => r.ID == Recipe.ID);

            if (recipeToUpdate == null)
            {
                return NotFound();
            }

            // Actualizăm câmpurile principale
            recipeToUpdate.RecipeName = Recipe.RecipeName;
            recipeToUpdate.ChefName = Recipe.ChefName;
            recipeToUpdate.Category = Recipe.Category;
            recipeToUpdate.Instructions = Recipe.Instructions;

            // Ștergem ingredientele existente
            _context.Ingredients.RemoveRange(recipeToUpdate.Ingredients);

            // Prelucrăm ingredientele din IngredientsText
            if (!string.IsNullOrWhiteSpace(IngredientsText))
            {
                var newIngredients = IngredientsText.Split('\n', StringSplitOptions.RemoveEmptyEntries)
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
                                RecipeID = recipeToUpdate.ID
                            };
                        }
                        return null;
                    })
                    .Where(i => i != null)
                    .ToList();

                if (newIngredients.Any())
                {
                    _context.Ingredients.AddRange(newIngredients);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(Recipe.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipe.Any(e => e.ID == id);
        }
    }
}
