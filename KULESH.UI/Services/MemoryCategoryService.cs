using KULESH.Domain.Entities;
using KULESH.Domain.Models;

namespace KULESH.UI.Services
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category {Id=1, Name="Ла Лига",NormalizedName="LaLiga"},
                new Category {Id=2, Name="Премьер-лига", NormalizedName="PremierLeague"},
                new Category {Id=3, Name="Серия А", NormalizedName="SerieA"},
                new Category {Id=4, Name="Бундеслига", NormalizedName="Bundesliga"},
                new Category {Id=5, Name="Лига 1", NormalizedName="Ligue1"},

            };
            var result = new ResponseData<List<Category>>();
            result.Data = categories;
            return Task.FromResult(result);
        }
    }
}
