using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reteteculinare.Data;

var builder = WebApplication.CreateBuilder(args);

// Adăugăm serviciile pentru Razor Pages și contexturile bazei de date
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")));


// Înregistrăm ReteteculinareContext (pentru operațiuni specifice aplicației)
builder.Services.AddDbContext<ReteteculinareContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReteteculinareContext")));


// Configurăm Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true; // Necesită confirmare prin email
})
.AddEntityFrameworkStores<ApplicationDbContext>() // Identity folosește ApplicationDbContext
.AddDefaultTokenProviders(); // Tokenuri implicite pentru autentificare

// Configurăm cookie-urile pentru autentificare
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Ruta pentru login
    options.AccessDeniedPath = "/Account/AccessDenied"; // Ruta pentru acces interzis
});

// Construim aplicația
var app = builder.Build();

// Configurarea pipeline-ului HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // În caz de eroare, redirecționează către pagina de eroare
    app.UseHsts(); // Activează strict transportul securizat
}

app.UseHttpsRedirection(); // Redirecționează cererile HTTP către HTTPS
app.UseStaticFiles(); // Permite servirea fișierelor statice

app.UseRouting(); // Activează rutarea

app.UseAuthentication(); // Activează autentificarea
app.UseAuthorization(); // Activează autorizarea

app.MapRazorPages(); // Mapează paginile Razor

app.Run(); // Rulează aplicația
