using Microsoft.Extensions.Http;
using Shop.Web.Models;
using System.Text.Json;

namespace Shop.Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly JsonSerializerOptions _serializerOptions;

        const string PRODUCT_API = "PRODUCT_API";
        private IEnumerable<CategoryViewModel> _categories;

        public CategoryService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        public async Task<IEnumerable<CategoryViewModel>> GetAllCategories()
        {
            var client = httpClientFactory.CreateClient(PRODUCT_API);
            using (var response = await client.GetAsync("/api/Category"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStreamAsync();
                    _categories = await JsonSerializer.DeserializeAsync<IEnumerable<CategoryViewModel>>(data, _serializerOptions);
                }
                else
                {
                    return null;
                }
            }
            return _categories;
        }
    }
}
