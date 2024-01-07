using DemoBlazorServerApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DemoBlazorServerApp
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions)
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if(databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Specialization>()
                .Property(s => s.SpecId)
                .UseIdentityColumn();
            modelBuilder.Entity<Specialization>()
                .HasKey(s => s.SpecId)
                .HasName("PK_SpecId");
            modelBuilder.Entity<StudentRegistration>()
                .Property(s => s.StudentId)
                .UseIdentityColumn();
            modelBuilder.Entity<StudentRegistration>()
                .HasKey(s => s.StudentId)
                .HasName("PK_StudentId");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Specialization> Specs { get; set; }
        public DbSet<StudentRegistration> Students { get; set; }
    }
}
