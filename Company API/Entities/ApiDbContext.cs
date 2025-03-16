using Microsoft.EntityFrameworkCore;

namespace Company_API.Entities
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Companies");

                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(c => c.Exchange)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Ticker)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(c => c.Isin)
                    .IsRequired()
                    .HasMaxLength(12);

                entity.HasIndex(e => e.Isin).IsUnique();

                entity.Property(c => c.Website)
                    .HasMaxLength(200);
                
            });



        }
    }
}
