using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Models.Task;
using TaskManager.Models.TaskStatus;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TaskManager.Services.Repositories.TaskStatusRepository
{
    public class TaskStatusRepository : ITaskStatusRepository
    {
        private readonly IMapper _mapper;
        private List<TaskStatusEntity> TaskStatuses { get; set; }


        public TaskStatusRepository(IMapper mapper) 
        {
            _mapper = mapper;

            TaskStatuses = new List<TaskStatusEntity>
            {
                new TaskStatusEntity
                {
                    Id= 0,
                    Name = "Создано"
                },
                new TaskStatusEntity
                {
                    Id= 1,
                    Name = "В работе"
                },
                new TaskStatusEntity
                {
                    Id= 2,
                    Name = "Приостановлено"
                },
                new TaskStatusEntity
                {
                    Id= 3,
                    Name = "Завершено"
                },
            };
        }
        public async Task<IEnumerable<TaskStatusDto>> GetTaskStatusesAsync()
        {
            var entities = TaskStatuses;
            return _mapper.Map<IEnumerable<TaskStatusDto>>(entities);
        }
    }
}
