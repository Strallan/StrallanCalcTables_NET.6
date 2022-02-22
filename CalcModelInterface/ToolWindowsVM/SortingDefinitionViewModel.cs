using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Strallan;
using static Strallan.Library;

namespace CalcTables.CoreInterface
{
    public partial class CalcModelTypes : ObservableObject
    {
        private ObservableCollection<SortingControlItem> sortingElements;
        public ObservableCollection<SortingControlItem> SortingElements
        {
            get
            {
                if (sortingElements == null)
                {
                    sortingElements = new ObservableCollection<SortingControlItem>();
                }
                return sortingElements;
            }
            set
            {
                sortingElements = value;
                OnPropertyChanged(nameof(SortingElements));
            }
        }

        #region Command - NewSortingConditionItem

        private RelayCommand newConditionCmd;
        public ICommand NewConditionCmd
        {
            get
            {
                if (newConditionCmd == null) newConditionCmd =
                         new RelayCommand(NewConditionExecute, NewConditionCanExec);
                return newConditionCmd;
            }
        }

        public void NewConditionExecute(object parameter)
        {
            if (SortingElements.Count == 0)
            {
                int numberOfColumns = NodeTable.ColCount;
                SortingControlItem item = new SortingControlItem();
                for (int i = 0; i < numberOfColumns; i++)
                {
                    item.ColumnsNames.Add(NodeTable.ColumnName(i));
                }
                item.CurrentItem = item.ColumnsNames[0];
                item.OnCurrentItemChange += RemoveSameConditions;
                SortingElements.Add(item);                
            }
            else
            {
                SortingControlItem item = new SortingControlItem();
                foreach(var columnName in SortingElements.Last().ColumnsNames)
                    item.ColumnsNames.Add(columnName);
                item.ColumnsNames.Remove(SortingElements.Last().CurrentItem);
                item.CurrentItem = item.ColumnsNames.First();
                item.OnCurrentItemChange += RemoveSameConditions;
                SortingElements.Add(item);
            }
        }

        public bool NewConditionCanExec(object paramter)
        {
            return SortingElements.Count < NodeTable.ColCount;
        }

        // при изменении значения в комбобоксе проводится проверка
        // по всем ниже лежащим элементам списка
        // строка с комбобоксом ниже, если значение выбранного совпадает
        // с вновь выбранным, удаляется, а другие нижележащие элементы перезаписываются
        private void RemoveSameConditions(string oldSelected, string newSelected)
        {
            var elementForRemove = from element in SortingElements
                                   where element.CurrentItem == newSelected
                                   select element;
            if (SortingElements.Last() == elementForRemove) return;

            if (elementForRemove.Count() > 0) SortingElements.Remove(elementForRemove.First());

            //TODO: как гарантрованно удалить экземпляры после отработки метода
            List<SortingControlItem> change = new List<SortingControlItem>();
            List<SortingControlItem> deleteList = new List<SortingControlItem>();

            foreach (var element in SortingElements)
            {
                if (!element.ColumnsNames.Contains(oldSelected))
                {
                    element.ColumnsNames.Add(oldSelected);
                    if(element.ColumnsNames.Contains(newSelected))
                        element.ColumnsNames.Remove(newSelected);
                    deleteList.Add(element);
                    change.Add(GetChangingItem(element));
                }
            }

            // всё, что лежит ниже элемента списка с изменяемым комбобоксом
            // будет вначале удалено
            foreach (var item in deleteList)
            {
                if (SortingElements.Contains(item)) SortingElements.Remove(item);
            }
            deleteList.Clear();

            // а затем на его место встанут новые элементы списка
            // что и отразится в интерфейсе
            foreach(var item in change)
            {
                SortingElements.Add(item);
            }
            change.Clear();
        }

        // после удаления и добавления в список доступных столбцов для сортировка в комбобоксе
        // нужно значения упорядочить по самому первому комбобоксу, иначе новое значение добавляется
        // в конец
        private SortingControlItem GetChangingItem(SortingControlItem element)
        {
            // сортировка по 
            IOrderedEnumerable<string> sorted = element.ColumnsNames
                .OrderBy(item => { return SortingElements.First().ColumnsNames.IndexOf(item); });
            //TODO: возможно, здесь стоит использовать клонирование?
            SortingControlItem newitem = new SortingControlItem();
            newitem.CurrentItem = element.CurrentItem;
            newitem.OnCurrentItemChange += RemoveSameConditions;
            foreach(string s in sorted)
            {
                newitem.ColumnsNames.Add(s);
            }
            return newitem;
        }

        #endregion

        #region Command - GetSortingExpression

        private RelayCommand getSortExpressionCmd;
        public ICommand GetSortExpressionCmd
        {
            get
            {
                if (getSortExpressionCmd == null) 
                    getSortExpressionCmd = new RelayCommand(GetSortExpressionExec, GetSortExpressionCanExec);
                return getSortExpressionCmd;
            }
        }

        public void GetSortExpressionExec(object commandParameter)
        {
            string s = "";

            foreach(var item in SortingElements)
            {
                s += (item.ByAscending ? "+" : "-") + item.CurrentItem + " ";
            }
            s.Trim();
            NodeTable.Sort(s);
            //NodeTable.Reset();

            MessageBox.Show(s);

        }

        public bool GetSortExpressionCanExec(object commandParameter)
        {
            return true;
        }

        #endregion

        #region Command - Cancel


        #endregion
    }

    public class SortingControlItem : ObservableObject
    {
        public delegate void SelectedChanging(string oldSelected, string newSelected);
        public SortingControlItem()
        {
            ColumnsNames = new ObservableCollection<string>();
            CurrentItem = string.Empty;
            byAscending = true;
            byDescending = false;
        }

        private string currentItem;
        public string CurrentItem
        {
            get
            {
                if (currentItem == null) currentItem = string.Empty;
                return currentItem;
            }
            set
            {
                string oldSelected = currentItem;
                if (OnCurrentItemChange != null)
                    OnCurrentItemChange.Invoke(oldSelected, value);
                currentItem = value;
            }
        }

        private bool byAscending;
        public bool ByAscending
        {
            get
            {
                return byAscending;
            }
            set
            {
                byAscending = value;
                OnPropertyChanged(nameof(ByAscending));

                if (AscToDscSeem) 
                {
                    AscToDscSeem = false;
                    ByDescending = byAscending ? false : true;
                    AscToDscSeem = true;
                }
            }
        }

        private bool AscToDscSeem = true;

        private bool byDescending;
        public bool ByDescending
        {
            get
            {
                return byDescending;
            }
            set
            {
                byDescending = value;                
                OnPropertyChanged(nameof(ByDescending));

                if (AscToDscSeem == true) 
                {
                    AscToDscSeem = false;
                    ByAscending = byDescending ? false : true;
                    AscToDscSeem = true;
                }
            }
        }

        public ObservableCollection<string> ColumnsNames { get; set; }
        public event SelectedChanging OnCurrentItemChange;
    }
}
