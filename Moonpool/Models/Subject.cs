﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonpool.Models
{
    public partial class Subject() : ObservableObject
    {
        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        public IList<Chapter> chapters = [];
    }
}
