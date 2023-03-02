using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UwpSample.Converters.ValueConverters
{
    public interface IMediaElementStateChanged
    {
        string Name { get; set; }
        string PrimaryLanguage { get; set; }
        CultureInfo Culture { get; set; }
        Uri Source { get; set; }
        MediaElementState CurrentState { get; set; }
    }
}
