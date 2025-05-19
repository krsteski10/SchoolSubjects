using Microsoft.AspNetCore.Mvc;
using SchoolSubjects.Application.DTOs;
using SchoolSubjects.Application.Interfaces;

namespace SchoolSubjects.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetAll()
        {
            var subjects = await _subjectService.GetAllSubjectsAsync();
            return Ok(subjects);
        }

        [HttpGet("{subjectName}")]
        public async Task<ActionResult<SubjectDto>> GetByName(string subjectName)
        {
            var subject = await _subjectService.GetSubjectByNameAsync(subjectName);
            if (subject == null)
                return NotFound();

            return Ok(subject);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSubjectDto dto)
        {
            var created = await _subjectService.AddSubjectAsync(dto);
            return CreatedAtAction(nameof(GetByName), new { subjectName = created.Name }, created);
        }

        [HttpDelete("{subjectName}")]
        public async Task<IActionResult> Delete(string subjectName)
        {
            var success = await _subjectService.RemoveSubjectAsync(subjectName);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpGet("fromfile")]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetFromFile()
        {
            var subjects = await _subjectService.ReadSubjectsFromFileAsync();
            return Ok(subjects);
        }
    }
}
