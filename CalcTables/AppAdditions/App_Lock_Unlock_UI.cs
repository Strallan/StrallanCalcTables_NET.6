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
        public void UnlockUI()
        {
            foreach (var win in Windows)
            {
                if (win is TabPanelWindow)
                {
                    TabPanelWindow tabPanelWindow = (TabPanelWindow)win;
                    tabPanelWindow.PreviewMouseDown -= TabPanelWindow_PreviewMouseDown;
                    tabPanelWindow.PreviewMouseUp -= TabPanelWindow_PreviewMouseUp;
                }
            }
        }

        public void LockUI()
        {
            foreach(var win in Windows)
            {
                if(win is TabPanelWindow)
                {
                    TabPanelWindow tabPanelWindow = (TabPanelWindow)win;
                    tabPanelWindow.PreviewMouseDown += TabPanelWindow_PreviewMouseDown;
                    tabPanelWindow.PreviewMouseUp += TabPanelWindow_PreviewMouseUp;
                }
            }
        }

        private void TabPanelWindow_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void TabPanelWindow_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }       
    }
}
