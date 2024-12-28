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

        public IList<Recipe> Recipe { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Recipe = await _context.Recipe.ToListAsync();
        }
    }
}
