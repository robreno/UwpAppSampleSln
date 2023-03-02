using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UwpSample.Converters.ValueConverters
{
    public sealed class RoutedEventArgsConverter : IValueConverter
    {
        /// <summary>
        /// Value converter that translates true to <see cref="Visibility.Visible"/> and false to
        /// <see cref="Visibility.Collapsed"/>.
        /// </summary>
        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            var args = (RoutedEventArgs)value;
            if (args.OriginalSource != null) return args;
            return parameter;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}