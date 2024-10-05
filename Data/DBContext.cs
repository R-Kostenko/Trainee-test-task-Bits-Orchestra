using Microsoft.EntityFrameworkCore;
using Trainee_Test.Models;

namespace Trainee_Test.Data;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options) { }

    public DbSet<PersonEntity> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersonEntity>()
            .HasKey(e => e.Id);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Trainee_Test.Models.PersonDTO> PersonDTO { get; set; } = default!;
}