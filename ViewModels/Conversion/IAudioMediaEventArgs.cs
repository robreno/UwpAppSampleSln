using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.ViewModels
{
    public interface IAudioMediaEventArgs
    {
        public string TargetName { get; set; }
        public string TargetType { get; set; }
        public string TargetJson { get; set; }
        public string Message { get; set; }
    }
}
