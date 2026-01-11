using System.ComponentModel.DataAnnotations;

namespace Shop.CartAPI.Models.DTOs
{
    public class CheckoutDTO
    {
        public Guid Id { get; set; }
        public CartHeaderDTO? CartHeader { get; set; }

        // checkout details
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        
        [EmailAddress]
        [Required]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        
        // Credit Card Info
        [Required]
        public string CardNumber { get; set; } = string.Empty;
        
        [Required]
        public string NameOnCard { get; set; } = string.Empty;
        
        [Required]
        [StringLength(4, MinimumLength = 3)]
        public string CVV { get; set; } = string.Empty;
        
        [Required]
        public string ExpireMonthYear { get; set; } = string.Empty;

        public IEnumerable<CartItemDTO> CartItems { get; set; } = [];
    }
}
