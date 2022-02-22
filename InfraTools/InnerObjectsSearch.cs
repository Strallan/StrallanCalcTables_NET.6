using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Infratools
{
    public static partial class Shovel
    {
        /* метод возвращает ScrollBar в составе экземпляра ScrollViewer,
         * горизонтальный или вертикальный в зависимости от второго аргумента
         */
        public static ScrollBar GetScrollViewBar(ScrollViewer scrollViewer, Orientation orientation)
        {
            scrollViewer.ApplyTemplate();
            int numberOfChildren = VisualTreeHelper.GetChildrenCount(scrollViewer);

            /* 
             * ScrollViewer - это Control. Его свойство Content заполнено единственным
             * элементом - Grid. Всё остальное, включая ScrollBar's, находится в Grid
             * и для их получения нужно искать дочерние объекты уже в нём
             * 
             */
            DependencyObject grid = VisualTreeHelper.GetChild(scrollViewer, 0);
            numberOfChildren = VisualTreeHelper.GetChildrenCount(grid);

            for (int i = 0; i < numberOfChildren; i++)
            {
                DependencyObject obj = VisualTreeHelper.GetChild(grid, i);

                if (obj is ScrollBar)
                {
                    ScrollBar sb = obj as ScrollBar;
                    if (sb.Orientation == orientation) return sb;
                    //Binding artefact
                    //sb.Height = scrollBarWidth;

                    //Binding heightBind = new Binding();
                    //heightBind.Source = this;
                    //heightBind.Path = new PropertyPath("ScrollBarWidth");
                    //heightBind.Mode = BindingMode.TwoWay;
                    //sb.SetBinding(ScrollBar.HeightProperty, heightBind);
                }
            }
            return null;
        }

        public static FE GetControlGrid<FE>(ContentControl control) where FE : FrameworkElement
        {
            return null;
        }

        public static Grid GetControlGrid(FrameworkElement control)
        {
            int numberOfChildren = VisualTreeHelper.GetChildrenCount(control);

            for (int i = 0; i < numberOfChildren; i++)
            {
                DependencyObject maybeGrid = VisualTreeHelper.GetChild(control, i);
                if (maybeGrid is Grid) return maybeGrid as Grid;
            }

            return null;
        }

        public static void ShowChildrens(FrameworkElement control)
        {
            int numberOfChildren = VisualTreeHelper.GetChildrenCount(control);
            string listOfChildren = "";

            for (int i = 0; i < numberOfChildren; i++)
            {
                DependencyObject maybeGrid = VisualTreeHelper.GetChild(control, i);
                listOfChildren += $"\n {maybeGrid.GetType().Name}";
            }

            MessageBox.Show(listOfChildren);
        }
    }
}
