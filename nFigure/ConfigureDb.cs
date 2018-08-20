using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace nFigure
{
    public class ConfigureDb : DbContext
    {
        public DbSet<ContainerForArrayFigures> FiguresContainer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optBuilder)
        {
            string stringConnection = ConfigurationManager.ConnectionStrings["dbOfFigures"].ConnectionString; 
            optBuilder.UseNpgsql(stringConnection);
        }
    }
}
