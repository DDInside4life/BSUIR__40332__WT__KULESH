using KULESH.Domain.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace KULESH.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // Выполнение миграций перед заполнением
            await context.Database.MigrateAsync();
            // Заполнение данными
            if (!context.Categories.Any() && !context.FootballTeams.Any())
            {
                var categories = new List<Category>
            {
                new Category { Name = "Ла Лига", NormalizedName = "LaLiga" },
                new Category { Name = "Премьер-лига", NormalizedName = "PremierLeague" },
                new Category { Name = "Серия А", NormalizedName = "SerieA" },
                new Category { Name = "Бундеслига", NormalizedName = "Bundesliga" },
                new Category { Name = "Лига 1", NormalizedName = "Ligue1" },

            };
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();

                // Используем CategoryId чтобы явно привязать команды к категориям и не задавать Id вручную
                var footballTeams = new List<FootballTeam>
                {
                    new FootballTeam
                    {
                        Name = "Реал Мадрид",
                        Description = "Испанский клуб",
                        Points = 100,
                        Image = "/Images/real-madrid.png",
                        CategoryId = categories.First(c => c.NormalizedName.Equals("LaLiga", StringComparison.OrdinalIgnoreCase)).Id
                    },

                    new FootballTeam
                    {
                        Name = "Барселона",
                        Description = "Испанский клуб",
                        Points = 45,
                        Image = "/Images/barcelona.png",
                        CategoryId = categories.First(c => c.NormalizedName.Equals("LaLiga", StringComparison.OrdinalIgnoreCase)).Id
                    },

                    new FootballTeam
                    {
                        Name = "Боруссия Дортмунд",
                        Description = "Немецкий клуб",
                        Points = 73,
                        Image = "/Images/borussia-dortmund.png",
                        CategoryId = categories.First(c => c.NormalizedName.Equals("Bundesliga", StringComparison.OrdinalIgnoreCase)).Id
                    },

                    new FootballTeam
                    {
                        Name = "Челси",
                        Description = "Английский клуб",
                        Points = 95,
                        Image = "/Images/chelsea.png",
                        CategoryId = categories.First(c => c.NormalizedName.Equals("PremierLeague", StringComparison.OrdinalIgnoreCase)).Id
                    },

                    new FootballTeam
                    {
                        Name = "Манчестер Сити",
                        Description = "Английский клуб",
                        Points = 70,
                        Image = "/Images/manchester-city.png",
                        CategoryId = categories.First(c => c.NormalizedName.Equals("PremierLeague", StringComparison.OrdinalIgnoreCase)).Id
                    },

                    new FootballTeam
                    {
                        Name = "Манчестер Юнайтед",
                        Description = "Английский клуб",
                        Points = 87,
                        Image = "/Images/manchester-united.png",
                        CategoryId = categories.First(c => c.NormalizedName.Equals("PremierLeague", StringComparison.OrdinalIgnoreCase)).Id
                    },

                    new FootballTeam
                    {
                        Name = "Милан",
                        Description = "Итальянский клуб",
                        Points = 88,
                        Image = "/Images/milan.png",
                        CategoryId = categories.First(c => c.NormalizedName.Equals("SerieA", StringComparison.OrdinalIgnoreCase)).Id
                    },

                    new FootballTeam
                    {
                        Name = "ПСЖ",
                        Description = "Французский клуб",
                        Points = 65,
                        Image = "/Images/psg.png",
                        CategoryId = categories.First(c => c.NormalizedName.Equals("Ligue1", StringComparison.OrdinalIgnoreCase)).Id
                    }

                };

                await context.FootballTeams.AddRangeAsync(footballTeams);
                await context.SaveChangesAsync();
            }
        }
    }
}
