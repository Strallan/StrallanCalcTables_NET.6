using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Strallan;
using Strallan.Controls;
using static Strallan.Library;

namespace CalcTables.CoreInterface
{
    public partial class CalcModelTypes : ObservableObject
    {
        public void InitializeGridDataSources()
        {
            NodeTable = InitTable(Model.GetNodeList);
            
            MemberTable = InitTable(Model.GetMemberList);
        }

        public GridControl InitTable<T>(Func<T> itemsGetter)
        {
            var table = new GridControl();
            table.BindDataSource(itemsGetter.Invoke() as IGridDataSource);
            table.StartWaiting += GridStartWaiting;
            table.EndWaiting += GridEndWaiting;
            return table;
        }

        #region SyncTableSample

        public void GridStartWaiting(object sender, long tag)
        {
            if(syncCode == 0)
            {
                LockUI?.Invoke();
                syncCode = tag;
            }
            else
            {
                throw new Exception("Двойная синхронизация");
            }
        }

        public void GridEndWaiting(object sender, long tag, int errorCode, string errorMessage)
        {
            if(syncCode == tag)
            {
                UnlockUI?.Invoke();
                syncCode = 0;
            }
            if(errorCode != 0)
            {
                throw new Exception(errorMessage);
            }
        }

        public event Action LockUI;
        public event Action UnlockUI;

        private long syncCode = 0;

        #endregion

        public bool ControlLeft
        {
            get
            {
                return Library.ControlLeft;
            }
            set
            {
                Library.ControlLeft = value;
            }
        }

        public event Action ControlLeftChanged;

        private void OnControlLeftChanged()
        {
            ControlLeftChanged?.Invoke();
        }

        #region Классы таблиц

        public GridControl NodeTable
        {
            get;
            private set;
        }

        public GridControl MemberTable
        {
            get;
            private set;
        }

        #endregion
    }
}
