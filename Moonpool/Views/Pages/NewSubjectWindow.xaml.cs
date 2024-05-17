using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Moonpool.Views.Pages
{
    /// <summary>
    /// NewSubjectWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NewSubjectWindow : Window
    {
        public string? NewSubject { get; private set; }

        public NewSubjectWindow() => InitializeComponent();

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NewSubject = NewSubjectTextBox.Text;
            if (!string.IsNullOrEmpty(NewSubject))
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid subject.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
