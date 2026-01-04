using System.Text.Json.Serialization;

namespace Shop.Web.Models
{
    public class CartItemViewModel
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; } = 1;
        public Guid ProductId { get; set; }

        public ProductCartViewModel? Product { get; set; }
        [JsonIgnore]
        public Guid CartHeaderId { get; set; }
        [JsonIgnore]
        public CartHeaderViewModel? CartHeader { get; set; }
    }
}
