using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Models
{
    public class CategoryProductViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        public ICollection<ProductViewModel>? Products { get; set; }

    }
}
