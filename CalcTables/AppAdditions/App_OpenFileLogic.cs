using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using System.Reflection;
using Microsoft.Win32;

using Kodama;
using CalcTables.CoreInterface;


namespace CalcTables
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string modelPath;
        public static string appConfigPath;

        public string SupportedExtensions
        {
            get { return ViewModel.GetSupportedExtensionList(); }
        }

        public Action<string, Action<string>> OnSaveModel;
        public Action<string, Action<string>> OnLoadModel;
        public Action AfterLoad;
    }
}
