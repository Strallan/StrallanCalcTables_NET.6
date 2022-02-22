using System;
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
        //private RelayCommand saveModelCmd;
        //public ICommand SaveModelCmd
        //{
        //    get
        //    {
        //        if (saveModelCmd == null) saveModelCmd =
        //                 new RelayCommand(SaveModelExecute, SaveModelCanExec);
        //        return saveModelCmd;
        //    }
        //}

        //public void SaveModelExecute(object parameter)
        //{
        //    string filePath = string.Empty;
        //    if(GetModelFilepath != null)
        //    {
        //        filePath = GetModelFilepath.Invoke(Model.GetGuid());
        //        Model.SaveToFile(filePath, SaveMode.FullModel, finishProcess);
        //    }
        //}

        //public bool SaveModelCanExec(object paramter)
        //{
        //    return true;
        //}


        
        //private RelayCommand loadModelCmd;
        //public ICommand LoadModelCmd
        //{
        //    get
        //    {
        //        if (loadModelCmd == null) loadModelCmd =
        //                 new RelayCommand(LoadModelExecute, LoadModelCanExec);
        //        return loadModelCmd;
        //    }
        //}

        //public void LoadModelExecute(object parameter)
        //{
        //    string filePath = string.Empty;
        //    if (GetModelFilepath != null)
        //    {
        //        filePath = GetModelFilepath.Invoke(Model.GetGuid());
        //        Model.LoadFromFile(filePath, finishProcess);
        //    }
        //}

        //public bool LoadModelCanExec(object paramter)
        //{
        //    return true;
        //}



        //public Func<string, string> GetModelFilepath;
    }
}