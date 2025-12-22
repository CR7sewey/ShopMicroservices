using Shop.Web.Models;
using System.Text;
using System.Text.Json;

namespace Shop.Web.Services
{
    public class ProductsService : IProductService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _serializerOptions;


        const string PRODUCT_API = "PRODUCT_API";

        private IEnumerable<ProductViewModel> products;
        private ProductViewModel product;


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
                    products = await JsonSerializer.DeserializeAsync<IEnumerable<ProductViewModel>>(data, _serializerOptions);
                }
                else
                {
                    return null;
                }
            }
            return products;

        }

        public async Task<ProductViewModel> GetProduct(Guid id)
        {
            var client = _httpClientFactory.CreateClient(PRODUCT_API);
            using (var response = await client.GetAsync($"/api/Product/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStreamAsync();
                    product = await JsonSerializer.DeserializeAsync<ProductViewModel>(data, _serializerOptions);
                }
                else
                {
                    return null;
                }
            }
            return product;
        }

        public async Task<ProductViewModel> CreateProduct(ProductViewModel productVM)
        {
            var client = _httpClientFactory.CreateClient(PRODUCT_API);
            var p = JsonSerializer.Serialize(productVM);
            StringContent content = new StringContent(p, Encoding.UTF8, "application/json");
            using (var response = await client.PostAsync("/api/Product", content)) {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStreamAsync();
                    product = await JsonSerializer.DeserializeAsync<ProductViewModel>(data, _serializerOptions);
                }
                else
                {
                    return null;
                }
            }
            return product;

        }

        public async Task<ProductViewModel> UpdateProduct(Guid id, ProductViewModel productVM)
        {
            var client = _httpClientFactory.CreateClient(PRODUCT_API);
            var p = JsonSerializer.Serialize(productVM);
            StringContent content = new StringContent(p, Encoding.UTF8, "application/json");
            using (var response = await client.PutAsync($"/api/Product/{id}", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    //var data = await response.Content.ReadAsStreamAsync();
                    //product = await JsonSerializer.DeserializeAsync<ProductViewModel>(data, _serializerOptions);
                    product = productVM;
                }
                else
                {
                    return null;
                }
            }
            return product;
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            var client = _httpClientFactory.CreateClient(PRODUCT_API);
            using (var response = await client.DeleteAsync($"/api/Product/{id}"))
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
    }
}
