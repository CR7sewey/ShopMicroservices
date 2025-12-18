using System.Text.Json.Serialization;

namespace Shop.ProductAPI.Models;

public record Category
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    [JsonIgnore] // 1-Many 
    public ICollection<Product>? Products { get; set; }
}
