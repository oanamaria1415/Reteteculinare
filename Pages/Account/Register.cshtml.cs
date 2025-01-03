
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Reteteculinare.Pages.Account
    {
        public class RegisterModel : PageModel
        {
            private readonly UserManager<IdentityUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly SignInManager<IdentityUser> _signInManager; // Adăugare SignInManager

            public RegisterModel(
                UserManager<IdentityUser> userManager,
                RoleManager<IdentityRole> roleManager,
                SignInManager<IdentityUser> signInManager) // Adaugă SignInManager în constructor
            {
                _userManager = userManager;
                _roleManager = roleManager;
                _signInManager = signInManager; // Inițializare
            }

            [BindProperty]
            public InputModel Input { get; set; }

            public class InputModel
            {
                public string Email { get; set; }
                public string Password { get; set; }
                public string ConfirmPassword { get; set; }
            }

            public async Task<IActionResult> OnPostAsync()
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                // Crează un nou utilizator
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    // Crearea rolurilor
                    if (!await _roleManager.RoleExistsAsync("Admin"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    }

                    if (!await _roleManager.RoleExistsAsync("User"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("User"));
                    }

                    // Atribuie rolul de "User"
                    await _userManager.AddToRoleAsync(user, "User");

                    // Autentifică utilizatorul imediat după înregistrare
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Redirecționează către pagina de Recipes
                    return RedirectToPage("/Recipes/Index");
                }

                // Afișează erorile, dacă există
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }
        }
    }

