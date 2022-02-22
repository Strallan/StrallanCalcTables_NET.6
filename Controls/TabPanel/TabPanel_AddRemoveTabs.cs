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
        // активная вкладка
        private UserControl activeTab;
        public UserControl ActiveTab
        {
            get { return activeTab; }
            set { activeTab = value; }
        }

        // возврат текущего количества вкладок на ленте
        private int tabCount;
        public int TabCount
        {
            get 
            {
                tabCount = tabContainer.Children.Count;
                return tabCount; 
            }            
        }

        public Action<UserControl> ActiveTabLastAction;
        public Action<UserControl> ActiveTabFirstAction;
        
        /* метод для пометки вкладки на ленте как активной через заполнение соответствующего поля
         * также отрабатывают два метода, переданных с делегатами, влияющих на состояние
         * вкладки, теряющей активный статус и 
         * вкладки, приобретающей активный статус
         * например, стиль кнопки и т.п.
         * здесь методы в аргументах, и это предпочтительно
         */
        public void SetActiveTab(UserControl newActiveTab, Action<UserControl> ActiveTabLastAction, Action<UserControl> ActiveTabFirstAction)
        {
            if (ContainsTab(newActiveTab))
            {
                if (ActiveTab != null)
                {                    
                    ActiveTabLastAction?.Invoke(ActiveTab);
                }
                ActiveTab = newActiveTab;
                ActiveTabFirstAction?.Invoke(ActiveTab);
            }
        }

        /* метод для пометки вкладки на ленте как активной через заполнение соответствующего поля
         * также отрабатывают два метода, переданных с делегатами, влияющих на состояние
         * вкладки, теряющей активный статус и 
         * вкладки, приобретающей активный статус
         * здесь взяты методы из полей самого класса ленты с вкладками
         * присвоение происходит в конструкторе родительского окна или контрола с лентой вкладок
         */
        public void SetActiveTab(UserControl newActiveTab)
        {
            SetActiveTab(newActiveTab, ActiveTabLastAction, ActiveTabFirstAction);
        }

        /* метод для добавления новой вкладки на ленту
         */
        public UserControl AddNewTab(UserControl tab)
        {            
            tabContainer.Children.Add(tab);
            tab.SizeChanged += Tab_SizeChanged;
            tab.MouseLeftButtonUp += SwipeOnPartialHide;
            SetArrowsVisibility();

                double sum = 0;
                double[] swipeSteps = GetSwipeSteps();
                for(int i = 0; i < swipeSteps.Length; i++)
                {
                    sum += swipeSteps[i];
                }

                tabScroll.ScrollToHorizontalOffset(sum);

            return tab;
        }


        /* методя для удаления вкладки с ленты
         */
        public void RemoveTab(UserControl tab)
        {
            tab.SizeChanged -= Tab_SizeChanged;
            tabContainer.Children.Remove(tab);
            if (tab == activeTab) activeTab = null;
            SetArrowsVisibility();
        }

        /* данный метод-обработчик события изменения размера контрола
         * пришлось ввести так как ActualWidth вкладки
         * меняется не сразу. В тот момент, когда он должен быть известен
         * он всё ещё равняется нулю. Соответственно, метод отслеживающий 
         * момент переполнения видимой области ленты вкладок неверно
         * определяет когда должны появиться кнопки навигации по ленте
         * опоздание на одну вкладку
         */
        private void Tab_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetArrowsVisibility();
            UserControl tab = sender as UserControl;            
        }

        
        /* метод для скрытия вкладки на ленте без удаления
         * хз пока зачем он нужен, больше похоже на древний артефакт
         */
        public void HideTab(UserControl tab)
        {
            tab.Visibility = Visibility.Collapsed;
        }

        /* получить вкладку из ленты по индексу
         */
        public UserControl GetTab(int index)
        {
            if(tabContainer.Children.Count > 0)
            {
                if(index < tabContainer.Children.Count) return tabContainer.Children[index] as UserControl;
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /* поскольку класс TabPanel является лишь интерфейсом для организации
         * элементов, может существовать внешний источник с элементами
         * и может быть ситуация, когда надо проводить есть ли элемент в ленте
         */
        public bool ContainsTab(UserControl tab)
        {
            return tabContainer.Children.Contains(tab);
        }

        /* получить индекс элемента
         * можно задавать как 
         */
        public int GetTabIndex(UserControl tab)
        {
            return tabContainer.Children.IndexOf(tab);
        }

        /* определение является ли элемент активной вкладкой
         */
        public bool IsActiveTab(UserControl tabToCompare)
        {
            if (activeTab == null) return false;
            return activeTab == tabToCompare;
        }
    }
}