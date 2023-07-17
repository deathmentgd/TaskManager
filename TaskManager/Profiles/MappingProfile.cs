using AutoMapper;
using TaskManager.Models.Task;
using TaskManager.Models.TaskStatus;

namespace TaskManager.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateTaskDto, TaskEntity>();
        CreateMap<TaskEntity, TaskShortDto>()
         .ForMember(x => x.Identity, opt => opt.MapFrom(o => $"{(o.Project != null ? o.Project.Name : "ДЕМО")}-{o.Number}"));
        CreateMap<TaskEntity, TaskDto>()
         .ForMember(x => x.Identity, opt => opt.MapFrom(o => $"{(o.Project != null ? o.Project.Name : "ДЕМО")}-{o.Number}"));
        CreateMap<TaskDto, TaskEntity>();
        CreateMap<TaskDto, UpdateTaskDto>();
        CreateMap<UpdateTaskDto, TaskEntity>();

        CreateMap<TaskStatusEntity, TaskStatusDto>();
    }
}