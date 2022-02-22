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

namespace Infratools
{
    public static partial class Shovel
    {
        // метод для отрисовки объекта Path, который может быть помещён в Grid
        // как линия визуализации сетки
        public static Path DrawTableLine(TableLineOrientation orientation)
        {
            Path tableLine = new Path();
            LineGeometry line = new LineGeometry();

            tableLine.Stroke = Brushes.Gray;
            tableLine.StrokeThickness = 0.5;
            tableLine.Stretch = Stretch.Fill;

            if (orientation == TableLineOrientation.Horizontal)
            {
                tableLine.VerticalAlignment = VerticalAlignment.Bottom;
                line.StartPoint = new Point(0, 1);
                line.EndPoint = new Point(1, 1);
            }
            if (orientation == TableLineOrientation.Vertical)
            {
                tableLine.HorizontalAlignment = HorizontalAlignment.Right;
                line.StartPoint = new Point(1, 0);
                line.EndPoint = new Point(1, 1);
            }

            tableLine.Data = line;
            return tableLine;
        }

        // вспомогательное перечисление с типами ориентации линий разметки Grid
        public enum TableLineOrientation
        {
            Horizontal,
            Vertical,
        }

        // зачем это нужно - искать ответ в решении UserScrollBar
        public enum AdditionTableLines
        {
            LeftVertical,
            TopHorizontal,
            None
        }

    }
}
