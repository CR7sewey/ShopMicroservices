using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shop.CartAPI.Models.DTOs
{
    public class CartHeaderDTO
    {
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string CouponCode { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<CartItemDTO> CartItems { get; set; } = [];
    }
}
