using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolSubjects.Domain;

namespace SchoolSubjects.Infrastructure;

public class SchoolContext : DbContext
{
    public SchoolContext(DbContextOptions<SchoolContext> options)
        : base(options)
    {
    }

    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Literature> Literature => Set<Literature>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Subject>().HasData(
            new Subject { Id = 1, Name = "Math", Description = "Mathematics focuses on numbers and problem-solving.", NumberOfWeeklyClasses = 5 },
            new Subject { Id = 2, Name = "English", Description = "English covers literature, writing, and grammar.", NumberOfWeeklyClasses = 4 },
            new Subject { Id = 3, Name = "Art", Description = "Art encourages creativity through drawing and painting.", NumberOfWeeklyClasses = 2 }
        );

        modelBuilder.Entity<Literature>().HasData(
            // Math
            new Literature { Id = 1, Title = "Mathematics for Beginners", SubjectId = 1 },
            new Literature { Id = 2, Title = "Advanced Algebra", SubjectId = 1 },

            // English
            new Literature { Id = 3, Title = "Shakespeare's Sonnets", SubjectId = 2 },
            new Literature { Id = 4, Title = "Modern English Grammar", SubjectId = 2 },

            // Art
            new Literature { Id = 5, Title = "The Art Book", SubjectId = 3 },
            new Literature { Id = 6, Title = "Drawing on the Right Side of the Brain", SubjectId = 3 }
        );
    }

}