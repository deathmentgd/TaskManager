using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Pdf.Native;
using DevExpress.Xpf.Accordion;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using TaskManager.Models.Menu;
using TaskManager.Models.Task;
using TaskManager.Services.Repositories.TaskRepository;
using TaskManager.Views.CreateTaskView;
using TaskManager.Views.ViewTasksView;

namespace TaskManager.ViewModels
{
    [GenerateViewModel(ImplementISupportServices = true)]
    public partial class MainViewModel
    {
        protected IDocumentManagerService DocumentManagerService => GetService<IDocumentManagerService>();
        protected INavigationService NavigationService => GetService<INavigationService>();
        private readonly IMenuBuilder _menuBuilder;

        /// <summary>
        /// Меню
        /// </summary>
        [GenerateProperty]
        ObservableCollection<MenuItem> _MenuItems;
        /// <summary>
        /// Текущий элемент меню
        /// </summary>
        [GenerateProperty]        
        MenuItem _SelectedItem;
        /// <summary>
        /// Флаг отображения WaitIndicator
        /// </summary>
        [GenerateProperty]
        bool _OnStartup;

        //------------------------------------------------------------------------------------

        public MainViewModel(IMenuBuilder menuBuilder)
        {
            _menuBuilder = menuBuilder;
            OnStartup = true;           
        }

        /// <summary>
        /// После загрузки отображения выбираем первое меню
        /// </summary>
        [GenerateCommand]
        public async Task OnLoaded()
        {
            if (OnStartup)
            {
                await GenerateMenuAsync();
                SelectedItem = _MenuItems.First();
            }
        }

        /// <summary>
        /// При выборе элемента меню открываем его представление
        /// </summary>
        /// <param name="e"></param>
        public void OnSelectedItemChanged(AccordionSelectedItemChangedEventArgs e)
        {
            if (e.OldItem != null)
            {
                OnStartup = false;
            }
            var newItem = e.NewItem as MenuItem;
            if (newItem == null)
            {
                return;
            }
            var document = DocumentManagerService.CreateDocument(newItem.ViewName, null,  this);
            document.DestroyOnClose = true;
            document.Show();
        }

        /// <summary>
        /// Формирование элементов меню
        /// </summary>
        async Task GenerateMenuAsync() 
        {
            MenuItems = await _menuBuilder.CreateMenuAsync();
        }
    }    
}
