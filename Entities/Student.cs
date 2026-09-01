using System.ComponentModel.DataAnnotations;

namespace CourseManagementApi.Entities;

public class Student
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    // Navigation Properties
    public StudentDetail? StudentDetail { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}