using System.ComponentModel.DataAnnotations;

namespace API.Entity
{
    public class Book
    {
        [Key]
        public String ISBN { get; set; }
        public String Title { get; set; }
        public String Author { get; set; }
        public int Category { get; set; }
        public String Language { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Edition { get; set; }
    }
}