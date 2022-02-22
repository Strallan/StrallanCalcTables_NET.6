using System;
using System.Collections;
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
using System.Windows.Shapes;
using Kodama;
using static CalcTables.App;

namespace CalcTables
{
    /// <summary>
    /// Логика взаимодействия для TabPanelWindow.xaml
    /// </summary>
    public partial class TabPanelWindow : Window
    {
        public TabPanelWindow()
        {
            InitializeComponent();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            if (Application.Current.Windows.Count <= 1) IsMainWindow = true;
            else IsMainWindow = false;

            InitializeTabContextMenu();
            if (!IsTabListDefined) InitializeCommonTabList();

            if (IsMainWindow)
            {
                Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
                CreateModelButton(out modelTabButton);
            };

            column = 0;
            columnSpan = 2;
            row = 1;
            rowSpan = 1;

            // может быть, это ненужное дублирование?
            tabPanel.ActiveTabLastAction += OnTabDeactivated;
            tabPanel.ActiveTabFirstAction += OnTabBecomeActive;

            KeyDown += TabPanelWindow_KeyDown;
            KeyDown += TabPanelWindow_KeyDown1;

            TabInvoked += HideSecondInstance;           
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(this, e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        private TabButton modelTabButton;

        private void CreateModelButton(out TabButton modelTabButton)
        {
            modelTabButton = new TabButton();
            modelTabButton.TabNameText = "Модель";
            modelTabButton.Foreground = new SolidColorBrush(Colors.Wheat);
            modelTabButton.MouseLeftButtonUp += ModelTabButton_MouseLeftButtonUp;
            modelTabButton.tabCloseButton.Visibility = Visibility.Hidden;
            modelTabButton.ContextMenu = tabPanel.ContextMenu;

            mainGrid.Children.Add(modelTabButton);
            Grid.SetColumn(modelTabButton, 0);
            Grid.SetRow(modelTabButton, 0);

            ModelAdminPage.ReturnToTabView += OnModelPageHiding;
            ModelAdminPage.LoadState += LoadModel;
            //ModelAdminPage.LoadState += LoadState;
            ModelAdminPage.SaveState += SaveModel;
            ModelAdminPage.IsActive = false;
        }

        /* для закрытия вкладки в один клик
         */
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            this.Activate();
        }

        public bool IsMainWindow
        {
            get;
            private set;
        }

        public TabPanelWindow LinkToMainWindow
        {
            get;
            internal set;
        }

        private UserControl activePage;
        public UserControl ActivePage
        {
            get { return activePage; }
            set { activePage = value; }
        }        

        protected int column;
        protected int columnSpan;
        protected int row;
        protected int rowSpan; 


        

        /* инициализвация всех необходимых пунктов меню
         * для контекстного меню ленты вкладок
         * Думаю, что в будущем можно делать это короче,
         * присвоив всем типам страниц нужный атрибут
         * то есть в цикле
         */
        protected void InitializeTabContextMenu()
        {
            InitializeNewStandartTabInvoker(typeof(CableElements));
            InitializeNewStandartTabInvoker(typeof(CoordinateSystems));
            InitializeNewStandartTabInvoker(typeof(FlatElements));
            InitializeNewStandartTabInvoker(typeof(FrameCoreElements));
            InitializeNewStandartTabInvoker(typeof(Groups));
            InitializeNewStandartTabInvoker(typeof(Loads));
            InitializeNewStandartTabInvoker(typeof(Materials));
            InitializeNewStandartTabInvoker(typeof(Nodes));
            InitializeNewStandartTabInvoker(typeof(Offsets));
            InitializeNewStandartTabInvoker(typeof(CalcTables.CsOrientation));
            InitializeNewStandartTabInvoker(typeof(RigidBodies));
            InitializeNewStandartTabInvoker(typeof(Sections));
            InitializeNewStandartTabInvoker(typeof(SpringElements));
            InitializeNewStandartTabInvoker(typeof(SuperElements));
        }

        protected void InitializeCommonTabList()
        {
            foreach (var kvp in TabInvokerType)
            {
                TabAndPageLinker linker = new TabAndPageLinker();
                linker.WindowPageType = kvp.Value;
                TabList.Add(linker);
            }
            IsTabListDefined = true;
        }




        /* обработчик события нажатия на вкладку
         */
        private void StandartTabInvoker_Click(object sender, RoutedEventArgs e)
        {
            MenuItem tabInvoker = sender as MenuItem;
            
            OnTabInvoked(this, TabInvokerType[tabInvoker]);
            
            if (tabInvoker.IsChecked == true)
            {
                if (!TabCreated(TabInvokerType[tabInvoker]))
                {
                    CreateNewStandartTab(tabInvoker);
                }
                RiseStandartTab(tabInvoker);
            }
            else
            {
                HideStandartTab(tabInvoker);
            }
        }

        /* флаг для определения будет ли срабатывать метод HideSecondInstance для данного
         * экземпляра окна программы
         * если true - нет
         * устанавливается в StandartTabInvoker_Click
         */
        public bool IsUntoucheable = false;

        /* метод, подписанный на событие TabInvoked в статическом класса
         * если вызываемая вкладка уже открыта в других окнах - там она будет
         * закрыта перед открытием в этом окне
         */
        public void HideSecondInstance(Window senderWindow, Type type)
        {
            if (senderWindow == this) return;

            //if (IsUntoucheable) return;
            MenuItem tabInvoker = GetKeyByValue(type, TabInvokerType);
            if(tabInvoker.IsChecked)
            HideStandartTab(tabInvoker);
            tabInvoker.IsChecked = false;
        }

        /* обработчик нажатия на вкладку
         * вызывает в окне нов
         */
        private void TabButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TabButton tabButton = sender as TabButton;            
            SetNewActivePage(GetPage(tabButton));
            tabPanel.SetActiveTab(tabButton, OnTabDeactivated, OnTabBecomeActive);
        }

