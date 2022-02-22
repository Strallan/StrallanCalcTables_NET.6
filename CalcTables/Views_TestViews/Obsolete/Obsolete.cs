using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Kodama;


using static CalcTables.App;

namespace CalcTables
{
    /// <summary>
    /// Логика взаимодействия для TabPanelWindow.xaml
    /// </summary>
    public partial class TabPanelWindow : Window
    {
        //private void LoadState()
        //{
        //    ApplicationStateConfig loadedState = new ApplicationStateConfig(App.loadModelPath);

        //    // запрос на получение экземпляра класса конфигурации окна
        //    WindowConfig mainWindow = (from win in loadedState.AppWindows
        //                               where (win.IsMainWindow == true)
        //                               select win).Single();
        //    SetWindowState(mainWindow);
        //    if (loadedState.AppWindows.Count == 1) return;
        //    var secondary = from win in loadedState.AppWindows
        //                    where (win.IsMainWindow != true)
        //                    select win;
        //    foreach (var win in secondary)
        //    {
        //        TabPanelWindow secondaryWindow = new TabPanelWindow();
        //        secondaryWindow.Show();
        //        secondaryWindow.SetWindowState(win);
        //    }
        //}

        //private void SetWindowState(WindowConfig winConfig)
        //{
        //    Type activePageType = Type.GetType($"CalcTables.{winConfig.ActiveTab}");
        //    // запрос для получения типов страниц 
        //    var pageTypes = from numberedTab in winConfig.OpenedTabs
        //                    select Type.GetType($"CalcTables.{numberedTab.Value}");
        //    // запрос для получения списка MenuItems соответствующих типам 
        //    // создаваемых страниц - важно сохранить порядок, поэтому есть
        //    // вложенный запрос
        //    var menuItems = from type in pageTypes
        //                    select
        //                        (from record in TabList
        //                         where record.WindowPageType == type
        //                         select GetKeyByValue(record.WindowPageType, TabInvokerType)).Single();
        //    // создание стандартных вкладок  
        //    foreach (var tabInvoker in menuItems)
        //    {
        //        CreateNewStandartTab(tabInvoker);
        //        RiseStandartTab(tabInvoker);
        //    }
        //    TabButton activeArg = (from record in TabList
        //                           where record.WindowPageType == activePageType
        //                           select record.Tab).Single();
        //    // сама установка новой 
        //    SetNewActivePage(GetPage(activeArg));
        //    // пометка вкладки как активной на ленте вкладок
        //    tabPanel.SetActiveTab(activeArg, OnTabDeactivated, OnTabBecomeActive);
        //}

        //public class WindowConfig
        //{
        //    private bool isMainWindow;
        //    public bool IsMainWindow
        //    {
        //        get { return isMainWindow; }
        //        set { isMainWindow = value; }
        //    }

        //    private string activeTab;
        //    public string ActiveTab
        //    {
        //        get { return activeTab; }
        //        set { activeTab = value; }
        //    }

        //    private SortedList<int, string> openedTabs;
        //    public SortedList<int, string> OpenedTabs
        //    {
        //        get { return openedTabs; }
        //        set { openedTabs = value; }
        //    }

        //    public WindowConfig()
        //    {
        //        IsMainWindow = false;
        //        ActiveTab = "";
        //        OpenedTabs = new SortedList<int, string>();
        //    }
        //}

        //public class ApplicationStateConfig
        //{
        //    private XDocument winConfigFile = new XDocument();
        //    public XDocument WinConfigFile
        //    {
        //        get { return winConfigFile; }
        //        set { winConfigFile = value; }
        //    }

        //    private List<WindowConfig> appWindows;
        //    public List<WindowConfig> AppWindows
        //    {
        //        get { return appWindows; }
        //        set { appWindows = value; }
        //    }

        //    private XElement AppRoot { get; set; }

        //    public ApplicationStateConfig(string source)
        //    {
        //        WinConfigFile = XDocument.Load(source);
        //        AppWindows = new List<WindowConfig>();
        //        AppWindowsRoot();
        //        GetWindowConfig();
        //    }

        //    private void AppWindowsRoot()
        //    {
        //        XElement root = WinConfigFile.Descendants().
        //            Where(appwin => appwin.Name == NodeNames.ApplicationWindows.ToString()).FirstOrDefault();
        //        AppRoot = root;
        //    }

        //    private void GetWindowConfig()
        //    {
        //        var windows = AppRoot.Descendants().Where(win => win.Name == NodeNames.WindowConfig.ToString());

        //        foreach (var win in windows)
        //        {
        //            WindowConfig winConfig = new WindowConfig();

        //            var tabs = win.Descendants().Where(tab => tab.Name == NodeNames.TabButton.ToString());

        //            foreach (var tab in tabs)
        //            {
        //                winConfig.OpenedTabs.Add
        //                    (
        //                    Int32.Parse(tab.Attribute(Attributes.order.ToString()).Value),
        //                    tab.Attribute(Attributes.pagetype.ToString()).Value);
        //                if (tab.Attribute(Attributes.active.ToString()).Value == "yes")
        //                    winConfig.ActiveTab = tab.Attribute(Attributes.pagetype.ToString()).Value;
        //            }

        //            if (win.Attribute(Attributes.main.ToString()).Value == "yes") winConfig.IsMainWindow = true;

        //            AppWindows.Add(winConfig);
        //        }
        //    }

        //    public enum NodeNames
        //    {
        //        ApplicationWindows,
        //        WindowConfig,
        //        TabButton,
        //    }

        //    public enum Attributes
        //    {
        //        count,
        //        main,
        //        order,
        //        pagetype,
        //        active,
        //    }
        //}

    }
}
