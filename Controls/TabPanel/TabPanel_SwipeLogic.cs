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
    /// Логика взаимодействия для TabPanel.xaml
    /// </summary>
    public partial class TabPanel : UserControl
    {
        /* метод для получения общей суммарной ширины всех элементов в 
         * ряду вкладок
         */
        private double GetTotalWidth()
        {
            double totalWidth = 0;           
            foreach (UserControl item in tabContainer.Children)
            {                
                totalWidth += item.ActualWidth;                
            }
            return totalWidth;
        }

        /* свойство для возврата значения отступа на ScrollView назад
         * в зависимости от текущего отступа и длины вкладки
         */
        public double OffsetBack
        {
            get
            {
                double sum = 0;
                double[] swipePositions = GetSwipeSteps();
                for (int i = 0; i < swipePositions.Length; i++)
                {
                    sum += swipePositions[i];
                    if (sum >= tabScroll.HorizontalOffset)
                    {
                        return sum - swipePositions[i];
                    }
                }
                return 0;
            }
        }

        /* свойство для возврата значения отступа ScrollView вперёд
         * в зависимости от текущего отступа и длины вкладок
         */
        public double OffsetForward
        {
            get
            {
                double sum = 0;
                double[] swipePositions = GetSwipeSteps();
                for (int i = 0; i < swipePositions.Length; i++)
                {
                    sum += swipePositions[i];
                    if (sum > tabScroll.HorizontalOffset)
                    {
                        return sum;

                    }
                }
                return 0;
            }
        }

        /* получить список длин всех вкладок в ленте
         */
        private double[] GetSwipeSteps()
        {
            int index = 0;
            double[] swipePositions = new double[tabContainer.Children.Count];
            foreach (UserControl item in tabContainer.Children)
            {
                swipePositions[index] = item.ActualWidth;
                index++;
            }
            return swipePositions;
        }

        /* условие для видимости стрелок прокрутки вкладок
         * когда общая длина вкладок больше, чем видимая область ScrollView
         * стрелки появляются
         * и наоборот
         */
        public void SetArrowsVisibility()
        {            
            if (GetTotalWidth() >= tabScroll.ActualWidth) SwipeArrowsVisible = true;
            else SwipeArrowsVisible = false;
        }

        /* сдвижка scrollview влево
         */
        private void LeftTabSwipe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tabScroll.ScrollToHorizontalOffset(OffsetBack);
            }
            catch
            {
            
            }
        }

        /* сдвижка scrollview вправо
         */
        private void RightTabSwipe_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                tabScroll.ScrollToHorizontalOffset(OffsetForward);                
            }
            catch
            {
            
            }
        }

        private void SwipeOnPartialHide(object sender, MouseButtonEventArgs e)
        {
            UserControl tab = sender as UserControl;
            int index = GetTabIndex(tab);
            double[] swipeSteps = GetSwipeSteps();

            double sum = 0;

            for(int i = 0; i < swipeSteps.Length; i++)
            {
                sum += swipeSteps[i];
                if( i == index)
                {
                    if (tabScroll.HorizontalOffset < sum && tabScroll.HorizontalOffset > sum - swipeSteps[i])
                        tabScroll.ScrollToHorizontalOffset(sum - swipeSteps[i]);


                    double offsetPlusWidth = tabScroll.HorizontalOffset + tabScroll.ActualWidth;

                    if(offsetPlusWidth > sum - swipeSteps[i] && offsetPlusWidth < sum)
                    {
                        tabScroll.ScrollToHorizontalOffset(tabScroll.HorizontalOffset + (sum - offsetPlusWidth));
                    }
                        
                }
            }

        }
    }
}
