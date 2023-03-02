using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.ViewModels
{
    public class RichTextBlockElementDoubleTapped : IRichTextBlockElementDoubleTapped
    {
        private string _parameter;
        public string Parameter { get { return _parameter; } set { _parameter = value; } }
        public RichTextBlockElementDoubleTapped(string parameter)
        {
            _parameter = parameter;
        }
    }
}
