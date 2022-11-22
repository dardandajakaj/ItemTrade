using System.ComponentModel.DataAnnotations.Schema;
namespace API.Entity
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public float VAT { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }

        [ForeignKey("AddedBy")]
        public User User { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}