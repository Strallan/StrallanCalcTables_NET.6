using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Strallan;
using static Strallan.Library;

namespace CalcTables.CoreInterface
{
    public partial class CalcModelTypes : ObservableObject
    {
        private RelayCommand addNewNodeCmd;
        public ICommand AddNewNodeCmd
        {
            get
            {
                if (addNewNodeCmd == null) addNewNodeCmd =
                         new RelayCommand(AddNewNodeExecute, AddNewNodeCanExec);
                return addNewNodeCmd;
            }
        }

        public void AddNewNodeExecute(object parameter)
        {
            NodeTable.AddRow(NewNodeIdValue);
            int lastRowIndex = NodeTable.RowCount - 1;            
            NewNodeXValue = SetNewCoordinateValue(NewNodeXValue, "X", ref xOldValue);
            NewNodeYValue = SetNewCoordinateValue(NewNodeYValue, "Y", ref yOldValue);
            NewNodeZValue = SetNewCoordinateValue(NewNodeZValue, "Z", ref zOldValue);
            //delta = Double.Parse(NewNodeXValue) - oldValue;
            //oldValue = Double.Parse(NewNodeXValue);
            //NodeTable.SetCell(NodeTable.RowCount - 1, NodeTable.ColumnIndex("X"), NewNodeXValue);
            //NewNodeXValue = (Double.Parse(NewNodeXValue) + delta).ToString();
            //NodeTable.SetCell(NodeTable.RowCount - 1, NodeTable.ColumnIndex("Y"), NewNodeYValue);
            //NodeTable.SetCell(NodeTable.RowCount - 1, NodeTable.ColumnIndex("Z"), NewNodeZValue);
        }

        private string SetNewCoordinateValue(string newNodeValue, string columnMark, ref double oldValue)
        {
            double delta = 0;
            delta = Double.Parse(newNodeValue) - oldValue;
            oldValue = Double.Parse(newNodeValue);
            NodeTable.SetCell(NodeTable.RowCount - 1, NodeTable.ColumnIndex(columnMark), newNodeValue);
            return (Double.Parse(newNodeValue) + delta).ToString();            
        }



        public bool AddNewNodeCanExec(object paramter)
        {
            return true;
        }

        // устаревшее поле
        private string addingNodeName = "";
        public string AddingNodeName
        {
            get { return addingNodeName; }
            set
            {
                addingNodeName = value;
                OnPropertyChanged(nameof(AddingNodeName));
            }
        }

        private string newNodeIdValue = "A1";
        public string NewNodeIdValue
        {
            get { return newNodeIdValue; }
            set
            {
                newNodeIdValue = value;
                OnPropertyChanged(nameof(NewNodeIdValue));
            }
        }

        private double xOldValue = 0;
        private string newNodeXValue = "0";
        public string NewNodeXValue
        {
            get { return newNodeXValue; }
            set
            {
                newNodeXValue = value;
                OnPropertyChanged(nameof(NewNodeXValue));
            }
        }

        private double yOldValue = 0;
        private string newNodeYValue = "0";
        public string NewNodeYValue
        {
            get { return newNodeYValue; }
            set
            {
                newNodeYValue = value;
                OnPropertyChanged(nameof(NewNodeYValue));
            }
        }

        private double zOldValue = 0;
        private string newNodeZValue = "0";
        public string NewNodeZValue
        {
            get { return newNodeZValue; }
            set
            {
                newNodeZValue = value;
                OnPropertyChanged(nameof(NewNodeZValue));
            }
        }

        private bool newNodeInOrthoCS;
        public bool NewNodeInOrthoCS
        {
            get { return newNodeInOrthoCS; }
            set
            {
                newNodeInOrthoCS = value;
                OnPropertyChanged(nameof(NewNodeInOrthoCS));
            }
        }

        private bool newNodeInCylindricalCS;
        public bool NewNodeInCylindricalCS
        {
            get { return newNodeInCylindricalCS; }
            set
            {
                newNodeInCylindricalCS = value;
                OnPropertyChanged(nameof(NewNodeInCylindricalCS));
            }
        }

        private bool newNodeInSphericalCS;
        public bool NewNodeInSphericalCS
        {
            get { return newNodeInSphericalCS; }
            set
            {
                newNodeInSphericalCS = value;
                OnPropertyChanged(nameof(NewNodeInSphericalCS));
            }
        }

        public bool nodeGuessModeOn;
        public bool NodeGuessModeOn
        {
            get { return nodeGuessModeOn; }
            set
            {
                nodeGuessModeOn = value;
                OnPropertyChanged(nameof(nodeGuessModeOn));
            }
        }
    }
}
