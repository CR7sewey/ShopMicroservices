using System.ComponentModel.DataAnnotations;

namespace Shop.ProductAPI.Models.DTOs
{
    public class CategoryProductDTO
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        public ICollection<Product>? Products { get; set; }

    }
}
