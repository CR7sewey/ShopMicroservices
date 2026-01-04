namespace Shop.Web.Models
{
    public class CartViewModel
    {
        public CartHeaderViewModel? CartHeader { get; set; }
        public IEnumerable<CartItemViewModel> CartItems { get; set; } = [];
    }
}
