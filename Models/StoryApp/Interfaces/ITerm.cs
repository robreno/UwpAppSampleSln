using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Models.Interfaces
{
    public interface ITerm
    {
        public string Index { get; set; }
        public string Text { get; set; }
        public string Meaning { get; set; }
        public string Explanation { get; set; }
        public string Example { get; set; }
    }
}
