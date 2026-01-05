using System.ComponentModel.DataAnnotations;

namespace Shop.DiscountAPI.Models
{
    public class CouponDTO
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string CouponCode { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; } = 0.00m;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiryDate { get; set; }
    }
}
