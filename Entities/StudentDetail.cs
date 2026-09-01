using System.ComponentModel.DataAnnotations;

namespace CourseManagementApi.Entities;

public class StudentDetail
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;

    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
}