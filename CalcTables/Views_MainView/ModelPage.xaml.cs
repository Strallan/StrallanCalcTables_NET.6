using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.IO;
using Microsoft.Win32;

namespace CalcTables
{
    /// <summary>
    /// Логика взаимодействия для ModelPage.xaml
    /// </summary>
    public partial class ModelPage : UserControl
    {
        public ModelPage()
        {
            InitializeComponent();
            stubText.Text = this.GetType().Name;
        }

        public Action ReturnToTabView;
        public Action LoadState;
        public Action<string> SaveState;

        private void returnToTabView_Click(object sender, RoutedEventArgs e)
        {
            ReturnToTabView.Invoke();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = ((App)App.Current).SupportedExtensions;
            fileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            fileDialog.CheckFileExists = true;
            fileDialog.CheckPathExists = true;
            fileDialog.Multiselect = false;
            fileDialog.DefaultExt = fileDialog.Filter.Split('|')[1];

            if (fileDialog.ShowDialog() == true)
            {
                foreach (var win in App.Current.Windows)
                {
                    if (win is TabPanelWindow)
                    {
                        var downcast = (TabPanelWindow)win;
                        foreach (var kvp in downcast.TabInvokerType)
                        {
                            kvp.Key.IsChecked = false;
                        }

                        downcast.tabPanel.tabContainer.Children.Clear();
                        downcast.mainGrid.Children.Remove(downcast.ActivePage);
                        downcast.ActivePage = null;
                        if (downcast.IsMainWindow) continue;
                        downcast.Close();
                    }
                }

                foreach (var item in App.TabList)
                {
                    item.Tab = null;
                    if (item.WindowPage is ICommonControlService)
                    {
                        (item.WindowPage as ICommonControlService).DetachSideControls();
                    }
                    item.WindowPage = null;
                }


                App.modelPath = fileDialog.FileName;
                ((App)App.Current).OnLoadModel.Invoke(App.modelPath, (string modelGuid) =>
                {

                    FileInfo modelFileInfo = new FileInfo(App.modelPath);
                    string xmlFileName = modelFileInfo.FullName.Replace(modelFileInfo.Extension, ".smcfg");
                    FileInfo xmlFileInfo = new FileInfo(xmlFileName);

                    if (xmlFileInfo.Exists)
                    {
                        XDocument xmlDoc = XDocument.Load(xmlFileInfo.FullName);
                        bool hasModelGuidNode = xmlDoc.Descendants()
                            .Where(node => node.Name == "ModelGUID")
                            .Count() > 0;
                        App.appConfigPath = xmlFileInfo.FullName;
                        LoadState.Invoke();
                        ((App)App.Current).AfterLoad.Invoke();
                    }
                    else
                    {
                        DirectoryInfo modelDirectory = modelFileInfo.Directory;
                        var allConfigFiles = modelDirectory.GetFiles()
                            .Where(file => file.Extension == ".smcfg");

                        foreach (var file in allConfigFiles)
                        {
                            XDocument configDoc = XDocument.Load(file.FullName);
                            var modelGuidValues = configDoc.Descendants()
                                .Where(node => node.Name == "ModelGUID")
                                .Select(node => { return node.Value; });
                            if (modelGuidValues.Count() > 0 && modelGuidValues.FirstOrDefault() == modelGuid)
                            {
                                App.appConfigPath = file.FullName;
                                LoadState.Invoke();
                                ((App)App.Current).AfterLoad.Invoke();
                            }
                        }
                    }
                });
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = ((App)App.Current).SupportedExtensions;
            fileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            fileDialog.OverwritePrompt = true;
            fileDialog.ValidateNames = true;
            fileDialog.DefaultExt = fileDialog.Filter.Split('|')[1];

            if (fileDialog.ShowDialog() == true)
            {
                App.modelPath = fileDialog.FileName;
                ((App)App.Current).OnSaveModel.Invoke(App.modelPath, (string modelGuid) =>
                {

                    FileInfo modelFileInfo = new FileInfo(App.modelPath);
                    string xmlFileName = modelFileInfo.FullName.Replace(modelFileInfo.Extension, ".smcfg");
                    FileInfo xmlFileInfo = new FileInfo(xmlFileName);

                    if (xmlFileInfo.Exists)
                    {
                        XDocument xmlDoc = XDocument.Load(xmlFileInfo.FullName);
                        bool hasModelGuidNode = xmlDoc.Descendants()
                            .Where(node => node.Name == "ModelGUID")
                            .Count() > 0;
                        App.appConfigPath = xmlFileInfo.FullName;
                        SaveState.Invoke(modelGuid);
                    }
                    else
                    {
                        DirectoryInfo modelDirectory = modelFileInfo.Directory;
                        var allConfigFiles = modelDirectory.GetFiles()
                            .Where(file => file.Extension == ".smcfg");

                        foreach (var file in allConfigFiles)
                        {
                            XDocument configDoc = XDocument.Load(file.FullName);
                            var modelGuidValues = configDoc.Descendants()
                                .Where(node => node.Name == "ModelGUID")
                                .Select(node => { return node.Value; });
                            if (modelGuidValues.Count() > 0 && modelGuidValues.FirstOrDefault() == modelGuid)
                            {
                                App.appConfigPath = file.FullName;
                                SaveState.Invoke(modelGuid);
                                return;
                            }
                        }
                        FileStream createdXml = xmlFileInfo.Create();
                        createdXml.Close();
                        App.appConfigPath = xmlFileInfo.FullName;
                        SaveState.Invoke(modelGuid);
                    }
                });
            }
        }

        public bool IsActive
        {
            get;
            set;
        }
    }
}
