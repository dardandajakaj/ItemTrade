using API.Entity;

namespace API.Dto
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public int CategoryId { get; set; }
        public float Price { get; set; }
        public bool IsSale { get; set; }
        public string Owner { get; set; }
        public string Address { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}