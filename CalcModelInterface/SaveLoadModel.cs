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
        public void Save(string pathToFile, Action<string> Finish)
        {
            long Tag = Server.Rdtsc();

            Model.SaveToFile(pathToFile, SaveMode.FullModel, new FinishEvent((int Code, string Msg) =>
            {
                GridEndWaiting(this, Tag, Code, Msg);
                Finish(Model.GetGuid());

            }));

            GridStartWaiting(this, Tag);
        }

        public void Load(string pathToFile, Action<string> Finish)
        {
            long Tag = Server.Rdtsc();

            Model.LoadFromFile(pathToFile, new FinishEvent((int Code, string Msg) =>
            {
                GridEndWaiting(this, Tag, Code, Msg);
                Finish(Model.GetGuid());

            }));

            GridStartWaiting(this, Tag);
        }

        public void AfterLoad()
        {
            NodeTable.Reset();
        }

 
    }
}
