using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        protected void TabPanelWindow_KeyDown1(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.W)
            {
                TabPanelWindow newTabPanelWindow = new TabPanelWindow();
                newTabPanelWindow.Show();
            }

            if(e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.V)
            {
                string s = "";

                foreach(var item in TabList)
                {

                    s += $"{item.WindowPageType.Name} - ";
                    if (item.Tab != null) s += $"{item.Tab}";
                    s += "\n";
                }

                MessageBox.Show(s);
            }
        }

        protected void TabPanelWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // очистка всего содержимого окна
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.L)
            {
                //foreach (MenuItem item in tabPanel.ContextMenu.Items)
                //{
                //    item.IsChecked = false;                    
                //}


                //tabPanel.tabContainer.Children.Clear();                
                //mainGrid.Children.Remove(ActivePage);
                //ActivePage = null;

                foreach(var win in App.Current.Windows)
                {
                    if(win is TabPanelWindow)
                    {
                        var downcast = (TabPanelWindow)win;
                        foreach(var kvp in downcast.TabInvokerType)
                        {
                            kvp.Key.IsChecked = false;
                        }

                        downcast.tabPanel.tabContainer.Children.Clear();
                        downcast.mainGrid.Children.Remove(downcast.ActivePage);
                        downcast.ActivePage = null;
                        if (downcast.IsMainWindow) continue;
                        downcast.Close();
                    }
                }

                foreach(var item in TabList)
                {
                    item.Tab = null;
                    if(item.WindowPage is ICommonControlService)
                    {
                        (item.WindowPage as ICommonControlService).DetachSideControls();
                    }
                    item.WindowPage = null;
                }
            }

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.K)
            {
                Type[] pageTypes = new Type[] {
                    Type.GetType("StrallanCalcTables.Nodes"),
                    Type.GetType("StrallanCalcTables.RigidBodies"),
                    Type.GetType("StrallanCalcTables.FrameCoreElements") };

                foreach (Type t in pageTypes)
                {
                    foreach (var record in TabList)
                    {
                        if (record.WindowPageType == t) 
                            CreateNewStandartTab(GetKeyByValue(record.WindowPageType, TabInvokerType));
                    }
                }
            }

            if(e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.W)
            {
                TabPanelWindow secondaryWindow = new TabPanelWindow();
                secondaryWindow.Show();
            }

            if(e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.A)
            {
                string s = "";
                foreach(Window win in Application.Current.Windows)
                {
                    s += win.Name + "\n";
                }
                
                MessageBox.Show(Application.Current.Windows.Count.ToString());
            }

            if(e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.F)
            {
                var windows = App.Current.Windows;

                string s = string.Empty;

                foreach (var tabWindow in windows)
                {
                    if(tabWindow is TabPanelWindow)
                    {
                        var win = (TabPanelWindow)tabWindow;



                    }
                    s += "----------------------";
                }
                
                MessageBox.Show(tabPanel.GetTabs().Length.ToString());
            }

            // show quantity of nodes in model
            if(e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.M)
            {
                string s = string.Empty;

                s = ((App)App.Current).ViewModel.GetModelGuid();

                MessageBox.Show(s);
            }

            if(e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.B)
            {
                SortingDefinitionView sdv = new SortingDefinitionView();
                sdv.ShowDialog();
            }
        }              
    }
}
