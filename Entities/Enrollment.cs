using System.ComponentModel.DataAnnotations;

namespace CourseManagementApi.Entities;

public class Enrollment
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public DateTime EnrolledOn { get; set; } = DateTime.UtcNow;

    [MaxLength(2)]
    public string? Grade { get; set; }
}