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

namespace Kodama
{
    /// <summary>
    /// Логика взаимодействия для TabButtonCustomControl.xaml
    /// </summary>
    public partial class TabButton : UserControl
    {
        private Brush originBottomLineBrush;
        private Brush originBackground;
        private Brush originCloseButtonBrush;

        /*дополнительный атрибут контрола
         *отвечает за изменение состояния вкладки
         *когда присваивается false - цвета возвращаются к первоначальным настройкам
         */
        private bool isTabActive;
        public bool IsTabActive
        {
            get { return isTabActive; }
            set
            {
                isTabActive = value;
                if(value == false)
                {
                    BottomLineBrush = originBottomLineBrush;
                    Background = originBackground;
                    CloseButtonBrush = originCloseButtonBrush;
                }
            }
        }

        public TabButton()
        {
            InitializeComponent();
            originBottomLineBrush = BottomLineBrush;
            originBackground = Background;
            originCloseButtonBrush = CloseButtonBrush;

            CloseButtonRadius = 6.0;
            CloseButtonSize = 25.0;
        }

        public void SetImage(string path)
        {
            tabIcon.Source = new ImageSourceConverter().ConvertFromString($"pack://application:,,,/{path}") as ImageSource;
        }
    }    
}


