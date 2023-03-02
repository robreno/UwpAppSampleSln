using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

using UwpSample.Models.Interfaces;

namespace UwpSample.Models
{
    public interface IParagraph
    {
        int SequenceID { get; set; }
        string Name { get; set; }
        string Timespan { get; set; }
        TimeSpan StartMarker { get; set; }
        TimeSpan EndMarker { get; set; }
        CultureInfo Language { get; set; }
        Paragraph Paragraph { get; set; }
        string Style { get; set; }
        string Text { get; set; }
        List<IRun> Runs { get; set; }
        IVocabulary Vocabulary { get; set; }
    }
}
