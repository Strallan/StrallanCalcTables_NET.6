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
        public Dictionary<MenuItem, Type> TabInvokerType = new Dictionary<MenuItem, Type>();

        private K GetKeyByValue<K, V>(V value, Dictionary<K, V> source) where V : class
        {
            K key = (from kvp in source
                     where kvp.Value == value
                     select kvp.Key).Single();
            return key;
        }

        /* метод для создания пункта меню в контекстном меню ленты вкладок
         */
        public void InitializeNewStandartTabInvoker(Type pageType)
        {
            MenuItem tabInvoker = new MenuItem()
            {
                Header = GetPageHeader(pageType),
                IsCheckable = true,
                IsChecked = false,
                StaysOpenOnClick = true,
            };
            tabInvoker.Click += StandartTabInvoker_Click;
            // запись созданного пункта меню в контекстное меню ленты вкладок
            tabPanel.ContextMenu.Items.Add(tabInvoker);
            TabInvokerType.Add(tabInvoker, pageType);
        }
    }
}
