using DevExpress.Mvvm;
using System;

namespace TaskManager.Models.Task
{
    /// <summary>
    /// Модель просмотра и редактироваиня задачи
    /// </summary>
    public class TaskDto:BindableBase
    {   
        /// <summary>
        /// Идентификатор в БД
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Идентификатор проекта (как заглушка, если надо разделять задачи по проектам)
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Номер задачи (КЛЮЧ ПРОЕКТА-НОМЕР ЗАДАЧИ)
        /// </summary>
        public string Identity { get; set; }
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
        public short StatusId 
        {
            get { return GetValue<short>(nameof(StatusId)); }
            set { SetValue(value, nameof(StatusId)); }
        }

    }
}
