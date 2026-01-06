using Shop.Web.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Shop.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly JsonSerializerOptions _serializerOptions;
      

        const string DISCOUNT_API = "DISCOUNT_API";
        private CouponViewModel _coupon;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<CouponViewModel?> GetCoupon(string couponCode, string token)
        {
            var client = httpClientFactory.CreateClient(DISCOUNT_API);
            AppendAuthorizationHeader(token, client);
            using (var response = await client.GetAsync($"/api/Coupon/{couponCode}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStreamAsync();
                    _coupon = await JsonSerializer.DeserializeAsync<CouponViewModel>(data, _serializerOptions);
                    return _coupon;
                }
                else
                {
                    return null;
                }
            }
        }
        private void AppendAuthorizationHeader(string token, HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
