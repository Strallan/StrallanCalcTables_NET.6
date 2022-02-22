using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Microsoft.Win32;

using Strallan;
using static Strallan.Library;



namespace CalcModelInterface.Samples
{
    public partial class CalcModelTypes
    {
        //private void Load_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    // Диалог открытия файла
        //    OpenFileDialog d = new OpenFileDialog
        //    {
        //        // Список расширений
        //        Filter = Server.GetSupportedExtensionList(),

        //        // Начальная папка обзора
        //        InitialDirectory = Directory.GetCurrentDirectory(),

        //        // Файл и путь должны существовать
        //        CheckFileExists = true,
        //        CheckPathExists = true,

        //        // Один диалог - один файл
        //        Multiselect = false,

        //    };
        //    // Расширение по умолчанию - первое из списка
        //    d.DefaultExt = d.Filter.Split('|')[1];

        //    if (d.ShowDialog() == true)
        //    {
        //        // Загружаем модель в конце вызвается λ-функция
        //        Model.LoadFromFile(d.FileName, new FinishEvent((int Code, string Msg) =>
        //        {
        //            if (Code == 0) // нет ошибки
        //            {
        //                gc.Reset(); // таки Reset, он вызывает Filter, который и синхронизирует списки
        //                            // потом надо будет переделать на LoadSettings

        //                // пример доступа к отдельным объектам
        //                AddrList List = new AddrList(); // создаём список адресов
        //                var NL = Model.GetNodeList(); // получаем INodeList
        //                var IL = NL as IItemList; // получаем IItemList (С# не способен нормально наследовать COM-интерфейсы, это баг, а не фича 
        //                IL.Enum(List); // получаем список адресов

        //                StringBuilder sb = new StringBuilder(); // формируем вывод
        //                for (int i = 0, n = List.Count; i < n; i++) // по всему списку
        //                {
        //                    var Addr = List[i]; // адрес из списка
        //                    sb.Append(IL.GetId(Addr)); // идентификатор 
        //                    sb.Append(" ");
        //                    NL.GetGlobalCoord(Addr, out double X, out double Y, out double Z); // координаты
        //                    sb.Append(X.ToString());
        //                    sb.Append(" ");
        //                    sb.Append(Y.ToString());
        //                    sb.Append(" ");
        //                    sb.AppendLine(Z.ToString());
        //                }

        //                MessageBox.Show(sb.ToString()); // показали
        //            }
        //            else
        //                throw new Exception(Msg); // есть ошибка — бросаем исключение

        //        }));
        //    }

        //}
    }

    

}
