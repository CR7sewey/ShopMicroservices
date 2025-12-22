using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shop.Web.Models;

public class ProductViewModel
{
    public Guid Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }
    [Required]
    [MaxLength(255)]
    public string? Description { get; set; }
    public decimal Price { get; set; }

    public long Stock { get; set; }
    [Required]
    [MaxLength(1024)]
    public string? ImageUrl { get; set; }

    public string? CategoryName { get; set; }
    [Display(Name = "Categories")]

    public Guid? CategoryId { get; set; } // Foreign Key

}
