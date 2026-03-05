using KULESH.Domain.Entities;
using KULESH.Domain.Models;

namespace KULESH.UI.Services
{
    public interface ITeamService
    {
        /// <summary> 
        /// Получение списка всех объектов 
        /// </summary> 
        /// <param name="category">нормализованное имя категории для фильтрации</param> 
        /// <returns></returns> 
        public Task<ResponseData<List<FootballTeam>>> GetTeamListAsync(string? category);
        /// <summary> 
        /// Поиск объекта по Id 
        /// </summary> 
        /// <param name="id">Идентификатор объекта</param> 
        /// <returns>Найденный объект или null, если объект не найден</returns> 
        public Task<ResponseData<FootballTeam>> GetTeamByIdAsync(int id);
        /// <summary> 
        /// Обновление объекта 
        /// </summary> 
        /// <param name="id">Id изменяемомго объекта</param> 
        /// <param name="product">объект с новыми параметрами</param> 
        /// <param name="formFile">Файл изображения</param>
        /// <returns></returns> 
        public Task UpdateTeamAsync(int id, FootballTeam product, IFormFile? formFile);
        /// <summary> 
        /// Удаление объекта 
        /// </summary> 
        /// <param name="id">Id удаляемомго объекта</param> 
        /// <returns></returns> 
        public Task DeleteTeamAsync(int id);
        /// <summary> 
        /// Создание объекта 
        /// </summary> 
        /// <param name="product">Новый объект</param> 
        /// <param name="formFile">Файл изображения</param> 
        /// <returns>Созданный объект</returns> 
        public Task<ResponseData<FootballTeam>> CreateTeamAsync(FootballTeam product, IFormFile? formFile);

    }
}
