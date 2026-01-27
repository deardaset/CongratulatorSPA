using CongratulatorSPA.Server.Data.Mappings;
using CongratulatorSPA.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace CongratulatorSPA.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        { }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new PersonMap());
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder
                .Properties<Enum>()
                .HaveConversion<string>();
        }
    }
}
