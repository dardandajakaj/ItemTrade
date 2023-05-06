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
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>().HasOne(c => c.User).WithMany(u => u.Categories).HasForeignKey(c => new { c.AddedBy }).OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<Product>().HasOne(p => p.User).WithMany(u => u.Products).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => new { p.CategoryId });
            builder.Entity<UserFavorites>()
                    .HasOne(uf => uf.User).WithMany(u => u.UserFavorites).HasForeignKey(uf => new { uf.UserId }).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<UserFavorites>().HasOne(uf => uf.Product).WithMany(p => p.UserFavorites).HasForeignKey(uf => new { uf.ProductId }).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<UserFavorites>().HasAlternateKey(uf => new { uf.UserId, uf.ProductId });
            builder.Entity<Conversation>().HasAlternateKey(c => new{c.ProductId, c.SenderId, c.ReceiverId});
            builder.Entity<Conversation>().HasOne(c => c.Product).WithMany(p => p.Conversations).HasForeignKey(c => new { c.ProductId }).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Conversation>().HasOne(c => c.Receiver).WithMany(r => r.ConversationsReceiver).HasForeignKey(c => new { c.ReceiverId }).OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<Conversation>().HasOne(c => c.Sender).WithMany(s => s.ConversationsSender).HasForeignKey(c => new { c.SenderId }).OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<Message>().HasOne(m => m.Conversation).WithMany(c => c.Messages).HasForeignKey(m => new { m.ConversationId }).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Message>().HasOne(m => m.Sender).WithMany(s => s.Messages).HasForeignKey(m => new { m.SenderId }).OnDelete(DeleteBehavior.Restrict);
        }
    }
}