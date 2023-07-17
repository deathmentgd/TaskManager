using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TaskManager.Models.Task
{
    /// <summary>
    /// Модель для создания новой задачи
    /// </summary>
    public class CreateTaskDto: BindableBase
    {
        /// <summary>
        /// Название задачи
        /// </summary>
        public string Name 
        {
            get { return GetValue<string>(nameof(Name)); }
            set { SetValue(value, nameof(Name)); }
        }
        /// <summary>
        /// Описание задачи
        /// </summary>
        public string Description 
        {
            get { return GetValue<string>(nameof(Description)); }
            set { SetValue(value, nameof(Description)); }
        }
    }
}
