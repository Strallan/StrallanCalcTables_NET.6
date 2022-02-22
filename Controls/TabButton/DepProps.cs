using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для TabButtonCustomControl.xaml
    /// </summary>
    public partial class TabButton : UserControl
    {
        static TabButton()
        {
            TabTextProperty = SetStringDependency(nameof(TabNameText), typeof(TabButton), null);
            TabIconPathProperty = SetStringDependency(nameof(TabIconPath), typeof(TabButton), null);
            CloseButtonBrushProperty = SetBrushDependency(nameof(CloseButtonBrush), typeof(TabButton), null);
            BottomLineBrushProperty = SetBrushDependency(nameof(BottomLineBrush), typeof(TabButton), null);
            CloseButtonRadiusProperty = SetDoubleDependency(nameof(CloseButtonRadius), typeof(TabButton), null);
            CloseButtonSizeProperty = SetDoubleDependency(nameof(CloseButtonSize), typeof(TabButton), null);
        }

        public static readonly DependencyProperty TabTextProperty;
        public string TabNameText
        {
            get { return (string)GetValue(TabTextProperty); }
            set { SetValue(TabTextProperty, value); }
        }

        public static readonly DependencyProperty TabIconPathProperty;
        public string TabIconPath
        {
            get { return (string)GetValue(TabIconPathProperty); }
            set { SetValue(TabIconPathProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonBrushProperty;
        public Brush CloseButtonBrush
        {
            get { return (Brush)GetValue(CloseButtonBrushProperty); }
            set { SetValue(CloseButtonBrushProperty, value); }
        }

        public static readonly DependencyProperty BottomLineBrushProperty;
        public Brush BottomLineBrush
        {
            get { return (Brush)GetValue(BottomLineBrushProperty); }
            set { SetValue(BottomLineBrushProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonRadiusProperty;
        public double CloseButtonRadius
        {
            get { return (double)GetValue(CloseButtonRadiusProperty); }
            set { SetValue(CloseButtonRadiusProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonSizeProperty;
        public double CloseButtonSize
        {
            get { return (double)GetValue(CloseButtonSizeProperty); }
            set { SetValue(CloseButtonSizeProperty, value); }
        }

        //public static readonly DependencyProperty TabIsActiveProperty;
        //public double TabIsActive
        //{
        //    get { return (double)GetValue(TabIsActiveProperty); }
        //    set { SetValue(TabIsActiveProperty, value); }
        //}
    } 
}
