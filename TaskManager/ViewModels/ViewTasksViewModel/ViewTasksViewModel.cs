using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskManager.Models.Task;
using TaskManager.Models.TaskStatus;
using TaskManager.Services.Repositories.TaskRepository;
using TaskManager.Services.Repositories.TaskStatusRepository;
using TaskManager.Views.CreateTaskView;
using TaskManager.Views.ViewEditTaskView;

namespace TaskManager.ViewModels.ViewTasksViewModel
{
    [GenerateViewModel(ImplementISupportServices = true, ImplementISupportParentViewModel = true)]
    public partial class ViewTasksViewModel
    {
        protected IDialogService DialogService => GetService<IDialogService>();
        protected IDocumentManagerService DocumentManagerService => GetService<IDocumentManagerService>();
        protected INavigationService NavigationService => GetService<INavigationService>();
        protected IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();

        /// <summary>
        /// Репозиторий для получения списка задач
        /// </summary>
        private readonly ITaskRepository _taskRepository;
        /// <summary>
        /// Репозиторий для получения списка статусов задачи
        /// </summary>
        private readonly ITaskStatusRepository _taskStatusRepository;

        /// <summary>
        /// Список задач
        /// </summary>
        [GenerateProperty]
        ObservableCollection<TaskShortDto> _Tasks;
        /// <summary>
        /// Выделенная задача
        /// </summary>
        [GenerateProperty]
        TaskShortDto _CurrentTask = null;
        /// <summary>
        /// Справочник статусов задачи. Используем на форме в LookupEdit
        /// </summary>
        [GenerateProperty]
        List<TaskStatusDto> _TaskStatuses = null;

        /// <summary>
        /// Флаг отображения в режиме таблицы
        /// </summary>
        [GenerateProperty]
        bool _IsTableViewChecked = true;
        //----------------------------------------------------------------------------------------------------------

        public ViewTasksViewModel(ITaskRepository taskRepository, ITaskStatusRepository taskStatusRepository)
        {
            _taskRepository = taskRepository;
            _taskStatusRepository = taskStatusRepository;
        }

        /// <summary>
        /// После загрузки представления обновим данные
        /// </summary>
        [GenerateCommand]
        async Task ViewLoadedAsync()
        {
            await RefreshTasksListAsync();
            await GetTaskStasusesAsync();
        }

        /// <summary>
        /// Получить список задач
        /// </summary>
        /// <returns></returns>
        [GenerateCommand]
        async Task RefreshTasksListAsync()
        {
            Tasks = new ObservableCollection<TaskShortDto>(await _taskRepository.GetTasksAsync());
        }

        /// <summary>
        /// Получить справочник статусов задачи
        /// </summary>
        /// <returns></returns>
        async Task GetTaskStasusesAsync()
        {
            TaskStatuses = (await _taskStatusRepository.GetTaskStatusesAsync()).ToList();
        }

        /// <summary>
        /// Показать диалог добавления новой задачи
        /// </summary>
        /// <returns></returns>
        [GenerateCommand]
        async Task CreateTaskAsync()
        {
            if (CanCreateTaskAsync())
            {
                var result = DialogService.ShowDialog(MessageButton.OKCancel, title: "Новая задача", documentType: typeof(CreateTaskView).ToString(), viewModel: null);
                await RefreshTasksListAsync();
            }
        }

        /// <summary>
        /// Логика доступности действия создания
        /// </summary>
        /// <returns></returns>
        bool CanCreateTaskAsync()
        {
            // Можно провести проверку прав пользователя
            return true;
        }

        /// <summary>
        /// Показать информацию по задаче
        /// </summary>
        [GenerateCommand]
        public void ShowDescription()
        {
            if (CurrentTask != null)
            {
                var document = DocumentManagerService.CreateDocument(typeof(ViewEditTaskView).ToString(), CurrentTask.Id, this);
                document.DestroyOnClose = true;                
                document.Show();
            }
        }

        /// <summary>
        /// Удалить задачу
        /// </summary>
        /// <returns></returns>
        [GenerateCommand]
        async Task DeleteTaskAsync()
        {
            if (CanDeleteTaskAsync())
            {
                if (MessageBoxService.ShowMessage("Удалить?", "Необходимо подтверждение", MessageButton.YesNo, MessageIcon.Question, MessageResult.No) == MessageResult.Yes)
                {
                    await _taskRepository.DeleteTaskAsync(CurrentTask.Id);
                    await RefreshTasksListAsync();
                }
            }
        }

        /// <summary>
        /// Логика доступности действия удаления
        /// </summary>
        /// <returns></returns>
        bool CanDeleteTaskAsync()
        {
            // Можно провести проверку прав пользователя
            return CurrentTask != null;
        }
    }
}
