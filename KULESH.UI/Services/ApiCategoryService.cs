using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KULESH.Domain.Entities;
using KULESH.Domain.Models;

namespace KULESH.UI.Services
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _http;

        public ApiCategoryService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            try
            {
                var resp = await _http.GetAsync("api/Categories");
                if (!resp.IsSuccessStatusCode)
                {
                    return ResponseData<List<Category>>.Error($"API error: {(int)resp.StatusCode} {resp.ReasonPhrase}");
                }

                var categories = await resp.Content.ReadFromJsonAsync<List<Category>>();
                return ResponseData<List<Category>>.OK(categories ?? new List<Category>());
            }
            catch (Exception ex)
            {
                return ResponseData<List<Category>>.Error(ex.Message);
            }
        }
    }
}
