using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class CategoryViewModel
    {
        
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
