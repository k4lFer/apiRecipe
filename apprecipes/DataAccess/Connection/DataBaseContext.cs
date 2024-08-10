using apprecipes.DataAccess.Entity;
using apprecipes.DataTransferObject.EnumObject;
using apprecipes.Generic;
using apprecipes.Helper;
using Microsoft.EntityFrameworkCore;

namespace apprecipes.DataAccess.Connection
{
    public class DataBaseContext : DataBaseContextGeneric
    {
        public DbSet<Authentication> Authentications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authentication>().ToTable("authentications");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Image>().ToTable("images");

            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Authentication>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.role)
                    .IsRequired()
                    .HasConversion(v => v.ToString(), v => (Role)Enum.Parse(typeof(Role), v))
                    .HasColumnType("enum('Admin','Other','Logged')");
            });
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasOne(e => e.ChildAthentication)
                    .WithOne(e => e.ParentUser)
                    .HasForeignKey<Authentication>(i => i.id)
                    .IsRequired();
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            { 
                string connectionString = AppSettings.GetConnectionStringMariaDb();
                ServerVersion serverVersion = new MySqlServerVersion(new Version(8, 4, 0));
                optionsBuilder.UseMySql(connectionString, serverVersion);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }  
    }
}
