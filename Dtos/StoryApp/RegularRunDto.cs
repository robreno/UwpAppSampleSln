using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UwpSample.Dtos.Interfaces;

namespace UwpSample.Dtos
{
    public class RegularRunDto : IRunDto
    {
        public RegularRunDto() { }
        public RegularRunDto(string text, string fontStyle)
        {
            Text = text;
            FontStyle = fontStyle;
        }
        public string Text { get; set; }
        public string FontStyle { get; set; }
    }
}
