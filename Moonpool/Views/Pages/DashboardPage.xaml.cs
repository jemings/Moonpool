using Microsoft.Win32;
using Moonpool.Models;
using Moonpool.ViewModels.Pages;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Text.Json;
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

        private string imageFilePath = "";
        public ObservableCollection<Problem> problemCollection = [];

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            this.DataContext = this;

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
                imageFilePath = openFileDialog.FileName;
                DisplayImageAndHash(imageFilePath);
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
            Console.WriteLine("Save");

            Console.WriteLine($"{SubjectsComboBox.Text}");
            Console.WriteLine($"{ChaptersComboBox.Text}");
            Console.WriteLine($"{imageFilePath}");
            Console.WriteLine($"{AnswerBox.Text}");

            var problem = new Problem(
                SubjectsComboBox.Text,
                ChaptersComboBox.Text,
                imageFilePath,
                AnswerBox.Text,
                WeightBox.Text
                );

            problemCollection.Add(problem);
            SaveProblemCollection();
        }

        private void SaveProblemCollection()
        {
            var json = JsonSerializer.Serialize(problemCollection, new JsonSerializerOptions
            {
                WriteIndented = true // JSON 출력 포맷을 정렬된 방식으로 설정합니다.
            });

            string filePath = App.DatabasePath + "\\database.json";
            File.WriteAllText(filePath, json);
        }

        private bool IsImageBoxEmpty()
        {
            if (ImageBox.Document.Blocks.Count == 0) return true;
            TextPointer startPointer = ImageBox.Document.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward);
            TextPointer endPointer = ImageBox.Document.ContentEnd.GetNextInsertionPosition(LogicalDirection.Backward);
            return startPointer.CompareTo(endPointer) == 0;
        }

        private void CommandSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(SubjectsComboBox.Text) && !string.IsNullOrEmpty(ChaptersComboBox.Text) && !IsImageBoxEmpty() && !string.IsNullOrEmpty(AnswerBox.Text) && !string.IsNullOrEmpty(WeightBox.Text) && decimal.TryParse(WeightBox.Text, out _);
        }
    }
}
