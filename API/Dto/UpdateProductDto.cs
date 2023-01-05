using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class UpdateProductDto
    {
       public string Name { get; set; } 
       public string Description { get; set; }
       public int CategoryId { get; set; }
       public float Price { get; set; }
       public bool isSale { get; set; }
    }
}