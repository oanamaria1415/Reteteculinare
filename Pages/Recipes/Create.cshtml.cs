using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reteteculinare.Data;
using Reteteculinare.Models;

namespace Reteteculinare.Pages.Recipes
{
    [Authorize(Roles = "Admin")]
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

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Debug pentru a vedea ce câmpuri lipsesc
                foreach (var state in ModelState)
                {
                    Console.WriteLine($"{state.Key}: {state.Value.Errors.FirstOrDefault()?.ErrorMessage}");
                }
                return Page();
            }

            try
            {
                // Procesare fișier imagine
                if (Image != null && Image.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Crează directorul dacă nu există
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(fileStream);
                    }

                    Recipe.ImagePath = "/images/" + uniqueFileName; // Setează calea imaginii
                }
                else
                {
                    Recipe.ImagePath = "/images/default-recipe.jpg"; // Imagine implicită
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the recipe.");
                return Page();
            }
        }
    }
}
