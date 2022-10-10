using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class BookDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}