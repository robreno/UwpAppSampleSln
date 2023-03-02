using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UwpSample.Models.Interfaces;

namespace UwpSample.Models
{
    public class Names : INames
    {
        public string RichTextBlock { get; set; }
        public string Paragraph { get; set; }
        public string Run { get; set; }

    }
}
