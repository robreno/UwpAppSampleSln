using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UwpSample.Models.Interfaces;

namespace UwpSample.Models
{
    public class ParagraphTuple
    {
        public int SequenceID { get; set; }
        public CultureInfo PrimaryLanguage { get; set; }
        public IParagraph PrimaryParagraph { get; set; }
        public CultureInfo SecondaryLanguage { get; set; }
        public IParagraph SecondaryParagraph { get; set; }
        public ParagraphTuple() { }
        public ParagraphTuple(string primaryLanguage, IParagraph primaryParagraph, 
                              string secondaryLanguage, IParagraph secondaryParagraph)
        {
            PrimaryLanguage = new CultureInfo(primaryLanguage);
            PrimaryParagraph = primaryParagraph;
            SecondaryLanguage = new CultureInfo(secondaryLanguage);
            SecondaryParagraph = secondaryParagraph;
        }
    }
}
