using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonpool.Models
{
    public class Subject(string subject)
    {
        public string Name { get; } = subject;
        public ArrayList Chapters = [];
    }
}
