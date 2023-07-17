using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Models.Project;
using TaskManager.Models.Task;

namespace TaskManager.Services.Repositories.TaskRepository
{
    public class TaskRepository : ITaskRepository
    {
        private List<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
        private long TaskIdSeq { get; set; } = 0;
        private long TaskNumberSeq { get; set; } = 0;

        private readonly IMapper _mapper;

        public TaskRepository(IMapper mapper)
        {
            _mapper = mapper;
            var task = new TaskEntity
            {
                Created = DateTime.Now,
                Description = "Что-то делаем",
                Id = ++TaskIdSeq,
                Name = "Супер задача",
                Number = ++TaskNumberSeq,
                ProjectId = 1,
            };
            Tasks.Add(task);
            task = new TaskEntity
            {
                Created = DateTime.Now,
                Description = "ЕЩЕ Что-то делаем",
                Id = ++TaskIdSeq,
                Name = "Супер пупер задача",
                Number = ++TaskNumberSeq,
                ProjectId = 1,
            };
            Tasks.Add(task);
        }

        public async Task<long> CreateTaskAsync(CreateTaskDto taskDto)
        {
            var entity = new TaskEntity();
            _mapper.Map(taskDto, entity);
            entity.StatusId = 0;
            entity.ProjectId = 1;
            entity.Id = ++TaskIdSeq;
            entity.Number = ++TaskNumberSeq;
            entity.Created = DateTime.Now;
            Tasks.Add(entity);

            return entity.Id;
        }

        public async Task DeleteTaskAsync(long taskId)
        {
            var item = Tasks.FirstOrDefault(x=>x.Id == taskId);

            if (item != null)
            {
                item.IsDeleted = true;
            }
            else
            {
                // здесь можно выбросить ошибку, если запись уже удалена или такого ИД не существует
            }
        }

        public async Task<TaskDto> GetTaskAsync(long taskId)
        {
            var entity = Tasks.FirstOrDefault(x => x.Id == taskId && !x.IsDeleted);
            return _mapper.Map<TaskDto>(entity);
        }

        public async Task<IEnumerable<TaskShortDto>> GetTasksAsync(int? projectId = null, short? statusId = null)
        {
            var entities = Tasks.Where(x => (!statusId.HasValue || x.StatusId == statusId) && !x.IsDeleted).ToList();
            return _mapper.Map<IEnumerable<TaskShortDto>>(entities);
        }

        public async Task UpdateTaskAsync(long id, UpdateTaskDto taskDto)
        {
            var entity = Tasks.FirstOrDefault(x=>x.Id == id);
            if (entity != null)
            {
                _mapper.Map(taskDto, entity);
            }
        }

        public async Task<TaskDto> SetTaskStatusAsync(long taskId, short statusId)
        {
            var entity = Tasks.FirstOrDefault(x => x.Id == taskId);
            if (entity != null)
            {
                entity.StatusId = statusId;
                if (statusId == 3)
                {
                    entity.Finished = DateTime.Now;
                }
            }
            return _mapper.Map<TaskDto>(entity);
        }
    }
}
