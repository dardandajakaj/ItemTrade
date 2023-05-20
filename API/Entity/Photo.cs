#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entity
{
    public class Photo
    {
        public int Id { get; set; }
        public int BelongsTo { get; set; }
        public string Filename { get; set; }
        public bool IsMain { get; set; } = false;

        [ForeignKey("BelongsTo")]
        public Product Product { get; set; }
    }
}