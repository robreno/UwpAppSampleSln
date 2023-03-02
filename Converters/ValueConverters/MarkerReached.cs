using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Converters.ValueConverters
{
    public class MarkerReached : IMarkerReached
    {
        private string _paragraphName;
        private string _marker;
        private string _request;
        public string ParagraphName { get { return _paragraphName; } set { _paragraphName = value; } }
        public string Marker { get { return _marker; } set { _marker = value; } }
        public string ActionRequest { get { return _request; } set { _request = value; } }
        public MarkerReached(string paragraphName, string marker, string request)
        {
            _paragraphName = paragraphName;
            _marker = marker;
            _request = request;
        }
    }
}
