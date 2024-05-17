using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonpool.Models
{
    public class Chapter(string chapter)
    {
        public string Name { get; } = chapter;
        public ArrayList Questions = [];
    }
}
