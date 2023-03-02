using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.ViewModels
{
    public class AudioMediaEventArgs : EventArgs, IAudioMediaEventArgs
    {
        public string TargetName { get; set; }
        public string TargetType { get; set; }
        public string TargetJson { get; set; }
        public string Message { get; set; }
        public AudioMediaEventArgs() { }
        public AudioMediaEventArgs(string targetName, string targetType, string targetJson, string message)
        {
            TargetName = targetName;
            TargetType = targetType;
            TargetJson = TargetJson;
            Message = message;
        }
    }
}
