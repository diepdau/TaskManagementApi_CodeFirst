using AutoMapper;
using TaskManagementApi.DTOs;
using TaskManagementApi.DTOs.TaskDto;
using TaskManagementApi.Models;

namespace TaskManagementApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Models.Task, TaskCreateDto>().ReverseMap();
            CreateMap<Models.Task, TaskUpdateDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<TaskComment, TaskCommentDto>().ReverseMap();
            CreateMap<TaskLabel, TaskLabelDto>().ReverseMap();
            CreateMap<Label, LabelDto>().ReverseMap();
            CreateMap<Models.Task, TaskDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.TaskLabels.Select(tl => tl.Labels.Name).ToList()));
        }
    }
}