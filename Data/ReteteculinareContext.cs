using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reteteculinare.Models;

namespace Reteteculinare.Data
{
    public class ReteteculinareContext : DbContext
    {
        public ReteteculinareContext (DbContextOptions<ReteteculinareContext> options)
            : base(options)
        {
        }

        public DbSet<Reteteculinare.Models.Recipe> Recipe { get; set; } = default!;
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Favorite> Favorites { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Ingredients)
                .WithOne(i => i.Recipe)
                .HasForeignKey(i => i.RecipeID);
        }
    }
}
