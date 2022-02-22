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
        private string filterRule;
        public string FilterRule
        {
            get
            {
                if (filterRule == null) filterRule = string.Empty;
                return filterRule;
            }

            set
            {
                filterRule = value;
                //if(!FilterRuleList.Contains(filterRule)) FilterRuleList.Add(filterRule);
                OnPropertyChanged(nameof(FilterRule));
            }
        }

        private ObservableCollection<string> filterRuleList;
        public ObservableCollection<string> FilterRuleList
        {
            get
            {
                if(filterRuleList == null) filterRuleList = new ObservableCollection<string>();
                return filterRuleList;
            }
            set
            {
                filterRuleList = value;
                OnPropertyChanged(nameof(FilterRuleList));
            }
        }


        #region COMMAND - Set New Filter Rule

        private RelayCommand newFilterRuleCmd;
        public ICommand NewFilterRuleCmd
        {
            get 
            {
                if (newFilterRuleCmd == null) newFilterRuleCmd = new RelayCommand(NewFilterRuleExec, NewFilterRuleCanExec);
                return newFilterRuleCmd;
            }
        }

        public void NewFilterRuleExec(object obj)
        {
            if (filterRule != null) NodeTable.Filter(filterRule);
            if (!FilterRuleList.Contains(filterRule)) FilterRuleList.Add(filterRule);
        }

        public bool NewFilterRuleCanExec(object obj)
        {
            return true;
        }

        #endregion


        #region COMMAND - Apply From Filter List

        private RelayCommand filterApplyCmd;
        public ICommand FilterApplyCmd
        {
            get
            {
                if (filterApplyCmd == null) filterApplyCmd = new RelayCommand(FilterApplyExec, FilterApplyCanExec);
                return filterApplyCmd;
            }
        }

        public void FilterApplyExec(object obj)
        {
            if (filterRule != null) NodeTable.Filter(filterRule);
        }

        public bool FilterApplyCanExec(object obj)
        {
            return true;
        }

        #endregion


        #region COMMAND - Reset Filter

        private RelayCommand resetTableFilterCmd;
        public ICommand ResetTableFilterCmd
        {
            get
            {
                if (resetTableFilterCmd == null) resetTableFilterCmd = new RelayCommand(ResetTableFilterExec, ResetTableFilterCanExec);
                return resetTableFilterCmd;
            }
        }

        public void ResetTableFilterExec(object obj)
        {
            NodeTable.Filter("");
        }

        public bool ResetTableFilterCanExec(object obj)
        {
            return true;
        }

        #endregion 
    }
}
