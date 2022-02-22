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
        public TabPanel()
        {
            InitializeComponent();
            SizeChanged += TabPanel_SizeChanged;
            KeyDown += TabPanel_KeyDown;  
        }

        private void TabPanel_KeyDown(object sender, KeyEventArgs e)
        {
           if(e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.S)
           {
                ScrollInfo();
           }
        }

        private void TabPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetArrowsVisibility();
        }

        #region test control methods

        private void ScrollInfo()
        {
            MessageBox.Show($"Total tab width = {GetTotalWidth()} \n " +
                $"{nameof(tabScroll.ActualWidth)} = {tabScroll.ActualWidth} \n " +
                $"{nameof(tabScroll.HorizontalOffset)} = {tabScroll.HorizontalOffset} \n " +
                //$"{nameof(currentSwipe)} = {currentSwipe} \n " +
                $"{nameof(tabScroll.ExtentWidth)} - {tabScroll.ExtentWidth} \n" +
                $"{nameof(tabScroll.ContentHorizontalOffset)} - {tabScroll.ContentHorizontalOffset} \n" +
                $"{nameof(tabScroll.ScrollableWidth)} = {tabScroll.ScrollableWidth} \n ");

        }

        #endregion

        public UserControl[] GetTabs()
        {
            UserControl[] uca = new UserControl[tabContainer.Children.Count];
            tabContainer.Children.CopyTo(uca, 0);
            return uca;
        }
    }
}
