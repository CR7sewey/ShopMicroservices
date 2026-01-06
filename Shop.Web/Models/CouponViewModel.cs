using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Models
{
    public class CouponViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string CouponCode { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; } = 0.00m;
        public DateTime? ExpiryDate { get; set; }
    }
}
