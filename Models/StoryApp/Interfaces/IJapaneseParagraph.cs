using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UwpSample.Models.Interfaces;
using Windows.UI.Xaml.Documents;


namespace UwpSample.Models
{
    public interface IJapaneseParagraph : IParagraph
    {
        string RubyText { get; set; }
        string PauseText { get; set; }
        char PauseMarker { get; set; }
        bool IsPauseText { get; set; }
        bool IsPauseTextVisible { get; set; }
        bool IsRubyText { get; set; }
        bool IsRubyTextVisible { get; set; }
    }
}
