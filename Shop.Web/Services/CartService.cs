using Shop.Web.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Shop.Web.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly JsonSerializerOptions _serializerOptions;
        const string CART_API = "CART_API";
        private CartViewModel _cart;

        const string DISCOUNT_API = "DISCOUNT_API";
        private CouponViewModel _coupon;

        private CheckoutViewModel _checkout;

        public CartService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<CartViewModel> GetCartByUserId(Guid userId, string token)
        {
            var client = httpClientFactory.CreateClient(CART_API);
            AppendAuthorizationHeader(token, client);
            using (var response = await client.GetAsync($"/api/Cart/getCart/{userId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStreamAsync();
                    _cart = await JsonSerializer.DeserializeAsync<CartViewModel>(data, _serializerOptions);
                }
                else
                {
                    return null;
                }
            }
            return _cart;
        }

        public async Task<CartViewModel> AddToCart(CartViewModel cartViewModel, string token)
        {
            var client = httpClientFactory.CreateClient(CART_API);
            AppendAuthorizationHeader(token, client);
            var cvm = JsonSerializer.Serialize(cartViewModel);
            StringContent content = new(cvm, Encoding.UTF8, "application/json");
            //var content = new StringContent(JsonSerializer.Serialize(cartViewModel), System.Text.Encoding.UTF8, "application/json");
            using (var response = await client.PostAsync("/api/Cart/createCart", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStreamAsync();
                    _cart = await JsonSerializer.DeserializeAsync<CartViewModel>(data, _serializerOptions);
                }
                else
                {
                    return null;
                }
            }
            return _cart;
        }

        public async Task<bool> RemoveFromCart(Guid userId, Guid productId, string token)
        {
            var client = httpClientFactory.CreateClient(CART_API);
            AppendAuthorizationHeader(token, client);
            using (var response = await client.DeleteAsync($"/api/Cart/removeCartItem/{productId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public async Task<CartViewModel> UpdateCart(CartViewModel cartViewModel, string token)
        {
            var client = httpClientFactory.CreateClient(CART_API);
            AppendAuthorizationHeader(token, client);
            var cvm = JsonSerializer.Serialize(cartViewModel);
            StringContent content = new(cvm, Encoding.UTF8, "application/json");
            //var content = new StringContent(JsonSerializer.Serialize(cartViewModel), System.Text.Encoding.UTF8, "application/json");
            using (var response = await client.PutAsync("/api/Cart/updateCart", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStreamAsync();
                    _cart = await JsonSerializer.DeserializeAsync<CartViewModel>(data, _serializerOptions);
                }
                else
                {
                    return null;
                }
            }
            return _cart;
        }

        public async Task<bool> ApplyCoupon(CartViewModel cartViewModel, Guid userId, string token)
        {
            var client = httpClientFactory.CreateClient(CART_API);
            AppendAuthorizationHeader(token, client);
            StringContent content = new(JsonSerializer.Serialize(cartViewModel), Encoding.UTF8, "application/json");
            using (var response = await client.PutAsync($"/api/Cart/applyCoupon/{userId}", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    //var data = await response.Content.ReadAsStreamAsync();
                    //await JsonSerializer.DeserializeAsync<CouponViewModel>(data, _serializerOptions);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> RemoveCoupon(Guid userId,  string token)
        {
            var client = httpClientFactory.CreateClient(CART_API);
            AppendAuthorizationHeader(token, client);
            using (var response = await client.DeleteAsync($"/api/Cart/removeCoupon/{userId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<CheckoutViewModel?> CheckoutCompleted(CheckoutViewModel checkoutViewModel, string token)
        {
            var client = httpClientFactory.CreateClient(CART_API);
            AppendAuthorizationHeader(token, client);
            StringContent content = new(JsonSerializer.Serialize(checkoutViewModel), Encoding.UTF8, "application/json");
            
            try
            {
                using (var response = await client.PostAsync($"/api/Cart/checkout", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStreamAsync();
                        _checkout = await JsonSerializer.DeserializeAsync<CheckoutViewModel>(data, _serializerOptions);
                        return _checkout;
                    }
                    else
                    {
                        // Log or handle the error response for debugging
                        var errorContent = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine($"Checkout error: {response.StatusCode} - {errorContent}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Checkout exception: {ex.Message}");
                return null;
            }
        }


        private void AppendAuthorizationHeader(string token, HttpClient client)
        {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);          
        }
    }
}
