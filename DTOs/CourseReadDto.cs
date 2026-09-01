namespace CourseManagementApi.DTOs;

public class CourseReadDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
}