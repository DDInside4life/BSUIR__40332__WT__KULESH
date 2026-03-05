using KULESH.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KULESH.UI.Data
{
    public class TempContext : DbContext
    {
        public TempContext(DbContextOptions<TempContext> opt) : base(opt) 
        {

        }

        public DbSet<FootballTeam> FootballTeams { get; set; }
        public DbSet<Category> Categories { get; set; }

     
    }
}
