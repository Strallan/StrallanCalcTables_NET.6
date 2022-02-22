using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Reflection;



namespace CalcTables
{
    /// <summary>
    /// Логика взаимодействия для Nodes.xaml
    /// </summary>
    [PageAttribute("Узлы")]
    public partial class Nodes : UserControl, ICommonControlService
    {
        public Nodes()
        {
            InitializeComponent();
            DataContext = (Application.Current as App).ViewModel;
            App.CreateTable(controlGrid, (Application.Current as App).ViewModel.NodeTable);
        } 

        public void DetachSideControls()
        {
            controlGrid.Children.Clear();
        }

        private void setSortingRule_Click(object sender, RoutedEventArgs e)
        {
            SortingDefinitionView sdv = new SortingDefinitionView();
            sdv.ShowDialog();
        }

        private void SetNewFilterRule_Click(object sender, RoutedEventArgs e)
        {
            FilterDefinitionView fdv = new FilterDefinitionView();
            fdv.ShowDialog();
        }
    }
}
