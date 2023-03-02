using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UwpSample.Models.Interfaces;

namespace UwpSample.Models
{
    public class JapaneseRubyRun : IRun
    {
        public JapaneseRubyRun() { }
        public JapaneseRubyRun(string text, string fontStyle)
        {
            Text = text;
            FontStyle = fontStyle;
        }
        public string Text { get; set; }
        public string FontStyle { get; set; }
    }
}
