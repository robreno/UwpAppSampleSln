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
    public class MediaElementStateChanged : IMediaElementStateChanged
    {
        private string _name;
        private string _primaryLanguage;
        private CultureInfo _culture;
        private Uri _sourceUri;
        private MediaElementState _currentState;
        public string Name { get { return _name; } set { _name = value; } }
        public string PrimaryLanguage { get { return _primaryLanguage; } set { _primaryLanguage = value; } }
        public CultureInfo Culture { get { return _culture; } set { _culture = value; } }
        public Uri Source { get { return _sourceUri; } set { _sourceUri = value; } }
        public MediaElementState CurrentState { get { return _currentState; } set { _currentState = value; } }
        public MediaElementStateChanged() { }
        public MediaElementStateChanged(string name, string language, CultureInfo culture, Uri source, MediaElementState currentState)
        {
            _name = name;
            _primaryLanguage = language;
            _culture = culture;
            _sourceUri = source;
            _currentState = currentState;
        }
    }
}
