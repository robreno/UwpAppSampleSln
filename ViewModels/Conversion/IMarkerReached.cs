using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.ViewModels
{
    public interface IMarkerReached
    {
        string ParagraphName { get; set; }
        string Marker { get; set; }
        string ActionRequest { get; set; }
    }
}
