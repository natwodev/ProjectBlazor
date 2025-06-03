using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BlazorApp2.DTOs;

namespace BlazorApp2.Services
{
    public class CategoryService
    {
        private readonly HttpClient _http;

        public CategoryService(HttpClient http)
        {
            _http = http;
        }
        
        public async Task<List<CategoryDto>> GetAllCategorysAsync()
        {
            var categorys = await _http.GetFromJsonAsync<List<CategoryDto>>("api/category");
            return categorys ?? new List<CategoryDto>();
        }
        
        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _http.GetFromJsonAsync<CategoryDto>($"api/category/{id}");
            return category;
        }

        public async Task<HttpResponseMessage> CreateCategoryAsync(CreateCategoryDto newCategory)
        {
            return await _http.PostAsJsonAsync("api/category", newCategory);
        }

        public async Task<HttpResponseMessage> UpdateCategoryAsync(int id, UpdateCategoryDto updatedCategory)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/category/{id}")
            {
                Content = JsonContent.Create(updatedCategory)
            };

            return await _http.SendAsync(request);
        }
        
        public async Task<HttpResponseMessage> DeleteCategoryAsync(int id)
        {
            return await _http.DeleteAsync($"api/category/{id}");
        }
    }
}