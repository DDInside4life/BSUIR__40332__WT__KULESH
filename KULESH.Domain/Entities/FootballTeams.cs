using System;
using System.Collections.Generic;
using System.Text;

namespace KULESH.Domain.Entities
{
    public class FootballTeams
    {
        public int Id { get; set; } // id команды
        public string Name { get; set; } // Название команды
        public string Description { get; set; } // Описание команды
        public int Points { get; set; } // Количество очков
        public string? Image { get; set; } // Путь к файлу изображения


        // Навигационные свойства
        /// <summary>
        /// группа блюд (например, супы, напитки и т.д.)
        /// </summary>
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
