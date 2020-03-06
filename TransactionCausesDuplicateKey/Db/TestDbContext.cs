using Microsoft.EntityFrameworkCore;

namespace TransactionCausesDuplicateKey.Db
{
    public class TestDbContext : DbContext
    {
        public DbSet<DbEmployee> Employees { get; set; }
        public DbSet<DbEvent> Events { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbEmployee>()
                .HasIndex(e => new { e.PersonId, e.PersonNumber })
                .IsUnique(true);

            modelBuilder.Entity<DbEvent>()
                .HasIndex(e => new { e.UniqueEventId })
                .IsUnique(true);
        }
    }
}
