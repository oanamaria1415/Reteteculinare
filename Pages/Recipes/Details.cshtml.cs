using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
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

        public async Task<IActionResult> OnGetDownloadPdfAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Preluarea datelor rețetei, inclusiv a ingredientelor
            Recipe = await _context.Recipe
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Recipe == null)
            {
                return NotFound();
            }
            


            // Generarea fișierului PDF
            using (var memoryStream = new MemoryStream())
            {
                var writer = new PdfWriter(memoryStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                var fontPath = Path.Combine("wwwroot", "fonts", "Roboto-Regular.ttf");
                var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

                // Titlul rețetei
                document.Add(new Paragraph(Recipe.RecipeName)
                    .SetFontSize(24)
                   
                    .SetTextAlignment(TextAlignment.CENTER));

                // Detalii despre rețetă
                document.Add(new Paragraph($"Chef: {Recipe.ChefName}")
                    .SetFontSize(16));
                document.Add(new Paragraph($"Category: {Recipe.Category}")
                    .SetFontSize(16));

                // Ingrediente
                document.Add(new Paragraph("Ingredients:")
                    .SetFontSize(18));

                if (Recipe.Ingredients != null && Recipe.Ingredients.Any())
                {
                    var list = new List();
                    foreach (var ingredient in Recipe.Ingredients)
                    {
                        list.Add(new ListItem($"{ingredient.Name} - {ingredient.Quantity} {ingredient.Unit}"));
                    }
                    document.Add(list);
                }
                else
                {
                    document.Add(new Paragraph("No ingredients available."));
                }

                // Instrucțiuni
                document.Add(new Paragraph("Instructions:")
                    .SetFontSize(18));
                document.Add(new Paragraph(Recipe.Instructions));

                document.Close();

                // Returnează fișierul PDF
                return File(memoryStream.ToArray(), "application/pdf", $"{Recipe.RecipeName}.pdf");
            }
        }
    }
}
