using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonpool.Models
{
    public partial class SubjectList : ObservableObject
    {
        public ObservableCollection<string> Name = [];

        [ObservableProperty]
        private IList<Subject> subjects = [];
    }
}
