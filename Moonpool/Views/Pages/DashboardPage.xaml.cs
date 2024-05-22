using Microsoft.Win32;
using Moonpool.Models;
using Moonpool.ViewModels.Pages;
using System.Collections.Specialized;
using System.Configuration;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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

        private void ImageBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                DisplayImageAndHash(filePath);
            }
        }

        private void DisplayImageAndHash(string filePath)
        {
            var hash = Problem.GetImageHash(Problem.GetImageByte(filePath));
            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath);
            bitmap.DecodePixelWidth = 30;  // 작은 크기로 이미지 디코딩
            bitmap.EndInit();

            var image = new System.Windows.Controls.Image();
            image.Source = bitmap;
            image.Width = 30;
            image.Height = 30;

            ImageBox.Document.Blocks.Clear();
            InlineUIContainer container = new(image);
            Paragraph paragraph = new(container);
            paragraph.Inlines.Add(new Run(" " + hash));
            ImageBox.Document.Blocks.Add(paragraph);
        }

        private void ImageBox_Drop(object sender, DragEventArgs e)
        {

        }

        private void CommandSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