        /* обработчик нажатия на кнопку закрытия вкладки на самой вкладке
         * при нажатии вкладка пропадает с ленты вкладок
         * а также снимается флажок с соответсвующего пункта в контекстном меню
         */
        private void TabButton_ClickClose(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("close button start work");

            // получаем TabButton как родителя Button закрывающей
            Button btn = sender as Button;
            DependencyObject obj = btn;
            TabButton tabButton;
            while (true)
            {
                obj = VisualTreeHelper.GetParent(obj);
                if (obj.GetType() == typeof(TabButton))
                {
                    tabButton = obj as TabButton;
                    break;
                }
            }

            MenuItem tabInvoker = GetMenuItem(tabButton);

            tabInvoker.IsChecked = false;
            HideStandartTab(tabInvoker);

            e.Handled = true;
        }

        /* обработчик нажатия на псевдо-вкладку - ModelPage -
         * скрывает видимый табличный интерфейс, ленту вкладок и саму кнопку, вызывающую
         * страницу администрирования модели
         * и эту же страницу делает видимой на всё окно
         */
        private void ModelTabButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ActivePage != null) ActivePage.Visibility = Visibility.Collapsed;
            tabPanel.Visibility = Visibility.Collapsed;
            modelTabButton.Visibility = Visibility.Collapsed;

            mainGrid.Children.Add(ModelAdminPage);
            Grid.SetColumn(ModelAdminPage, 0);
            Grid.SetColumnSpan(ModelAdminPage, 2);
            Grid.SetRow(ModelAdminPage, 0);
            Grid.SetRowSpan(ModelAdminPage, 2);

