using System.Text.Json.Serialization;

namespace Shop.CartAPI.Models
{
    public class CartHeader
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CouponCode { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<CartItem> CartItems { get; set; } = [];
    }
}
