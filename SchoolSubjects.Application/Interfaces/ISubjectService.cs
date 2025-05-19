using SchoolSubjects.Application.DTOs;

namespace SchoolSubjects.Application.Interfaces;

public interface ISubjectService
{
    Task<IEnumerable<SubjectDto>> ReadSubjectsFromFileAsync();
    Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync();
    Task<SubjectDto?> GetSubjectByNameAsync(string name);
    Task<SubjectDto> AddSubjectAsync(CreateSubjectDto dto);
    Task<bool> RemoveSubjectAsync(string name);
}