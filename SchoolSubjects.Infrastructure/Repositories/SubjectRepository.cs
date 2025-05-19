using Microsoft.EntityFrameworkCore;
using SchoolSubjects.Domain;
using SchoolSubjects.Infrastructure.Interfaces;

namespace SchoolSubjects.Infrastructure.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly SchoolContext _context;

    public SubjectRepository(SchoolContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Subject>> GetAllAsync()
    {
        return await _context.Subjects.Include(s => s.LiteratureUsed).ToListAsync();
    }

    public async Task<Subject?> GetByNameAsync(string name)
    {
        return await _context.Subjects
            .Include(s => s.LiteratureUsed)
            .FirstOrDefaultAsync(s => s.Name == name);
    }

    public async Task AddAsync(Subject subject)
    {
        await _context.Subjects.AddAsync(subject);
    }

    public async Task RemoveAsync(Subject subject)
    {
        _context.Subjects.Remove(subject);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
