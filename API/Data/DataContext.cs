using Microsoft.EntityFrameworkCore;
using API.Entity;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<UserFavorites> UserFavorites { get; set; }

        protected override void OnModelCreating(ModelBuilder builder){
            builder.Entity<User>().HasMany(u => u.Products).WithOne(p => p.User).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Product>().HasOne(p => p.User).WithMany(u => u.Products).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<UserFavorites>().HasOne(uf => uf.User).WithMany(u => u.UserFavorites).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<UserFavorites>().HasOne(uf => uf.Product).WithMany(p => p.UserFavorites).OnDelete(DeleteBehavior.NoAction);
        }
    }
}