using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Models.Interfaces
{
    public interface INames
    {
        string RichTextBlock { get; set; }
        string Paragraph { get; set; }
        string Run { get; set; }
    }
}
