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

namespace CalcTables
{
    /// <summary>
    /// Логика взаимодействия для CableElements.xaml
    /// </summary>
    [PageAttribute("Кабельные элементы")]
    public partial class CableElements : UserControl
    {
        public CableElements()
        {
            InitializeComponent();
            stubText.Text = this.GetType().Name;
        }
    }
}
