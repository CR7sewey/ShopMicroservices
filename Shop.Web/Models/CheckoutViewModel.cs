using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Shop.Web.Models
{
    public class CheckoutViewModel
    {
        public Guid Id { get; set; }
        public CartHeaderViewModel? CartHeader { get; set; }

        // checkout details
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;
        
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; } = string.Empty;
        
        // Credit Card Info
        [Required(ErrorMessage = "Card number is required")]
        public string CardNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Name on card is required")]
        public string NameOnCard { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "CVV is required")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "CVV must be 3 or 4 digits")]
        public string CVV { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Expiration date is required")]
        public string ExpireMonthYear { get; set; } = string.Empty;

        public IEnumerable<CartItemViewModel> CartItems { get; set; } = [];
    }
}
