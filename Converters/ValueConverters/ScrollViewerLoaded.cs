using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Converters.ValueConverters
{
    public class ScrollViewerLoaded : IScrollViewerLoaded
    {
        private string _parameter;
        public string Parameter { get { return _parameter; } set { _parameter = value; } }
        public ScrollViewerLoaded(string parameter)
        {
            _parameter = parameter;
        }
    }
}
