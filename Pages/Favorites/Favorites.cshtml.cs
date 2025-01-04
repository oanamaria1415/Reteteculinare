using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Reteteculinare.Data;
using Reteteculinare.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reteteculinare.Pages
{
    public class FavoritesModel : PageModel
    {
        private readonly ReteteculinareContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FavoritesModel(ReteteculinareContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Favorite> Favorites { get; set; } = new List<Favorite>();

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                Favorites = new List<Favorite>();
                return;
            }

            Favorites = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.Recipe) // Include relația cu rețeta
                .ToListAsync();
        }
    }
}
