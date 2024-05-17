using Moonpool.ViewModels.Pages;
using System.Collections.Specialized;
using System.Configuration;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using Wpf.Ui.Controls;

namespace Moonpool.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            LoadSubjects();
            SubjectsComboBox.Items.Add("New");  // Add "New" item
        }

        private void LoadSubjects()
        {
            var subjectsSection = ConfigurationManager.GetSection("Subjects") as NameValueCollection;
            if (subjectsSection != null)
            {
                string?[] array = subjectsSection.AllKeys ?? Array.Empty<string>();
                for (int i = 0; i < array.Length; i++)
                {
                    string? key = array[i];
                    if (key != null)
                    {
                        SubjectsComboBox.Items.Add(subjectsSection[key]);
                    }
                }
            }
        }
        private void SubjectsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubjectsComboBox.SelectedItem != null && SubjectsComboBox.SelectedItem.ToString() == "New")
            {
                var newSubjectWindow = new NewSubjectWindow();
                if (newSubjectWindow.ShowDialog() == true)
                {
                    var newSubject = newSubjectWindow.NewSubject;
                    if (!string.IsNullOrEmpty(newSubject))
                    {
                        // Add new subject to ComboBox
                        SubjectsComboBox.Items.Insert(SubjectsComboBox.Items.Count - 1, newSubject);
                        SubjectsComboBox.SelectedItem = newSubject;
                        // Add new subject to App.config
                        AddSubjectToConfig(newSubject);
                    }
                }
                else
                {
                    // Reset selection if dialog is cancelled
                    SubjectsComboBox.SelectedIndex = -1;
                }
            }
        }

        private void AddSubjectToConfig(string newSubject)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var configFilePath = config.FilePath;

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(configFilePath);

            XmlNode? configurationNode = xmlDoc.SelectSingleNode("configuration");
            if (configurationNode == null)
            {
                throw new InvalidOperationException("Configuration node not found in the config file.");
            }

            XmlNode? subjectsNode = configurationNode.SelectSingleNode("Subjects");
            if (subjectsNode == null)
            {
                subjectsNode = xmlDoc.CreateElement("Subjects");
                configurationNode.AppendChild(subjectsNode);
            }

            int newKey = subjectsNode.ChildNodes.Count;
            XmlElement newElem = xmlDoc.CreateElement("add");
            newElem.SetAttribute("key", newKey.ToString());
            newElem.SetAttribute("value", newSubject);
            subjectsNode.AppendChild(newElem);

            xmlDoc.Save(configFilePath);
            ConfigurationManager.RefreshSection("Subjects");
        }

        private void SubjectsComboBox_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var selectedItem = comboBox.SelectedItem as string;

                if (!string.IsNullOrEmpty(selectedItem) && selectedItem != "New")
                {
                    var result = System.Windows.MessageBox.Show($"Do you want to remove '{selectedItem}' from the list?", "Confirmation", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == System.Windows.MessageBoxResult.Yes)
                    {
                        comboBox.Items.Remove(selectedItem);
                        RemoveSubjectFromConfig(selectedItem);
                    }
                }
            }
        }

        private void RemoveSubjectFromConfig(string subjectToRemove)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var configFilePath = config.FilePath;

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(configFilePath);

            XmlNode? subjectsNode = xmlDoc.SelectSingleNode($"configuration/Subjects/add[@value='{subjectToRemove}']");

            if (subjectsNode != null)
            {
                subjectsNode.ParentNode?.RemoveChild(subjectsNode);
                xmlDoc.Save(configFilePath);
                ConfigurationManager.RefreshSection("Subjects");
            }
        }
    }
}
