using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Models.Task;

namespace TaskManager.Services.Repositories.TaskRepository
{
    /// <summary>
    /// Предоставляет функционал для работы с ЗАДАЧАМИ
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Возвращает список всех задач
        /// </summary>
        /// <param name="projectId">ИД проекта, если не указан, то все</param>
        /// <param name="statusId">ИД статуса задач, если не указан, то все</param>
        /// <returns></returns>
        public Task<IEnumerable<TaskShortDto>> GetTasksAsync(int? projectId = null, short? statusId = null);
        /// <summary>
        /// Возвращает задачу по ИД
        /// </summary>
        /// <param name="taskId">ИД задачи</param>
        /// <returns></returns>
        public Task<TaskDto> GetTaskAsync(long taskId);
        /// <summary>
        /// Создает задачу
        /// </summary>
        /// <returns>Возвращает ИД созданной задачи</returns>
        public Task<long> CreateTaskAsync(CreateTaskDto taskDto);
        /// <summary>
        /// Обновляет задачу
        /// </summary>
        /// <param name="taskDto"></param>
        /// <returns></returns>
        public Task UpdateTaskAsync(long id, UpdateTaskDto taskDto);
        /// <summary>
        /// Удаляет указанную задачу
        /// </summary>
        /// <param name="taskId">ИД задачи</param>
        public Task DeleteTaskAsync(long taskId);

        public Task<TaskDto> SetTaskStatusAsync(long taskId, short statusId);
    }
}
