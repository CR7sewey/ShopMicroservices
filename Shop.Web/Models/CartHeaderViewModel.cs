using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shop.Web.Models
{
    public class CartHeaderViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<CartItemViewModel> CartItems { get; set; } = [];

        public double TotalAmount { get; set; } = 0.00d;
    }
}
