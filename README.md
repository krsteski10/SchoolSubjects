**Documentation**

Overview:
•	This system manages school subjects and their associated literature.
•	It uses a layered architecture for maintainability and scalability.
•	Core technologies: .NET, EF Core, and a Console + Web API presentation (Swagger integrated).


Defining the Subject and Literature classes, which will represent the core entity in our system.
-Setting up the Database. Entity Framework Core used to interact with the database.
1.	Creating a SchoolContext class to manage the database connection.
2.  Installing all the necessary packages to interact with EF Core: Microsoft. *EntityFramework *Core, *Tools, *Configuration etc.
3.  Setting up appsettings file to keep the Connection string (this way we can set up different environments in the future).
4.	Configuring the database schema for the Subject and Literature entities.
5.	Using migrations to create the database.

-Implementing the Core Functionality in the Console app.
1.	List Subjects: Allow the user to view all available subjects.
2.	View Subject Details: Provide detailed information about a selected subject.
3.	Extendability: Design the system to allow adding new subjects from external sources (APIs or other databases).

-Implementing Layered Architecture which is suitable for smaller projects or when simplicity is a priority.
Can be evolved later on into Clean Architecture by introducing dependency inversion and abstractions.

**1.  Domain Layer**
•	Type: Class Library
•	Responsibilities: Core entities (Subject, Literature).
•	Dependencies: None (completely independent).

**2.	Application Layer:**
•	Type: Class Library
•	Responsibilities: Business logic, Services (e.g., SubjectService), and use cases.
•	Dependencies: References the Domain layer and Infrastructure layer.

**3.	Infrastructure Layer:**
•	Type: Class Library
•	Responsibilities: Repositories and Interfaces, EF Core DbContext (SchoolContext), migrations, and data access.
•	Dependencies: References the Domain layer.
•	Migrations are moved to this layer, and commands like Add-Migration and Update-Database are used to manage schema changes.
	Run commands like Add-Migration and Update-Database. Commonly used commands:
	SchoolSubjects.Presentation -Project SchoolSubjects.Infrastructure)
	(or dotnet ef database update --project ./SchoolSubjects.Infrastructure --startup-project ./SchoolSubjects.ConsoleApp)

**4.	Presentation Layer:**
•	Type: Console App
•	Responsibilities: Handles user input/output and interacts with the Application layer.
•	Dependencies: References the Infrastructure layer.

**5.  API layer:**
•	Type: Web API
•	Responsibilities: Exposes the Subject service via API and interacts with the Application layer.
•	Dependencies: References the Application layer. Registered dependencies using Dependency Injection in the Program.cs.

**6.  Unit Tests:**
•   Type: XUnit Tests Project
•	Responsibilities: Unit tests focused on testing a single unit of functionality (e.g.,SubjectService).
•   Dependencies: References the Domain and Application layer.
•   Tools: xUnit for test cases and Moq for mocking dependencies.

**7.  Caching and File Handling Enhancements**
•	Caching with IMemoryCache. The system implements memory caching to optimize the retrieval of subjects from the database. This technique reduces the need for repeated database queries, improving performance when retrieving frequently accessed data.

Caching in the SubjectService:
The GetAllSubjectsAsync method checks if subjects are already cached. If they are, the system retrieves them from the cache, otherwise, it fetches the data from the database and caches it for future use.

Example caching implementation in SubjectService:
-------------------------------------------------
__cache.Set(CacheKey, dtoList, TimeSpan.FromMinutes(5));_

Reading Data from a File (JSON)
The system allows subjects to be loaded from a JSON file. This feature supports scenarios where subject data may come from external files, enabling flexibility in data import.

Reading Subjects from File:
The method ReadSubjectsFromFileAsync reads subject data from a JSON file and deserializes it into a list of Subject objects. This data is then returned to the service layer.

Example file reading in SubjectService:
---------------------------------------
_public async Task<List<Subject>> ReadSubjectsFromFileAsync()
{
    var filePath = Path.Combine(_env.ContentRootPath, "subjects.json");
    var jsonData = await File.ReadAllTextAsync(filePath);
    var subjects = JsonSerializer.Deserialize<List<Subject>>(jsonData);
    return subjects;
}_

Important: The subjects.json file must be present in the root directory of the application or in a directory specified by the environment. This file should contain data in the following format:

_[
    {
        "name": "Math",
        "description": "Mathematics subject",
        "numberOfWeeklyClasses": 5,
        "literatureUsed": [
            {
                "title": "Math Book"
            }
        ]
    },
    {
        "name": "History",
        "description": "History subject",
        "numberOfWeeklyClasses": 3,
        "literatureUsed": [
            {
                "title": "History Book"
            }
        ]
    }
]
_

[Presentation Layer (Console + API)] 
    ↓
[Application Layer (Services)]
    ↓
[Domain Layer (Entities)]
    ↓
[Infrastructure Layer (Database)]

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
**SOLID Principles**

•	Single Responsibility Principle (SRP): The SubjectService class is focused on handling subject-related operations.
•	Open/Closed Principle (OCP): The system can be easily extended to include new data sources or repositories, following the OCP.
•	Liskov Substitution Principle (LSP): The use of interfaces ensures that different implementations can be substituted without breaking the code.
•	Interface Segregation Principle (ISP): The ISubjectRepository interface is focused on repository operations.
•	Dependency Inversion Principle (DIP): The SubjectService depends on the ISubjectService abstraction rather than a concrete implementation, which adheres to DIP.

**Data-Driven Design and Extensibility**

•	The system supports dynamic loading of subjects from external sources, such as JSON files or APIs.
•	New entities (e.g., Teacher, Student) can be easily added by creating new classes in the Domain layer.
•	External data sources, like APIs or CSV files, can be integrated with minimal effort by adding new repository classes.
•	Presentation layers can be expanded to include web frontends or mobile applications as the project evolves.
