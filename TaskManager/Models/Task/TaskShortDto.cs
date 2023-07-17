using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models.Task
{
    public class TaskShortDto
    {
        /// <summary>
        /// Идентификатор в БД
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Номер задачи (КЛЮЧ ПРОЕКТА-НОМЕР ЗАДАЧИ)
        /// </summary>
        public string Identity { get; set; }
        /// <summary>
        /// Название задачи
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Статус задачи
        /// </summary>
        public short StatusId { get; set; }
    }
}
