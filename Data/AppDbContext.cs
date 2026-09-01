using CourseManagementApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CourseManagementApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<StudentDetail> StudentDetails => Set<StudentDetail>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Composite Key for Junction Table (Many-to-Many)
        modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.CourseId });

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // 2. One-to-One Configuration
        modelBuilder.Entity<Student>()
            .HasOne(s => s.StudentDetail)
            .WithOne(sd => sd.Student)
            .HasForeignKey<StudentDetail>(sd => sd.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}