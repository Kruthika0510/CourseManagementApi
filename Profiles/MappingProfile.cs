using AutoMapper;
using CourseManagementApi.DTOs;
using CourseManagementApi.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CourseManagementApi.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Domain Entity -> DTO Mappings
        CreateMap<Student, StudentReadDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.StudentDetail != null ? src.StudentDetail.Address : null))
            .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Enrollments.Select(e => e.Course)));

        CreateMap<Course, CourseReadDto>();

        // DTO -> Domain Entity Mapping
        CreateMap<StudentCreateDto, Student>()
            .ForMember(dest => dest.StudentDetail, opt => opt.MapFrom(src => new StudentDetail
            {
                Address = src.Address,
                PhoneNumber = src.PhoneNumber
            }));
    }
}