using SchoolSubjects.Domain;

namespace SchoolSubjects.Infrastructure.Interfaces;

public interface ISubjectRepository
{
    Task<IEnumerable<Subject>> GetAllAsync();
    Task<Subject?> GetByNameAsync(string name);
    Task AddAsync(Subject subject);
    Task RemoveAsync(Subject subject);
    Task SaveChangesAsync();
}
