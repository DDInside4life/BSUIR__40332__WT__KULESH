using KULESH.Domain.Entities;
using KULESH.Domain.Models;

namespace KULESH.UI.Services
{
    public interface ICategoryService
    {
        // Получение списка всех категорий
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
