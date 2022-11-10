using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Category { get; set; }
        public float Price { get; set; }
        public bool OnSale { get; set; }
    }
}