using System.Text.Json.Serialization;

namespace Shop.ProductAPI.Models;

public record Product
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public decimal Price { get; set; }

    public long Stock {  get; set; }

    public string? ImageUrl { get; set; }

    public Guid? CategoryId { get; set; } // Foreign Key
    [JsonIgnore]
    public Category? Category { get; set; }


}
