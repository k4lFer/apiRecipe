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
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<New> News { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authentication>().ToTable("authentications");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Recipe>().ToTable("recipes");
            modelBuilder.Entity<Like>().ToTable("likes");
            modelBuilder.Entity<Rating>().ToTable("ratings");
            modelBuilder.Entity<Image>().ToTable("images");
            modelBuilder.Entity<Video>().ToTable("videos");
            modelBuilder.Entity<New>().ToTable("news");

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

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasMany(e => e.ChildRecipes)
                    .WithOne(e => e.ParentCategory)
                    .HasForeignKey(e => e.idCategory);
            });
            
            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasKey(e => e.id);
                
                entity.Property(e => e.difficulty)
                    .IsRequired()
                    .HasConversion(v => v.ToString(), v => (Difficulty)Enum.Parse(typeof(Difficulty), v))
                    .HasColumnType("enum('Easy','Half','Difficult')");
                entity.HasMany(e => e.ChildImages)
                    .WithOne(i => i.ParentRecipe)
                    .HasForeignKey(i => i.idRecipe);
                entity.HasMany(e => e.ChildVideos)
                    .WithOne(v => v.ParentRecipe)
                    .HasForeignKey(v => v.idRecipe);
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
