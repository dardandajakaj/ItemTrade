using Microsoft.EntityFrameworkCore;
using API.Entity;

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
        public DbSet<Message> Message { get; set; }

        protected override void OnModelCreating(ModelBuilder builder){
            builder.Entity<User>().HasMany(u => u.Products).WithOne(p => p.User).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Product>().HasOne(p => p.User).WithMany(u => u.Products).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Category>().HasMany(c => c.Products).WithOne(p => p.Category).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<UserFavorites>().HasKey(uf => new {uf.UserId, uf.ProductId});
            builder.Entity<UserFavorites>().HasOne(uf => uf.User).WithMany(u => u.UserFavorites).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<UserFavorites>().HasOne(uf => uf.Product).WithMany(p => p.UserFavorites).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Message>().HasOne(s => s.Sender).WithMany(m => m.MessagesSent).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Message>().HasOne(r => r.Receiver).WithMany(m => m.MessagesReceived).OnDelete(DeleteBehavior.Restrict);
        }
    }
}