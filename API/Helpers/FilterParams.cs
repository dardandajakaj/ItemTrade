using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class FilterParams
    {
        public string Name { get; set; }
        public int Category { get; set; } = 0;
        public int MinPrice { get; set; } = 0;
        public int MaxPrice { get; set; } = 0;
        public int sort { get; set; } = 3;
    }
}