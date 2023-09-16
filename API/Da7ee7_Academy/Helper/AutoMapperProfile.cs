using AutoMapper;
using Da7ee7_Academy.DTOs;
using Da7ee7_Academy.Entities;

namespace Da7ee7_Academy.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<Section, SectionDto>();
            CreateMap<SectionItem, SectionItemDto>();
        }
    }
}
