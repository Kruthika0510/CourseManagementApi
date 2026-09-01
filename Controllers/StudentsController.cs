using AutoMapper;
using CourseManagementApi.Data;
using CourseManagementApi.DTOs;
using CourseManagementApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    // Constructor Injection from DI Container
    public StudentsController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Students (AsNoTracking + Eager Loading)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentReadDto>>> GetStudents()
    {
        var students = await _context.Students
            .AsNoTracking() // Performance optimization for read queries
            .Include(s => s.StudentDetail) // 1-to-1 Eager Loading
            .Include(s => s.Enrollments) // Many-to-Many Eager Loading
                .ThenInclude(e => e.Course) // Nested Eager Loading
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<StudentReadDto>>(students));
    }

    // GET: api/Students/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentReadDto>> GetStudent(int id)
    {
        var student = await _context.Students
            .AsNoTracking()
            .Include(s => s.StudentDetail)
            .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
        {
            // Returning ProblemDetails on 404
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Student Not Found",
                Detail = $"No student record found with ID {id}."
            });
        }

        return Ok(_mapper.Map<StudentReadDto>(student));
    }

    // POST: api/Students
    [HttpPost]
    public async Task<ActionResult<StudentReadDto>> CreateStudent([FromBody] StudentCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var studentEntity = _mapper.Map<Student>(dto);

        await _context.Students.AddAsync(studentEntity);
        await _context.SaveChangesAsync();

        var readDto = _mapper.Map<StudentReadDto>(studentEntity);

        return CreatedAtAction(nameof(GetStudent), new { id = studentEntity.Id }, readDto);
    }

    // POST: api/Students/1/enroll/1
    [HttpPost("{studentId:int}/enroll/{courseId:int}")]
    public async Task<IActionResult> EnrollStudent(int studentId, int courseId)
    {
        var student = await _context.Students.FindAsync(studentId);
        var course = await _context.Courses.FindAsync(courseId);

        if (student == null || course == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource Not Found",
                Detail = "Either the specified student or course does not exist."
            });
        }

        // Check for existing enrollment using composite primary key lookup
        var existingEnrollment = await _context.Enrollments.FindAsync(studentId, courseId);
        if (existingEnrollment != null)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Duplicate Enrollment",
                Detail = $"Student ID {studentId} is already enrolled in Course ID {courseId}."
            });
        }

        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrolledOn = DateTime.UtcNow
        };

        await _context.Enrollments.AddAsync(enrollment);
        await _context.SaveChangesAsync();

        return Ok(new { Message = $"Student ID {studentId} successfully enrolled in Course ID {courseId}." });
    }

    // DELETE: api/Students/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}