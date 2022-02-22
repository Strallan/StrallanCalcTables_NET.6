using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using System.Reflection;

using Kodama;
using CalcTables.CoreInterface;


namespace CalcTables
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AddViewModel();

        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            ViewModel.OnClose();
        }

        public CalcModelTypes ViewModel
        {
            get;
            set;
        }

        public void AddViewModel()
        {
            ViewModel = new CalcModelTypes();
            OnLoadModel += ViewModel.Load;
            OnSaveModel += ViewModel.Save;
            AfterLoad += ViewModel.AfterLoad;

            ViewModel.LockUI += LockUI;
            ViewModel.UnlockUI += UnlockUI;
        }


        public static CalcModelTypes GetMainviewModel(App application)
        {
            return application.ViewModel;
        }

        static App()
        {
            MainWindowDefined = false;
            TabList = new List<TabAndPageLinker>();
            IsTabListDefined = false;
            ModelAdminPage = new ModelPage();
        }



        public static bool MainWindowDefined
        {
            get;
            set;
        }

        public static List<TabAndPageLinker> TabList;

        public static bool IsTabListDefined
        {
            get;
            set;
        }

        public static bool TabCreated(Type pageType)
        {
            return (from item in TabList
                    where item.WindowPageType == pageType
                    select item).Any(item => item.Tab != null);
        }

        public static event Action<Window, Type> TabInvoked;

        public static void OnTabInvoked(Window senderWindow, Type windowPageType)
        {
            TabInvoked.Invoke(senderWindow, windowPageType);
        }

        public static ModelPage ModelAdminPage
        {
            get;
            set;
        }

        public static void CreateTable(Grid grid, FrameworkElement table)
        {
            grid.Children.Add(table);
            Grid.SetColumn(table, 0);
            Grid.SetRow(table, 0);
        }
    }

    public class TabAndPageLinker
    {
        public Type WindowPageType { get; set; }
        public TabButton Tab { get; set; }
        public UserControl WindowPage { get; set; }
    }



}
