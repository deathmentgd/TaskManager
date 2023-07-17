using DevExpress.Mvvm.POCO;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using TaskManager.Models.DISource;
using TaskManager.Models.Menu;
using TaskManager.Profiles;
using TaskManager.Services.Repositories.TaskRepository;
using TaskManager.Services.Repositories.TaskStatusRepository;
using TaskManager.ViewModels;
using TaskManager.ViewModels.CreateTaskViewModel;
using TaskManager.ViewModels.ViewEditTaskViewModel;
using TaskManager.ViewModels.ViewTasksViewModel;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        ServiceProvider ServiceProvider { get; set; }
        object Resolve(Type type, object key, string name) => type == null ? null : ServiceProvider.GetService(type);

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ServiceProvider = new ServiceCollection()
                .AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)))
                .AddSingleton(typeof(ITaskRepository), typeof(TaskRepository))
                .AddSingleton(typeof(ITaskStatusRepository), typeof(TaskStatusRepository))
                .AddTransient(typeof(IMenuBuilder), typeof(MenuBuilder))
                .AddTransient(typeof(CreateTaskViewModel))
                .AddTransient(typeof(ViewEditTaskViewModel))                
                .AddSingleton(typeof(ViewTasksViewModel))
                .AddSingleton(typeof(MainViewModel))
                .BuildServiceProvider();
            DISource.Resolver = Resolve;
        }

        /* Эта часть должна необработанные ошибки ловить, но... */
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }
        private void Application_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {            
            ReportUnhandledException(e.Exception);
            e.Handled = true;
        }
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ReportUnhandledException(e.ExceptionObject as Exception);
        }
        void ReportUnhandledException(Exception exception)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                 new System.Action(delegate ()
                                 {
                                     MessageBox.Show("Не обработанное исключение: " + exception.ToString());
                                 })
                              );
        }
    }
}
