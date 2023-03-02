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
    public sealed class MediaElementStateChangedArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            IMediaElementStateChanged item = null;
            string type = value.GetType().Name;
            var args = (RoutedEventArgs)value;

            MediaElement me = null;
            if (parameter != null)
            { 
                me = (MediaElement)parameter;
                item = new MediaElementStateChanged()
                {
                    Name = me.Name,
                    PrimaryLanguage = me.Language,
                    Culture = new System.Globalization.CultureInfo(language),
                    Source = me.Source,
                    CurrentState = me.CurrentState,
                };
            }
            return item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
