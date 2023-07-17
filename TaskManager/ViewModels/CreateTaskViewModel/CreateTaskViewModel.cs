using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using Microsoft.VisualBasic.Logging;
using System.ComponentModel;
using System.Threading.Tasks;
using TaskManager.Models.Task;
using TaskManager.Services.Repositories.TaskRepository;

namespace TaskManager.ViewModels.CreateTaskViewModel
{
    [GenerateViewModel(ImplementISupportServices=true)]
    public partial class CreateTaskViewModel:IDataErrorInfo
    {
        protected virtual ICurrentDialogService CurrentDialogService => GetService<ICurrentDialogService>();

        /// <summary>
        /// Репозиторий для работы с задачами
        /// </summary>
        private readonly ITaskRepository _taskRepository;        

        /// <summary>
        /// Название задачи
        /// </summary>
        [GenerateProperty]
        string _Name;
        /// <summary>
        /// Описание задачи
        /// </summary>
        [GenerateProperty]
        string _Description;

        /// <summary>
        /// Текст ошибки валидации
        /// </summary>
        string error;
        /// <summary>
        /// Реализация IDataErrorInfo
        /// </summary>
        string IDataErrorInfo.Error { get { return error; } }
        /// <summary>
        /// Реализация IDataErrorInfo
        /// </summary>
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name":
                        return ValidateStringIsNotEmpty(Name) ? string.Empty : error;
                    case "Description":
                        return ValidateStringIsNotEmpty(Description) ? string.Empty : error;
                }
                return string.Empty;
            }
        }

        //-------------------------------------------------------------------------------------------

        public CreateTaskViewModel(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        /// <summary>
        /// Создать задачу
        /// </summary>
        [GenerateCommand]
        public async Task CreateTask()
        {            
            await _taskRepository.CreateTaskAsync(new CreateTaskDto { Name = Name, Description = Description });
            CurrentDialogService.Close(MessageResult.OK);
        }        

        /// <summary>
        /// Установить ошибку если значение пустое
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool ValidateStringIsNotEmpty(string name)
        {
            bool isValid = !string.IsNullOrEmpty(name?.Trim());
            return SetError(isValid, "Значение не может быть пустым");
        }

        /// <summary>
        /// Установить текст ошибки валидации
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="errorString"></param>
        /// <returns></returns>
        bool SetError(bool isValid, string errorString)
        {
            error = isValid ? string.Empty : errorString;           
            return isValid;
        }

        /// <summary>
        /// Закрыть диалог со значением Cancel
        /// </summary>
        [GenerateCommand]
        public void CloseDialog() 
        {
            CurrentDialogService.Close(MessageResult.Cancel);
        }
    }
}
