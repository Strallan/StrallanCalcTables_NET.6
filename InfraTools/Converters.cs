using System;
using System.Collections.Generic;
using System.Collections;
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
using System.Globalization;

namespace Infratools
{
    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value.ToString();
            //return new ImageSourceConverter().ConvertFromString(path) as ImageSource;
            try
            {
                return new ImageSourceConverter().ConvertFromString($"pack://application:,,,/{path}") as ImageSource;
            }
            catch
            {
                return new BitmapImage();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;

        }
    }

    // ��������� - ��������, �� � ������ �� ���������� ����������� ����
    // ����� ��������������� ������������ ������ ��������� ����������� ���� Brush
    [ValueConversion(typeof(string), typeof(Brush))]
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (Brush)(new BrushConverter().ConvertFrom(value.ToString()));
            }
            // ���� �������������, ��� ������������� �������� ������� �� �������� ���������� �����
            // ��-�� ����� � value �������� null
            // ����������, ��� ���� � ������ ���������� �������� �������� �� ���������
            // �� �� �������� ����� ������ � � ���������� StringToImage
            catch
            {
                return new SolidColorBrush(Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class GridLengthToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GridLength convertedValue = (GridLength)value;
            return convertedValue.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visible = (bool)value;
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

