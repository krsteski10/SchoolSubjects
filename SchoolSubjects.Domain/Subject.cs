﻿namespace SchoolSubjects.Domain;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int NumberOfWeeklyClasses { get; set; }
    public List<Literature> LiteratureUsed { get; set; } = new();
}
