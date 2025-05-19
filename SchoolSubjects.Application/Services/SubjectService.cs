using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using SchoolSubjects.Application.DTOs;
using SchoolSubjects.Application.Interfaces;
using SchoolSubjects.Domain;
using SchoolSubjects.Infrastructure.Interfaces;
using System.Text.Json;

namespace SchoolSubjects.Application.Services;

public class SubjectService : ISubjectService
{
    private readonly ISubjectRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly IHostEnvironment _env;

    private const string CacheKey = "subjects_cache";
    public SubjectService(ISubjectRepository repository, IMemoryCache cache, IHostEnvironment env)
    {
        _repository = repository;
        _cache = cache;
        _env = env;
    }

    // Use case: List all subjects available in the system.
    public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync()
    {
        // Return from cache if available
        if (_cache.TryGetValue(CacheKey, out IEnumerable<SubjectDto> cachedSubjects))
        {
            return cachedSubjects;
        }

        var subjects = await _repository.GetAllAsync();

        var dtoList = subjects.Select(s => new SubjectDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            NumberOfWeeklyClasses = s.NumberOfWeeklyClasses,
            LiteratureTitles = s.LiteratureUsed?.Select(l => l.Title).ToList() ?? new()
        }).ToList();

        // Set cache for 5 minutes
        _cache.Set(CacheKey, dtoList, TimeSpan.FromMinutes(5));

        return dtoList;
    }

    public async Task<IEnumerable<SubjectDto>> ReadSubjectsFromFileAsync()
    {
        var filePath = Path.Combine(_env.ContentRootPath, "Data", "subjects.json");

        if (!File.Exists(filePath))
            return new List<SubjectDto>();

        var json = await File.ReadAllTextAsync(filePath);

        var subjects = System.Text.Json.JsonSerializer.Deserialize<List<Subject>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return subjects?.Select(s => new SubjectDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            NumberOfWeeklyClasses = s.NumberOfWeeklyClasses,
            LiteratureTitles = s.LiteratureUsed?.Select(l => l.Title).ToList() ?? new()
        }) ?? new List<SubjectDto>();
    }

    public async Task<SubjectDto?> GetSubjectByNameAsync(string name)
    {
        var subject = await _repository.GetByNameAsync(name);
        if (subject == null) return null;

        return new SubjectDto
        {
            Id = subject.Id,
            Name = subject.Name,
            Description = subject.Description,
            NumberOfWeeklyClasses = subject.NumberOfWeeklyClasses,
            LiteratureTitles = subject.LiteratureUsed?.Select(l => l.Title).ToList() ?? new()
        };
    }

    public async Task<SubjectDto> AddSubjectAsync(CreateSubjectDto dto)
    {
        var subject = new Subject
        {
            Name = dto.Name,
            Description = dto.Description,
            NumberOfWeeklyClasses = dto.NumberOfWeeklyClasses,
            LiteratureUsed = dto.LiteratureTitles?.Select(t => new Literature { Title = t }).ToList() ?? new()
        };

        await _repository.AddAsync(subject);
        await _repository.SaveChangesAsync();

        return new SubjectDto
        {
            Id = subject.Id,
            Name = subject.Name,
            Description = subject.Description,
            NumberOfWeeklyClasses = subject.NumberOfWeeklyClasses,
            LiteratureTitles = subject.LiteratureUsed.Select(l => l.Title).ToList()
        };
    }

    public async Task<bool> RemoveSubjectAsync(string name)
    {
        var subject = await _repository.GetByNameAsync(name);
        if (subject == null) return false;

        await _repository.RemoveAsync(subject);
        await _repository.SaveChangesAsync();
        return true;
    }
}
