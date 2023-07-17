using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Views.ViewTasksView;

namespace TaskManager.Models.Menu
{
    public class MenuBuilder:IMenuBuilder
    {
        public async Task<ObservableCollection<MenuItem>> CreateMenuAsync()
        {
            var menuItems = new ObservableCollection<MenuItem>();
            var menuItem = new MenuItem { Name = "Список задач", ViewName = typeof(ViewTasksView).ToString() };
            menuItems.Add(menuItem);
            return menuItems;
        }
    }
}
