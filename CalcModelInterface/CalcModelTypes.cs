using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Strallan;
using static Strallan.Library;

namespace CalcTables.CoreInterface
{
    public partial class CalcModelTypes : ObservableObject
    {
        public CalcModelTypes()
        {
            InitializeStrallanLibrary();
            InitializeGridDataSources();
            ComObject.ControlLeftChanged += OnControlLeftChanged;
        }

        public void OnClose()
        {
            ComObject.ControlLeftChanged -= OnControlLeftChanged;
            FinalizeStrallanLibrary();
        }

        public string GetSupportedExtensionList()
        {
            return Server.GetSupportedExtensionList();
        }

        // TEST
        public string GetModelGuid()
        {
            return Model.GetGuid();
        }

        //TEST
        //public string GetFirstNode()
        //{
        //    //string s = string.Empty;
        //    var s = Model.GetNodeList() as IItemList;
        //    ILongEnum
        //}
    }
}
