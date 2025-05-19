namespace SchoolSubjects.Application.DTOs;

public class SubjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int NumberOfWeeklyClasses { get; set; }
    public List<string> LiteratureTitles { get; set; } 
}
