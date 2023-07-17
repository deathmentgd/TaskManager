using AutoMapper;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models.Task;
using TaskManager.Models.TaskStatus;
using TaskManager.Services.Repositories.TaskRepository;
using TaskManager.Services.Repositories.TaskStatusRepository;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TaskManager.ViewModels.ViewEditTaskViewModel
{
    [GenerateViewModel(ImplementISupportServices = true, ImplementISupportParentViewModel = true)]
    public partial class ViewEditTaskViewModel : ISupportParameter
    {
        protected virtual IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();
        protected virtual INavigationService NavigationService => GetService<INavigationService>(); 

        /// <summary>
        /// Репозиторий для работы с задачей
        /// </summary>
        private readonly ITaskRepository _taskRepository;
        /// <summary>
        /// Репозиторий для получения справочника статусов задачи
        /// </summary>
        private readonly ITaskStatusRepository _taskStatusRepository;
        /// <summary>
        /// Автомаппер полей
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Просматриваемая задачи
        /// </summary>
        [GenerateProperty]
        TaskDto _TaskItem = null;
        /// <summary>
        /// Справочник статусов задачи. Используем на форме в LookupEdit
        /// </summary>
        [GenerateProperty]
        List<TaskStatusDto> _TaskStatuses = null;

        /// <summary>
        /// Реализация ISupportParameter, чтобы прокинуть данные при открытии формы
        /// </summary>
        public object Parameter { get; set; }

        /// <summary>
        /// Флаг видимости кнопки "В работу"
        /// </summary>
        [GenerateProperty]        
        bool _IsBeginButtonVisible = false;
        /// <summary>
        /// Флаг видимости кнопки "Приостановить"
        /// </summary>
        [GenerateProperty]
        bool _IsPauseButtonVisible = false;
        /// <summary>
        /// Флаг видимости кнопки "Завершить"
        /// </summary>
        [GenerateProperty]
        bool _IsFinishButtonVisible = false;
        /// <summary>
        /// Флаг видимости блока кнопок "Редактирование"
        /// </summary>
        [GenerateProperty]
        bool _IsEditButtonsVisible;
        [GenerateProperty]
        bool _IsEditButtonChecked;

        ///-------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="taskRepository"></param>
        /// <param name="taskStatusRepository"></param>
        /// <param name="mapper"></param>
        public ViewEditTaskViewModel(ITaskRepository taskRepository, ITaskStatusRepository taskStatusRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _taskStatusRepository = taskStatusRepository;
            _mapper= mapper;
            IsEditButtonsVisible = true && CanEdit();
            IsEditButtonChecked = false;
        }

        /// <summary>
        /// Подписка на загрузку формы
        /// </summary>
        [GenerateCommand]
        async void OnViewLoadedAsync()
        {
            // После загрузки формы получаем всю ифно о задаче
            await GetTaskInfoAsync((long)Parameter);
            // Вытаскиваем справочник статусов задачи
            await GetTaskStasusesAsync();
            // Отображаем нужные кнопки
            SetButtonsVisibility();
        }

        /// <summary>
        /// Выставляет доступность действий над задачей
        /// </summary>
        private void SetButtonsVisibility()
        {
            if (TaskItem!= null)
            {
                switch(TaskItem.StatusId) 
                { 
                    case 0:
                    case 2:
                        IsBeginButtonVisible = true && CanDoWork();
                        IsPauseButtonVisible = false;
                        IsFinishButtonVisible = false;
                        break;
                    case 1:
                        IsBeginButtonVisible = false;
                        IsPauseButtonVisible = true && CanDoWork(); 
                        IsFinishButtonVisible = true && CanDoWork();
                        break;
                    case 3:
                        IsBeginButtonVisible = false;
                        IsPauseButtonVisible = false;
                        IsFinishButtonVisible = false;
                        IsEditButtonsVisible = false;
                        break;
                }
            }
            else
            {
                IsBeginButtonVisible = false;
                IsPauseButtonVisible = false;
                IsFinishButtonVisible = false;
                IsEditButtonsVisible = false;
            }
        }

        /// <summary>
        /// Проверка прав на редактирование
        /// </summary>
        /// <returns></returns>
        private bool CanEdit()
        {
            return true;
        }

        /// <summary>
        /// Проверка прав на изменения статуса
        /// </summary>
        /// <returns></returns>
        private bool CanDoWork()
        {
            return true;
        }

        /// <summary>
        /// Получить данные о задаче
        /// </summary>
        /// <returns></returns>
        private async Task GetTaskInfoAsync(long taskId)
        {
            TaskItem = await _taskRepository.GetTaskAsync(taskId);
        }

        /// <summary>
        /// Получить справочник статусов задачи
        /// </summary>
        /// <returns></returns>
        private async Task GetTaskStasusesAsync()
        {
            TaskStatuses = (await _taskStatusRepository.GetTaskStatusesAsync()).ToList();
        }

        /// <summary>
        /// Установить статус задаче
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [GenerateCommand]
        async Task SetTaskStatusAsync(short statusId)
        {            
            TaskItem = await _taskRepository.SetTaskStatusAsync(TaskItem.Id, statusId);
            SetButtonsVisibility();
        }

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        /// <returns></returns>
        [GenerateCommand]
        async Task SaveTaskAsync()
        {
            if (IsEditButtonsVisible)
            {
                if (MessageBoxService.ShowMessage("Сохранить?", "Необходимо подтверждение", MessageButton.YesNo, MessageIcon.Question, MessageResult.No) == MessageResult.Yes)
                {
                    // В идеале отправлять на сохранение только измененные поля
                    await _taskRepository.UpdateTaskAsync(TaskItem.Id, _mapper.Map<UpdateTaskDto>(TaskItem));
                }
            }
        }

        /// <summary>
        /// Отменить изменения/ обновить данные о задаче из источника
        /// </summary>
        /// <returns></returns>
        [GenerateCommand]
        async Task RestoreTaskAsync()
        {
            await GetTaskInfoAsync(TaskItem.Id);
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
                    await _taskRepository.DeleteTaskAsync(TaskItem.Id);
                    GoBack();
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
            return true;
        }

        /// <summary>
        /// Вернуться к списку задач
        /// </summary>
        /// <returns></returns>
        [GenerateCommand]
        void GoBack()
        {
            NavigationService.GoBack();
        }
    }
}
