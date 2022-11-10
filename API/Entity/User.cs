using System.ComponentModel.DataAnnotations;
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

        public int Role { get; set; }
        public int IsActive { get; set; }
    }
}