using System.ComponentModel.DataAnnotations;

namespace CourseManagementApi.DTOs;

public class StudentCreateDto
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}