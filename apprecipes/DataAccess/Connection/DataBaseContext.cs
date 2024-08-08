using apprecipes.DataAccess.Entity;
using apprecipes.DataTransferObject.EnumObject;
using apprecipes.Generic;
using apprecipes.Helper;
using Microsoft.EntityFrameworkCore;

namespace apprecipes.DataAccess.Connection
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Authentication> Authentications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        
        public DataBaseContext()
        {
            AutoMapper.Start();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>().ToTable("images");
            modelBuilder.Entity<Authentication>().ToTable("authentications");
            modelBuilder.Entity<Authentication>().ToTable("users");

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
