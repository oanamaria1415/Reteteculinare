using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

public class LoginModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(SignInManager<IdentityUser> signInManager,
                      UserManager<IdentityUser> userManager,
                      ILogger<LoginModel> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public string ReturnUrl { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public void OnGet(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        ReturnUrl = returnUrl ?? Url.Content("~/");
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/Recipes");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Normalizează email-ul introdus de utilizator
        var normalizedEmail = Input.Email.ToUpper();

        // Găsim utilizatorul pe baza email-ului normalizat
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt. User not found.");
            return Page();
        }

        // Comparăm parola în clar (doar pentru testare, fără hash)
        if (Input.Password != user.PasswordHash)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt. Password mismatch.");
            return Page();
        }

        // Logăm utilizatorul
        await _signInManager.SignInAsync(user, isPersistent: false);
        return LocalRedirect(returnUrl);
    }

    // Metodă pentru Logout
    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return RedirectToPage("/Account/Login");
    }
}

