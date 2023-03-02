using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace UwpSample.Converters.ValueConverters
{
    /// <summary>
    /// Value converter that translates true to <see cref="Visibility.Visible"/> and false to
    /// <see cref="Visibility.Collapsed"/>.
    /// </summary>
    public sealed class MediaElementArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string type = value.GetType().Name;
            if (type == "IMediaElementEnded")
            {
                var param = (string)parameter;
                if (param == null) return value;
                IMediaElementEnded item = new MediaElementEnded(param)
                {
                    Parameter = param
                };
                return item;
            }
            else if (type == "TimelineMarkerRoutedEventArgs")
            {
                var args = (TimelineMarkerRoutedEventArgs)value;
                MediaElement me = (MediaElement)parameter;
                var currentState = me.CurrentState;

                string name = args.Marker.Text;
                string marker = args.Marker.Time.ToString();
                string request = "Stop";

                IMarkerReached item = new MarkerReached(name, marker, request)
                {
                    ParagraphName = name,
                    Marker = marker,
                    ActionRequest = request
                };
                return item;
            }
            else if (type == "RoutedEventArgs")
            {
                var args = (RoutedEventArgs)value;
                var param = (string)parameter;
                if (param == null) return value;
                IMediaElementStatusChanged item = new MediaElementStatusChanged(param)
                {
                    Parameter = param
                };
                return item;
            }
            else
            {
                var param = (string)parameter;
                if (param == null) return value;
                IMediaElementEnded item = new MediaElementEnded(param)
                {
                    Parameter = param
                };
                return item;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
