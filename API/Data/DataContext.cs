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

        protected override void OnModelCreating(ModelBuilder builder){
            builder.Entity<User>().HasMany(u => u.Products).WithOne(p => p.User).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Product>().HasOne(p => p.User).WithMany(u => u.Products).HasForeignKey(p => new{ p.InsertedBy }).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => new {p.CategoryId}).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Category>().HasOne(c => c.User).WithMany( u => u.Categories).HasForeignKey(c => new{c.AddedBy}).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<UserFavorites>().HasKey(uf => new{uf.Id});
            builder.Entity<UserFavorites>().HasAlternateKey(uf => new {uf.UserId, uf.ProductId});
            builder.Entity<UserFavorites>().HasOne(uf => uf.User).WithMany(u => u.UserFavorites).HasForeignKey( uf => new{uf.UserId}).OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<UserFavorites>().HasOne(uf => uf.Product).WithMany(p => p.UserFavorites).HasForeignKey( uf => new {uf.ProductId}).OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<Message>().HasOne(m => m.Sender).WithMany(u => u.Messages).HasForeignKey(m => new{ m.SenderId}).OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<Message>().HasOne(m => m.Conversation).WithMany(c => c.Messages).HasForeignKey(m => new{ m.ConversationId}).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Conversation>().HasOne(c => c.Sender).WithMany(s => s.ConversationsSender).HasForeignKey(c => new {c.SenderId}).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Conversation>().HasOne(c => c.Receiver).WithMany(r => r.ConversationsReceiver).HasForeignKey(c => new{ c.ReceiverId}).OnDelete(DeleteBehavior.Restrict);
        }
    }
}