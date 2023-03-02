using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Models
{
    public class TokenOccurrence
    {
        public int DocumentID { get; set; }
        public int SequenceID { get; set; }
        public int DocumentPosition { get; set; }
        public int TermPosition { get; set; }
        public string ParagraphID { get; set; }
    }
}
