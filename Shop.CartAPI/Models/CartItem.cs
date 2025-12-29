using System.Text.Json.Serialization;

namespace Shop.CartAPI.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; } = 1;
        public Guid ProductId { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }

        public Guid CartHeaderId { get; set; }

        [JsonIgnore]
        public CartHeader? CartHeader { get; set; }



    }
}
