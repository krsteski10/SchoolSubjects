namespace SchoolSubjects.Application.DTOs;

public class CreateSubjectDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int NumberOfWeeklyClasses { get; set; }
    public List<string>? LiteratureTitles { get; set; }
}
