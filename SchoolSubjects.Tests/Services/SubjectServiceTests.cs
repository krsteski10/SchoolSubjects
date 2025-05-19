using Xunit;
using Moq;
using SchoolSubjects.Application.Services;
using SchoolSubjects.Domain;
using SchoolSubjects.Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using SchoolSubjects.Application.DTOs;

namespace SchoolSubjects.Tests.Services
{
    public class SubjectServiceTests
    {
        private readonly Mock<ISubjectRepository> _subjectRepoMock;
        private readonly SubjectService _subjectService;
        private readonly IMemoryCache _cache;
        private readonly Mock<IHostEnvironment> _envMock;

        public SubjectServiceTests()
        {
            _subjectRepoMock = new Mock<ISubjectRepository>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _envMock = new Mock<IHostEnvironment>();

            _subjectService = new SubjectService(
                _subjectRepoMock.Object,
                _cache,
                _envMock.Object
            );
        }

        [Fact]
        public async Task GetAllSubjectsAsync_ReturnsSubjects()
        {
            // Arrange
            var fakeSubjects = new List<Subject>
            {
                new Subject { Id = 1, Name = "Math", Description = "Mathematics", NumberOfWeeklyClasses = 5 },
                new Subject { Id = 2, Name = "History", Description = "History Class", NumberOfWeeklyClasses = 3 }
            };
            _subjectRepoMock.Setup(repo => repo.GetAllAsync())
                            .ReturnsAsync(fakeSubjects);

            // Act
            var result = await _subjectService.GetAllSubjectsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetSubjectByNameAsync_ReturnsNull_IfNotFound()
        {
            // Arrange
            _subjectRepoMock.Setup(repo => repo.GetByNameAsync("Physics"))
                            .ReturnsAsync((Subject?)null);

            // Act
            var result = await _subjectService.GetSubjectByNameAsync("Physics");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetSubjectByNameAsync_ReturnsSubject_IfExists()
        {
            // Arrange
            var fakeSubject = new Subject
            {
                Name = "Math",
                Description = "Basic Math",
                NumberOfWeeklyClasses = 3,
                LiteratureUsed = new List<Literature> { new() { Title = "Math Book" } }
            };

            _subjectRepoMock
                .Setup(r => r.GetByNameAsync("Math"))
                .ReturnsAsync(fakeSubject);

            // Act
            var result = await _subjectService.GetSubjectByNameAsync("Math");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Math", result.Name);
        }

        [Fact]
        public async Task GetAllSubjectsAsync_ShouldReturnEmptyList_WhenNoSubjectsExist()
        {
            // Arrange
            _subjectRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Subject>());

            // Act
            var result = await _subjectService.GetAllSubjectsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        //Reading and parsing a JSON file into subject DTO
        public async Task ReadSubjectsFromFileAsync_ShouldReturnList_WhenFileIsValid()
        {
            // Arrange
            var json = @"[{ ""name"": ""Physics"", ""description"": ""Physics desc"", ""numberOfWeeklyClasses"": 4, ""literatureUsed"": [{ ""title"": ""Physics Book"" }] }]";

            var envMock = new Mock<IHostEnvironment>();
            var testFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "subjects.json");

            var dataDirectory = Path.GetDirectoryName(testFilePath);
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            await File.WriteAllTextAsync(testFilePath, json);

            envMock.Setup(e => e.ContentRootPath).Returns(AppContext.BaseDirectory);

            var repoMock = new Mock<ISubjectRepository>();
            var cache = new MemoryCache(new MemoryCacheOptions());

            var service = new SubjectService(repoMock.Object, cache, envMock.Object);

            // Act
            var result = await service.ReadSubjectsFromFileAsync();
            var subject = result.FirstOrDefault();

            // Assert
            Assert.NotNull(subject);
            Assert.Equal("Physics", subject.Name);

            // Clean up the file after the test
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
        }

    }
}
