using CourseManagementApi.Data;
using CourseManagementApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/Courses
    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCourseById), new { id = course.Id }, course);
    }

    // GET: api/Courses/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Course>> GetCourseById(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        return course != null ? Ok(course) : NotFound();
    }

    // PUT: api/Courses/increase-credits (EF Core 7+ ExecuteUpdate)
    [HttpPut("increase-credits")]
    public async Task<IActionResult> IncreaseCreditsForSmallCourses([FromQuery] int currentCredits, [FromQuery] int boostBy)
    {
        // Direct bulk update on database level without loading entities into DbContext memory
        int rowsAffected = await _context.Courses
            .Where(c => c.Credits <= currentCredits)
            .ExecuteUpdateAsync(s => s.SetProperty(c => c.Credits, c => c.Credits + boostBy));

        return Ok(new { Message = $"Updated credits for {rowsAffected} courses." });
    }

    // DELETE: api/Courses/bulk-delete (EF Core 7+ ExecuteDelete)
    [HttpDelete("bulk-delete")]
    public async Task<IActionResult> DeleteEmptyCourses()
    {
        // Direct bulk deletion on database level
        int rowsDeleted = await _context.Courses
            .Where(c => !c.Enrollments.Any())
            .ExecuteDeleteAsync();

        return Ok(new { Message = $"Deleted {rowsDeleted} unused courses." });
    }
}
//edited controller to include bulk update and delete operations using EF Core 7+ features.