using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models.Task
{
    public class UpdateTaskDto
    {
        /// <summary>
        /// Название задачи
        /// </summary>
        [Required]
        public virtual string Name { get; set; }
        /// <summary>
        /// Описание задачи
        /// </summary>
        [Required]
        public virtual string Description { get; set; }
    }
}
