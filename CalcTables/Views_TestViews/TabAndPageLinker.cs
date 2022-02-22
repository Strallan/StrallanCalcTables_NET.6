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
        private UserControl GetPage(TabButton tabButton)
        {
            return (from record in TabList
                    where record.Tab == tabButton
                    select record.WindowPage).Single();
        }

        private UserControl GetPage(MenuItem tabInvoker)
        {
            return (from record in TabList
                    where record.WindowPageType == TabInvokerType[tabInvoker]
                    select record.WindowPage).Single();
        }

        private Type GetPageType(MenuItem tabInvoker)
        {
            return (from record in TabList
                    where record.WindowPageType == TabInvokerType[tabInvoker]
                    select record.WindowPageType).Single();
        }

        private TabButton GetTab(MenuItem tabInvoker)
        {
            return (from record in TabList
                    where record.WindowPageType == TabInvokerType[tabInvoker]
                    select record.Tab).Single();
        }

        private MenuItem GetMenuItem(TabButton tabButton)
        {
            var item = (from record in TabList
                    where record.Tab == tabButton
                    select record).Single();

            return GetKeyByValue(item.WindowPageType, TabInvokerType);
        }
    }
}
