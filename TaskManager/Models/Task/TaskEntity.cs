using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Models.Project;

namespace TaskManager.Models.Task
{
    /// <summary>
    /// Модель сущности ЗАДАЧА в БД
    /// </summary>
    internal class TaskEntity
    {
        /// <summary>
        /// Идентификатор в БД
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Идентификатор проекта (как заглушка, если надо разделять задачи по проектам)
        /// </summary>
        public int ProjectId { get; set; }
        public ProjectEntity Project { get; set; }
        public long Number { get; set; }
        /// <summary>
        /// Название задачи
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание задачи
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Дата завершения
        /// </summary>
        public DateTime? Finished { get; set; }
        /// <summary>
        /// Статус задачи
        /// </summary>
        public short StatusId { get; set; } = 0;
        /// <summary>
        /// Флаг логического удаления
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
