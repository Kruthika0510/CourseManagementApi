namespace CourseManagementApi.DTOs;

public class StudentReadDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Address { get; set; }
    public List<CourseReadDto> Courses { get; set; } = new();
}