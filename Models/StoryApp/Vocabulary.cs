using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UwpSample.Models.Interfaces;

namespace UwpSample.Models
{
    public class Vocabulary : IVocabulary
    {
        public int SequenceID { get; set; }
        public CultureInfo Language { get; set; }
        public List<ITerm> Terms { get; set; }
        public Vocabulary() { }
        public Vocabulary(int sequenceId, string lang, List<ITerm> terms)
        {
            SequenceID = sequenceId;
            Language = new CultureInfo(lang);
            Terms = terms;
        }
    }
}
