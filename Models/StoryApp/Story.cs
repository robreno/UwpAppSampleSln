using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Models
{
    public class Story
    {
        public int ID { get; set; }
        public CultureInfo PrimaryLanguage { get; set; }
        public CultureInfo SecondaryLanguage { get; set; }
        public string PriamryLanguageTitle { get; set; }
        public string SecondaryLanguageTitle { get; set; }
        public List<IParagraph> Paragraphs { get; set; }
        public List<ParagraphTuple> ParagraphTuples { get; set; }
    }
}
