using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entity
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Fullname { get; set; }

        [EmailAddress , Required]
        public string Email { get; set; }
        
        [Required]
        public string UserName { get; set; } 

        [Required]
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Registered { get; set; }
        public int Role { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<UserFavorites> UserFavorites { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<Conversation> ConversationsSender { get; set; }
        public ICollection<Conversation> ConversationsReceiver { get; set; }
    }
}