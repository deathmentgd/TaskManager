using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Models.TaskStatus;

namespace TaskManager.Services.Repositories.TaskStatusRepository
{
    /// <summary>
    /// Предоставляет функционал для работы с ЗАДАЧАМИ
    /// </summary>
    public interface ITaskStatusRepository
    {
        /// <summary>
        /// Возвращает список всех задач
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TaskStatusDto>> GetTaskStatusesAsync();       
    }
}
