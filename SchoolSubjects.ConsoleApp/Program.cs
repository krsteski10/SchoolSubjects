using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolSubjects.Infrastructure;
using SchoolSubjects.Application.Interfaces;
using SchoolSubjects.Infrastructure.Repositories;
using SchoolSubjects.Application.Services;
using SchoolSubjects.Infrastructure.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

class Program
{
    static async Task Main(string[] args)
    {
        //var configuration = new ConfigurationBuilder()
        //    .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile("appsettings.json", optional: false)
        //    .Build();

        //var services = new ServiceCollection();

        //// Register DbContext
        //services.AddDbContext<SchoolContext>(options =>
        //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        //// Register Dependencies
        //services.AddMemoryCache();
        //services.AddScoped<ISubjectRepository, SubjectRepository>();
        //services.AddScoped<ISubjectService, SubjectService>();

        //var serviceProvider = services.BuildServiceProvider();

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders(); // Disables all logging output
            })
            .ConfigureServices((context, services) =>
            {
                // Configuration
                var config = context.Configuration;

                // DbContext
                services.AddDbContext<SchoolContext>(options =>
                    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

                // App Services
                services.AddMemoryCache();
                services.AddScoped<ISubjectRepository, SubjectRepository>();
                services.AddScoped<ISubjectService, SubjectService>();
            })
            .Build();

        using var scope = host.Services.CreateScope();
        var subjectService = scope.ServiceProvider.GetRequiredService<ISubjectService>();

        Console.WriteLine("Welcome to the School Subjects Information System!");
        Console.WriteLine("1. List Subjects");
        Console.WriteLine("2. View Subject Details");
        Console.WriteLine("3. Exit");

        while (true)
        {
            Console.Write("\nEnter your choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ListSubjects(subjectService);
                    break;
                case "2":
                    await ViewSubjectDetails(subjectService);
                    break;
                case "3":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static async Task ListSubjects(ISubjectService subjectService)
    {
        var subjects = await subjectService.GetAllSubjectsAsync();

        if (!subjects.Any())
        {
            Console.WriteLine("No subjects available.");
            return;
        }

        Console.WriteLine("\nAvailable Subjects:");
        foreach (var subject in subjects)
        {
            Console.WriteLine($"- {subject.Name}");
        }
    }

    static async Task ViewSubjectDetails(ISubjectService subjectService)
    {
        Console.Write("\nEnter the subject name: ");
        var name = Console.ReadLine()?.Trim();

        var subject = await subjectService.GetSubjectByNameAsync(name!);

        if (subject == null)
        {
            Console.WriteLine("Subject not found.");
            return;
        }

        Console.WriteLine($"\nDetails for {subject.Name}:");
        Console.WriteLine($"Description: {subject.Description}");
        Console.WriteLine($"Weekly Classes: {subject.NumberOfWeeklyClasses}");

        Console.WriteLine("\nLiterature Used:");
        if (subject.LiteratureTitles == null || !subject.LiteratureTitles.Any())
        {
            Console.WriteLine("- No literature available.");
        }
        else
        {
            foreach (var title in subject.LiteratureTitles)
            {
                Console.WriteLine($"- {title}");
            }
        }
    }
}