            ModelAdminPage.IsActive = true;
        }

        /* метод, передающийся с делегатом в экземпляр типа ModelPage - страница администрирования модели
         * выполняет действия по скрытию ModelPage и возврату табличных видов с лентой вкладок
         */
        protected void OnModelPageHiding()
        {
            mainGrid.Children.Remove(ModelAdminPage);
            if (ActivePage != null) ActivePage.Visibility = Visibility.Visible;
            tabPanel.Visibility = Visibility.Visible;
            modelTabButton.Visibility = Visibility.Visible;
            ModelAdminPage.IsActive = false;
        }

        /* метод для изменения состояния вкладки, которая становится активной
         */
        protected void OnTabBecomeActive(UserControl tabControl)
        {
            TabButton tab = tabControl as TabButton;
            tab.IsTabActive = true;
            tab.Background = (Brush)(new BrushConverter().ConvertFrom("Red"));
            tab.BottomLineBrush = (Brush)(new BrushConverter().ConvertFrom("Red"));
            tab.CloseButtonBrush = (Brush)(new BrushConverter().ConvertFrom("Red"));
        }

        /* метод для изменения состояния вкладки, которая перестаёт быть активной
         */
        protected void OnTabDeactivated(UserControl tabControl)
        {
            TabButton tab = tabControl as TabButton;
            tab.IsTabActive = false;
        }





        /* Классы страниц имеют пользовательский атрибут.
         * Через него, в зависимости от типа страницы, можно получить
         * название окна, которое должно быть отображено на вкладке
         * и в названии пункта меню
         */
        private string GetPageHeader(Type pageType)
        {
            string header = "NONAME";
            object[] pageAttributes = pageType.GetCustomAttributes(false);
            foreach (var attribute in pageAttributes)
            {
                if(attribute.GetType() == typeof(PageAttribute))
                header = (attribute as PageAttribute).Header;
            }
            return header;
        }

        /* сделать активной и видимой в окне заданную страницу
         */
        public void SetNewActivePage(UserControl strallanPage)
        {            
            if (ActivePage != null)
            {
                mainGrid.Children.Remove(activePage);
            }            
            ActivePage = strallanPage;

            mainGrid.Children.Add(strallanPage);
            Grid.SetColumn(strallanPage, column);
            Grid.SetRow(strallanPage, row);
            Grid.SetColumnSpan(strallanPage, columnSpan);
            Grid.SetRowSpan(strallanPage, rowSpan);

            if (ModelAdminPage.IsActive && IsMainWindow)
            {
                ActivePage.Visibility = Visibility.Collapsed;
            }
            else
            {
                ActivePage.Visibility = Visibility.Visible;
            }
        }



        /* добавление новой вкладки на ленту вкладок
         */
        //public TabButton AddNewStandartTab(MenuItem tabInvoker)
        //{
        //    Type pageType = GetPageType(tabInvoker);
        //    UserControl page = Activator.CreateInstance(pageType) as UserControl;
        //    TabButton tab = new TabButton()
        //    {
        //        TabNameText = GetPageHeader(pageType),
        //        Foreground = new SolidColorBrush(Colors.Wheat)
        //    };
        //    tab.MouseLeftButtonUp += TabButton_MouseLeftButtonUp;
        //    tab.tabCloseButton.Click += TabButton_ClickClose;

        //    var linker = (from record in TabList
        //                  where record.WindowPageType == TabInvokerType[tabInvoker]
        //                  select record).Single(); // 1

        //    linker.Tab = tab;
        //    linker.WindowPage = page;

        //    tabPanel.AddNewTab(tab); 
        //    tabPanel.SetActiveTab(tab, OnTabDeactivated, OnTabBecomeActive);
        //    SetNewActivePage(page); // 2
        //    return tab;

        //    /* 1. установка связи между вкладкой и страницей программы
        //     * 2. добавление вкладки на ленту
        //     *    установка соответствующей активной страницы
        //     * 3. возврат страницы - нужно для добавления в словарь соответствия
        //     *    см. обработчик нажатия на вкладку
        //     */
        //}

        /* вызов на ленту уже существующей вкладки 
         * и в окно - уже существующей странцы программы
         */
        public void RiseStandartTab(MenuItem tabInvoker)
        {
            TabButton tab = GetTab(tabInvoker);
            try
            {
                tabPanel.AddNewTab(tab); // 1                
            }
            catch
            {

            }
            tab.MouseLeftButtonUp += TabButton_MouseLeftButtonUp;
            tab.tabCloseButton.Click += TabButton_ClickClose;
            SetNewActivePage(GetPage(tab));
            tabPanel.SetActiveTab(tab, OnTabDeactivated, OnTabBecomeActive); // 2

            /* 1. могут быть исключения при смене вкладок во время
             *    закрытия активной вкладки - если вкладка уже есть на ленте
             * 2. помечаем вкладку как активную в ленте
             *    внутри метод вызываются ещё два делегата
             *    значения которым присвоены в конструкторе этого окна
             *    - метод для обработки состояния вкладки, перестающей быть активной
             *    OnTabDeactivated
             *    - метод для обработки состояния вкладки, становящейся активной
             *    OnTabBecomeActive
             */
        }

        /* создание новой вкладки
         */
        public void CreateNewStandartTab(MenuItem tabInvoker)
        {
            Type pageType = TabInvokerType[tabInvoker];

            UserControl page = Activator.CreateInstance(pageType) as UserControl;
            TabButton tab = new TabButton()
            {
                TabNameText = GetPageHeader(pageType),
                Foreground = new SolidColorBrush(Colors.Wheat)
            };


            TabAndPageLinker linker = (from record in TabList
                          where record.WindowPageType == pageType
                          select record).Single();

            linker.Tab = tab;
            linker.WindowPage = page;
            

            // 1
            if (tabInvoker.IsChecked == false) tabInvoker.IsChecked = true; // 2

            /* 1. добавляем соответсвие пункта меню и вкладки на ленте
             * 2. задаём значение для MenuItem равное true в случае если 
             *    метод вызывается программно, а не по нажатию на пункт меню
             */
        }

        /* выполняет действия по скрытию вкладки на ленте 
         * и скрытию активного окна
         * а также по открытию соседней вкладки
         * и соответствующей страницы (если есть)
         */
        public void HideStandartTab(MenuItem tabInvoker)
        {            
            TabButton tab = GetTab(tabInvoker); // 1

            tab.MouseLeftButtonUp -= TabButton_MouseLeftButtonUp;
            tab.tabCloseButton.Click -= TabButton_ClickClose;

            bool wasActive = tabPanel.IsActiveTab(tab);            
            int nextTabIndex = tabPanel.GetTabIndex(tab); //2
            tabPanel.RemoveTab(tab); //3
            if (GetPageType(tabInvoker) == ActivePage.GetType()) //4
            {
                mainGrid.Children.Remove(activePage);
            }
            //5
            if (wasActive && tabPanel.TabCount > 0) // 6
            {
                if (tabPanel.TabCount == nextTabIndex) nextTabIndex--; // 7
                if (tabPanel.TabCount == 1) nextTabIndex = 0; // 8
                TabButton newActiveTab = tabPanel.GetTab(nextTabIndex) as TabButton; //9
                foreach (var record in TabList)
                {
                    if (record.Tab == newActiveTab)
                    {
                        RiseStandartTab(GetKeyByValue(record.WindowPageType, TabInvokerType)); // 10                     
                        break;
                    }
                }
            }

            /* 1. из словаря соответствия пунктов меню и вкладок получаем вкладку
             * 2. получаем индекс вкладки в ленте вкладок
             * 3. удаляем вкладку с ленты вкладок при этом она остаётся в словаре главного окна
             * 4. если страница, соответствующая вкладке, является на данный момент
             *    активной, то есть видимой, делаем её невидимой, удаляя из списка
             *    дочерних объектов главного грида окна (или страницы)
             * 5. вкладка удалена. Если она не была активной, или если была единственной в ленте 
             *    иных действий не требуется, иначе выполняется следующий блок операций.
             *    он нужен для присвоения статуса активности соседней от закрываемой вкладке
             * 6. если закрываемая вкладка является активной и в списке дочерних объектов ленты вкладок
             *    есть хотя бы одна в вкладка...
             * 7. если число элементов в списке дочерних объектов ленты вкладок равно ранее вычисленному индексу
             *    а это бывает, если удаляется последняя вкладка с ленты...
             * 8. перекрытие прошлого условия: если число элементов в списке дочерних объектов ленты вкладок равно
             *    еденице, а такое бывает в случае когда на ленте после удаление остаётся всего одна вкладка -
             *    если этого не сделать, то для последней вкладки не будет выведена соответствующая страница
             * 9. по индексу находим соседнюю от закрываемой вкладку из ленты
             * 10. вызываем метод активации созданной ранее вкладки и появления на экране соответствующей страницы
             */
        }
    }
}
