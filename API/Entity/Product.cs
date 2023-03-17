using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entity
{
    public class Product
    {
        [Required]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }

        public int CategoryId { get; set; }
        public float Price { get; set; }
        public bool IsSale { get; set; }

        [ForeignKey("InsertedBy")]
        public User User { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public ICollection<UserFavorites> UserFavorites { get; set; }
        public ICollection<Conversation> Conversations { get; set; }
    }
}