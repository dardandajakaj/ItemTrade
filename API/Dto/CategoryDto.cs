using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public float VAT { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
    }
}