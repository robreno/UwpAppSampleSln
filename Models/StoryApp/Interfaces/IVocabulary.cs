using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Models.Interfaces
{
    public interface IVocabulary
    {
        public int SequenceID { get; set; }
        public CultureInfo Language { get; set; }
        public List<ITerm> Terms { get; set; }
    }
}
