using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonpool.ViewModels.Pages
{
    public partial class ProblemViewModel : ObservableObject
    {
        [ObservableProperty]
        public required string subjectName;

        [ObservableProperty]
        public required string chapterName;

        [ObservableProperty]
        public required string imageHash;

        [ObservableProperty]
        public required string answer;

        [ObservableProperty]
        public required decimal correctRate;

        [ObservableProperty]
        public bool isEditing;
    }
}
