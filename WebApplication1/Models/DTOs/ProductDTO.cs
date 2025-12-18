using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shop.ProductAPI.Models.DTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Description { get; set; }
        [Precision(14,2)]
        public decimal Price { get; set; }

        public long Stock { get; set; }
        [Required]
        [MaxLength(1024)]
        public string? ImageUrl { get; set; }

        public Guid? CategoryId { get; set; } // Foreign Key
    
    }
}
