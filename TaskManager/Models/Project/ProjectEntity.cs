using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models.Project
{
    public class ProjectEntity
    {
        /// <summary>
        /// Модель сущности ПРОЕКТ в БД
        /// </summary>
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
