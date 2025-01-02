using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reteteculinare.Data;


var builder = WebApplication.CreateBuilder(args);

// Adăugăm serviciile pentru Razor Pages și contextul bazei de date
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReteteculinareContext")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>() // Folosește ApplicationDbContext pentru Identity
    .AddDefaultTokenProviders(); // Adaugă tokenuri pentru autentificare

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Ruta de login
    options.AccessDeniedPath = "/Account/AccessDenied"; // Ruta pentru acces interzis
});


var app = builder.Build();

// Configurarea pipeline-ului HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
