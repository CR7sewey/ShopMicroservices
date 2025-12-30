using Microsoft.EntityFrameworkCore;
using Shop.CartAPI.Context;
using Shop.CartAPI.Models;
using Shop.CartAPI.Models.ViewModel;

namespace Shop.CartAPI.Repositories
{
    public class CartRepository : ICartRepository
    {

        private readonly ApplicationDbContext _dbContext;

        public CartRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public Task<bool> ApplyCoupon(Guid userId, string couponCode)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ClearCart(Guid userId)
        {
            var cartHeader = await _dbContext.CartHeaders
                .FirstOrDefaultAsync(u => u.UserId == userId);
            if (cartHeader is null)
            {
                return false;
            }
            var cartItems = _dbContext.CartItems
                .Where(u => u.CartHeaderId == cartHeader.Id);
            _dbContext.CartItems.RemoveRange(cartItems);
            await _dbContext.SaveChangesAsync();

            return true;

        }

        public async Task<Cart> CreateUpdateCart(Cart cart)
        {
            CartHeader c1 = await _dbContext.CartHeaders
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);
            if (c1 is null)
            {
                // Create
                var cartHeader= _dbContext.CartHeaders.Add(cart.CartHeader);
                await _dbContext.SaveChangesAsync();
                foreach (var item in cart.CartItems)
                {
                    item.CartHeaderId = cartHeader.Entity.Id;
                    // check if product exists
                    if (await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(u => u.Id == item.ProductId) is null)
                    {
                        // Create product
                        var product = _dbContext.Products.Add(item.Product);
                        await _dbContext.SaveChangesAsync();
                        item.ProductId = product.Entity.Id;
                    }
                    _dbContext.CartItems.Add(item);
                }
                Cart res = new Cart
                {
                    CartHeader = cart.CartHeader,
                    CartItems = cart.CartItems
                };
                return res;

            }
            else
            {
                // Update
                var cartItems = cart.CartItems;

                // validar se cartItems possui o mesmo produto para atualizar a quantidade
                var cartDetail = await _dbContext.CartItems
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.ProductId == cartItems.FirstOrDefault().ProductId &&
                                              p.CartHeaderId == c1.Id);

                if (cartDetail is null)
                {
                    // criar cart item
                    cart.CartItems.FirstOrDefault().CartHeaderId = c1.Id;
                    cart.CartItems.FirstOrDefault().Product = null; // evitar inserir o produto novamente
                    _dbContext.CartItems.Add(cart.CartItems.FirstOrDefault());
                }
                else
                {
                    // atualizar quantidade
                    cart.CartItems.FirstOrDefault().Product = null; // evitar inserir o produto novamente
                    cart.CartItems.FirstOrDefault().Quantity += cartDetail.Quantity;
                    cart.CartItems.FirstOrDefault().Id = cartDetail.Id;
                    cart.CartItems.FirstOrDefault().CartHeaderId = c1.Id;
                    _dbContext.CartItems.Entry(cart.CartItems.FirstOrDefault()).State = EntityState.Modified;
                }


                    /*
                    // Update
                    var cartItems = cart.CartItems;

                    foreach (var item in cartItems)
                    {
                        item.CartHeaderId = c1.Id; //cart.CartHeader.Id;
                        if (await _dbContext.CartItems.AsNoTracking().FirstOrDefaultAsync(u => u.Id == item.Id) is null) // cart item not exists
                        {
                            // New Item
                            if (await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(u => u.Id == item.ProductId) is null)
                            {
                                // Create product
                                var product=_dbContext.Products.Add(item.Product);
                                await _dbContext.SaveChangesAsync();
                                item.ProductId = product.Entity.Id;

                                _dbContext.CartItems.Add(item);
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            // Update Item
                            _dbContext.CartItems.Entry(item).State = EntityState.Modified;
                        }
                        await _dbContext.SaveChangesAsync();

                }*/
                    return cart;
            }
        }

        public async Task<Cart> GetCartByUserId(Guid userId)
        {
            Cart cart = new Cart();
            CartHeader cartHeader = await _dbContext.CartHeaders
                .FirstOrDefaultAsync(u => u.UserId == userId);
            if (cartHeader is null)
            {
                throw new Exception("User does not have a cart...");
            }
            IEnumerable<CartItem> cartItems = await _dbContext.CartItems
                .Include(u => u.Product)
                .Where(u => u.CartHeaderId == cartHeader.Id).ToListAsync();

            cart.CartItems = cartItems;
            cart.CartHeader = cartHeader;
            //await _dbContext.SaveChangesAsync();

            return cart;
        }

        public Task<bool> RemoveCoupon(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveItemCart(Guid cartItemId)
        {
            var cartItem = await _dbContext.CartItems
                .FirstOrDefaultAsync(u => u.Id == cartItemId);
            if (cartItem is null)
            {
                //throw new Exception("Cart item does not exist...");
                return false;

            }

            int totalCartItems = _dbContext.CartItems
                .Where(u => u.CartHeaderId == cartItem.CartHeaderId)
                .Count();

            _dbContext.CartItems.Remove(cartItem);


            if (totalCartItems == 1)
            {
                _dbContext.CartHeaders.Remove(
                    await _dbContext.CartHeaders
                    .FirstOrDefaultAsync(u => u.Id == cartItem.CartHeaderId));
            }
            
            //await _dbContext.SaveChangesAsync();
            return true;
                
        }
    }
}
