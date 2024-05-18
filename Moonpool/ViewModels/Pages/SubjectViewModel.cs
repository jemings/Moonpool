using Moonpool.Models;
using Moonpool.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Moonpool.ViewModels.Pages
{
    public partial class SubjectViewModel : ObservableObject
    {
        private string? selectedSubject;
        private string? selectedDetail;

        public SubjectViewModel()
        {
            Subjects = new ObservableCollection<string>();
            Details = new ObservableCollection<string>();
            LoadSubjects();
        }

        [ObservableProperty]
        private ObservableCollection<string> subjects;

        [ObservableProperty]
        private ObservableCollection<string> details;

        public string? SelectedSubject
        {
            get => selectedSubject;
            set
            {
                if (SetProperty(ref selectedSubject, value))
                {
                    if (selectedSubject == "New")
                    {
                        AddNewSubject();
                    }
                    else if (selectedSubject != null)
                    {
                        LoadDetails(selectedSubject);
                    }
                }
            }
        }

        public string? SelectedDetail
        {
            get => selectedDetail;
            set
            {
                if (SetProperty(ref selectedDetail, value))
                {
                    if (selectedDetail == "New")
                    {
                        AddNewDetail();
                    }
                }
            }
        }

        private void LoadSubjects()
        {
            Subjects.Clear();
            if (ConfigurationManager.GetSection("Subjects") is NameValueCollection subjectsSection)
            {
                foreach (var key in subjectsSection.AllKeys)
                {
                    var value = subjectsSection[key];
                    if (!string.IsNullOrEmpty(value))
                    {
                        Subjects.Add(value!); // null 확인 후 강제 언래핑
                    }
                }
                //Subjects.Add("New");
            }
        }

        private void LoadDetails(string subject)
        {
            Details.Clear();
            if (ConfigurationManager.GetSection(subject) is NameValueCollection section)
            {
                foreach (var key in section.AllKeys)
                {
                    var value = section[key];
                    if (!string.IsNullOrEmpty(value))
                    {
                        Details.Add(value!); // null 확인 후 강제 언래핑
                    }
                }
                Details.Add("New");
            }
            else
            {
                // 새로 추가된 Subject에 대해 Details를 초기화하고 "New"를 추가합니다.
                Details.Add("New");
            }
        }

        private void AddNewSubject()
        {
            var newSubjectWindow = new NewSubjectWindow();
            if (newSubjectWindow.ShowDialog() == true)
            {
                var newSubject = newSubjectWindow.NewSubject;
                if (!string.IsNullOrEmpty(newSubject))
                {
                    Subjects.Insert(Subjects.Count - 1, newSubject!);
                    AddNewEntryToConfig("Subjects", newSubject!);
                    SelectedSubject = newSubject;

                    // 새로 추가된 Subject에 대한 Details를 초기화합니다.
                    LoadDetails(newSubject);
                }
            }
        }

        private void AddNewDetail()
        {
            var newSubjectWindow = new NewSubjectWindow();
            if (newSubjectWindow.ShowDialog() == true)
            {
                var newDetail = newSubjectWindow.NewSubject;
                if (!string.IsNullOrEmpty(newDetail) && !string.IsNullOrEmpty(SelectedSubject))
                {
                    Details.Insert(Details.Count - 1, newDetail!);
                    AddNewEntryToConfig(SelectedSubject!, newDetail!);
                    SelectedDetail = newDetail;
                }
            }
        }

        private void AddNewEntryToConfig(string sectionName, string newValue)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(config.FilePath);

            var sectionNode = xmlDoc.SelectSingleNode($"//{sectionName}");
            if (sectionNode != null)
            {
                var addElement = xmlDoc.CreateElement("add");
                addElement.SetAttribute("key", sectionNode.ChildNodes.Count.ToString());
                addElement.SetAttribute("value", newValue);
                sectionNode.AppendChild(addElement);

                xmlDoc.Save(config.FilePath);
                ConfigurationManager.RefreshSection(sectionName);
            }
        }
    }
}
