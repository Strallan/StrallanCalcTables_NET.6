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

using static Infratools.DefineDependencyProperties;

namespace Kodama
{
    /// <summary>
    /// Логика взаимодействия для TabPanel.xaml
    /// </summary>
    public partial class TabPanel : UserControl
    {
        static TabPanel()
        {
            SwipeArrowsVisibleProperty = SetBoolDependency(nameof(SwipeArrowsVisible), typeof(TabPanel), null);
        }

        public static readonly DependencyProperty SwipeArrowsVisibleProperty;
        public bool SwipeArrowsVisible
        {
            get { return (bool)GetValue(SwipeArrowsVisibleProperty); }
            set { SetValue(SwipeArrowsVisibleProperty, value); }
        }

    }
}
