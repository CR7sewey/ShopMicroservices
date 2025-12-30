using System.Text.Json.Serialization;

namespace Shop.CartAPI.Models.DTOs
{
    public class CartItemDTO
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; } = 1;
        public Guid ProductId { get; set; }

        public ProductDTO? Product { get; set; }
        [JsonIgnore]
        public Guid CartHeaderId { get; set; }
        [JsonIgnore]
        public CartHeaderDTO? CartHeader { get; set; }

    }
}
