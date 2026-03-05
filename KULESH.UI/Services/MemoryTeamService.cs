using KULESH.Domain.Entities;
using KULESH.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace KULESH.UI.Services
{
    public class MemoryTeamService : ITeamService
    {
        private readonly IConfiguration _config;
        List<FootballTeam> _footballTeams;
        List<Category> _categories;

        public MemoryTeamService([FromServices] IConfiguration config, ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync().Result.Data;
            _config = config;
            SetupData();
        }

        public Task<ResponseData<FootballTeam>> CreateTeamAsync(FootballTeam product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTeamAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<FootballTeam>> GetTeamByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<List<FootballTeam>>> GetTeamListAsync(string? category)
        {
            ResponseData<List<FootballTeam>> result;
            int? categoryId = null;

            if (category != null)
            {
                categoryId = _categories
                    .Find(c => c.NormalizedName
                    .Equals(category))?
                    .Id;
            }

            var data = _footballTeams
                .Where(f => categoryId == null || f.CategoryId.Equals(categoryId))?
                .ToList();

            if (data.Count == 0)
            {
                result = ResponseData<List<FootballTeam>>
                    .Error("Нет объектов в выбранной категории");
            }
            else
            {
                result = ResponseData<List<FootballTeam>>.OK(data);
            }

            return Task.FromResult(result);

            //var response = new ResponseData<List<FootballTeam>>();

            //var data = string.IsNullOrEmpty(category)
            //    ? _footballTeams
            //    : _footballTeams.Where(t => t.Category?.NormalizedName == category).ToList();

            //response.Data = data;
            //response.Success = true;

            //return Task.FromResult(response);
        }

        public Task UpdateTeamAsync(int id, FootballTeam product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        private void SetupData()
        {
            _footballTeams = new List<FootballTeam>();

          
            //var laLigaCategory = _categories?.FirstOrDefault(c => c.NormalizedName.Equals("LaLiga"));
            //if (laLigaCategory != null)
            {
                _footballTeams.Add(new FootballTeam
                {
                    Id = 1,
                    Name = "Реал Мадрид",
                    Description = "Испанский клуб",
                    Points = 100,
                    Image = "/images/real-madrid.png",
                    CategoryId = _categories.Find(c =>c.NormalizedName.Equals("LaLiga")).Id
                });

                _footballTeams.Add(new FootballTeam
                {
                    Id = 2,
                    Name = "Барселона",
                    Description = "Испанский клуб",
                    Points = 45,
                    Image = "/images/barcelona.png",
                    CategoryId = _categories.Find(c => c.NormalizedName.Equals("LaLiga")).Id
                });

                _footballTeams.Add(new FootballTeam
                {
                    Id = 3,
                    Name = "Боруссия Дортмунд",
                    Description = "Немецкий клуб",
                    Points = 73,
                    Image = "/images/borussia-dortmund.png",
                    CategoryId = _categories.Find(c => c.NormalizedName.Equals("Bundesliga")).Id
                });

                _footballTeams.Add(new FootballTeam
                {
                    Id = 4,
                    Name = "Челси",
                    Description = "Английский клуб",
                    Points = 95,
                    Image = "/images/chelsea.png",
                    CategoryId = _categories.Find(c => c.NormalizedName.Equals("PremierLeague")).Id
                });

                _footballTeams.Add(new FootballTeam
                {
                    Id = 5,
                    Name = "Манчестер Сити",
                    Description = "Английский клуб",
                    Points = 70,
                    Image = "/images/manchester-city.png",
                    CategoryId = _categories.Find(c => c.NormalizedName.Equals("PremierLeague")).Id
                });

                _footballTeams.Add(new FootballTeam
                {
                    Id = 6,
                    Name = "Манчестер Юнайтед",
                    Description = "Английский клуб",
                    Points = 87,
                    Image = "/images/manchester-united.png",
                    CategoryId = _categories.Find(c => c.NormalizedName.Equals("PremierLeague")).Id
                });

                _footballTeams.Add(new FootballTeam
                {
                    Id = 7,
                    Name = "Милан",
                    Description = "Итальянский клуб",
                    Points = 88,
                    Image = "/images/milan.png",
                    CategoryId = _categories.Find(c => c.NormalizedName.Equals("SerieA")).Id
                });

                _footballTeams.Add(new FootballTeam
                {
                    Id = 8,
                    Name = "ПСЖ",
                    Description = "Французский клуб",
                    Points = 65,
                    Image = "/images/psg.png",
                    CategoryId = _categories.Find(c => c.NormalizedName.Equals("Ligue1")).Id
                });
            }
        }
    }
}

       
