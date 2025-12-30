using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Shop.CartAPI.Models.DTOs;

public class ProductDTO
{
    
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = String.Empty;
        [MaxLength(255)]
        public string? Description { get; set; } = string.Empty;
        [Precision(14, 2)]
        public decimal Price { get; set; }

        public long Stock { get; set; }
        [MaxLength(1024)]
        public string ImageUrl { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

}
