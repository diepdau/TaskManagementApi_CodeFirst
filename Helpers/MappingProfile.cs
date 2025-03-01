using AutoMapper;
using TaskManagementApi.DTOs;
using TaskManagementApi.Models;

namespace TaskManagementApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Models.Task, TaskDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<TaskComment, TaskCommentDto>().ReverseMap();
            CreateMap<TaskLabel, TaskLabelDto>().ReverseMap();
            CreateMap<Label, LabelDto>().ReverseMap();
        }
    }
}