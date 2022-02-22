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
    /// Логика взаимодействия для Groups.xaml
    /// </summary>
    [PageAttribute("Группы")]
    public partial class Groups : UserControl
    {
        public Groups()
        {
            InitializeComponent();
            stubText.Text = this.GetType().Name;
        }
    }
}
