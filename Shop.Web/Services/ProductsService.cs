using Shop.Web.Models;
using System.Text.Json;

namespace Shop.Web.Services
{
    public class ProductsService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _serializerOptions;


        const string PRODUCT_API = "PRODUCT_API";

        private IEnumerable<ProductViewModel> products;


        public ProductsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        }

        public async Task<IEnumerable<ProductViewModel>> GetProducts()
        {

            var client = _httpClientFactory.CreateClient(PRODUCT_API);
            using (var request = await client.GetAsync("/api/Product"))
            {
                if (request.IsSuccessStatusCode)
                {
                    var data = await request.Content.ReadAsStreamAsync();
                    products = JsonSerializer.Deserialize<IEnumerable<ProductViewModel>>(data, _serializerOptions);
                }
                else
                {
                    return null;
                }
            }
            return products;

        }

    }
}
