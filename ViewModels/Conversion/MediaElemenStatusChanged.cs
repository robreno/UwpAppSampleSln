using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.ViewModels
{
    public class MediaElementStatusChanged : IMediaElementStatusChanged
    {
        private string _parameter;
        public string Parameter { get { return _parameter; } set { _parameter = value; } }
        public MediaElementStatusChanged(string parameter)
        {
            _parameter = parameter;
        }
    }
}
