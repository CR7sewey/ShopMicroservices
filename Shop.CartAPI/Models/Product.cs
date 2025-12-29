using System.Text.Json.Serialization;

namespace Shop.CartAPI.Models;

public record Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public long Stock {  get; set; }

    public string? ImageUrl { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;


    [JsonIgnore]
    public ICollection<CartItem>? CartItems { get; set; }

}
