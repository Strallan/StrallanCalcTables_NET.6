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
        public void SaveModel(string modelGuid)
        {
            TabPanelWindow[] appWindows = GetAppWindows();
            XDocument saveDoc = new XDocument();
            XElement root = new XElement("ApplicationStateConfig");

            XElement guid = new XElement("ModelGUID");
            guid.Value = modelGuid;

            XElement subRoot = new XElement("ApplicationWindows");            
            XAttribute subRootWinCount = new XAttribute("count", appWindows.Length);
            subRoot.Add(subRootWinCount);
            foreach(TabPanelWindow window in appWindows)
            {
                GetWindowState(window, ref subRoot);
            }
            root.Add(guid, subRoot);
            saveDoc.Add(root);
            saveDoc.Save(App.appConfigPath);
        }
        
        public void GetWindowState(TabPanelWindow win, ref XElement subRoot)
        {
            var tabs = win.tabPanel;

            XElement window = new XElement("WindowConfig");
            XAttribute isMain = new XAttribute("main", win.IsMainWindow ? "yes" : "no") ;
            window.Add(isMain);
            XElement placement = new XElement("Placement");
            XElement tabPanel = new XElement("TabPanel");
            XAttribute count = new XAttribute("count", win.tabPanel.GetTabs().Length);
            tabPanel.Add(count);
            window.Add(placement, tabPanel);
            var winTabs = win.tabPanel.GetTabs();
            for(int i = 0; i < winTabs.Length; i++)
            {
                TabButton tab = winTabs[i] as TabButton;
                XElement tabButton = new XElement("TabButton");                
                XAttribute pagetype = new XAttribute("pagetype", win.GetPage(tab).GetType().Name);
                XAttribute order = new XAttribute("order", i+1);
                XAttribute active = new XAttribute("active", tab.IsTabActive ? "yes" : "no");
                XElement content = new XElement("Content");
                tabButton.Add(pagetype, order, active, content);
                tabPanel.Add(tabButton);
            }
            subRoot.Add(window);
        }
        
        public TabPanelWindow[] GetAppWindows()
        {
            TabPanelWindow[] winArray = new TabPanelWindow[] { };
            foreach(var window in App.Current.Windows)
            {
                if (window is TabPanelWindow)
                {
                    Array.Resize(ref winArray, winArray.Length + 1);
                    winArray[winArray.Length - 1] = (TabPanelWindow)window;
                }
            }
            return winArray;
        }

        public void LoadModel()
        {
            XDocument loadXmlModel = XDocument.Load(App.appConfigPath);
            bool appHasSeveralWindows = loadXmlModel.Descendants()
                .Where(node => node.Name == "WindowConfig").Count() > 1 ? true : false;
            var mainWindow = loadXmlModel.Descendants()
                .Where(node => node.Name == "WindowConfig" && node.Attribute("main").Value == "yes").FirstOrDefault();
            SetWindowState(mainWindow);
            if (!appHasSeveralWindows) return;
            var secondaryWindows = loadXmlModel.Descendants()
                .Where(node => node.Name == "WindowConfig" && node.Attribute("main").Value == "no");
            foreach (var win in secondaryWindows)
            {
                TabPanelWindow secondaryWindow = new TabPanelWindow();
                secondaryWindow.Show();
                secondaryWindow.SetWindowState(win);
            }
        }

        public void SetWindowState(XElement winXmlDescription)
        {
            Type activePageType = null;
            var appWinPageTypes = winXmlDescription.Descendants()
                .Where(node => node.Name == "TabButton")
                .Select((node) =>
                {
                    Type t = Type.GetType($"CalcTables.{node.Attribute("pagetype").Value}");
                    if (node.Attribute("active").Value == "yes")
                    {
                        activePageType = t;
                    };
                    return t;
                });
            // запрос для получения списка MenuItems соответствующих типам 
            // создаваемых страниц - важно сохранить порядок, поэтому есть
            // вложенный запрос
            var menuItems = from type in appWinPageTypes
                            select
                                (from record in TabList
                                 where record.WindowPageType == type
                                 select GetKeyByValue(record.WindowPageType, TabInvokerType)).Single();
            // создание стандартных вкладок  
            foreach (var tabInvoker in menuItems)
            {
                CreateNewStandartTab(tabInvoker);
                RiseStandartTab(tabInvoker);
            }
            TabButton activeArg = (from record in TabList
                                   where record.WindowPageType == activePageType
                                   select record.Tab).Single();
            // сама установка новой 
            SetNewActivePage(GetPage(activeArg));
            // пометка вкладки как активной на ленте вкладок
            tabPanel.SetActiveTab(activeArg, OnTabDeactivated, OnTabBecomeActive);
        }
    }    
}
