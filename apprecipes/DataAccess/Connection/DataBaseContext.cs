using apprecipes.Helper;
using apprecipes.Models;
using Microsoft.EntityFrameworkCore;

namespace apprecipes.DataAccess.Connection
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Image> Images { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>().ToTable("images");

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            { 
                string connectionString = AppSettings.GetConnectionStringMariaDb();
                ServerVersion serverVersion = new MySqlServerVersion(new Version(11, 4, 2));
                optionsBuilder.UseMySql(connectionString, serverVersion);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }  
    }
}
