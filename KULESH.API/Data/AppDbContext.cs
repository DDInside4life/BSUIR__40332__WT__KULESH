using KULESH.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KULESH.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
          
        }

        public DbSet<FootballTeam> FootballTeams { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
