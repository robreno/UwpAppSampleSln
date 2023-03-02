using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UwpSample.Converters.ValueConverters
{
    public sealed class ScrollViewerArgsConverter : IValueConverter
    {
        /// <summary>
        /// Value converter that translates true to <see cref="Visibility.Visible"/> and false to
        /// <see cref="Visibility.Collapsed"/>.
        /// </summary>
        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            var args = (RoutedEventArgs)value;
            string target = (string)parameter;
            if (args.OriginalSource != null) return args;
            IScrollViewerLoaded item = new ScrollViewerLoaded(target)
            {
                Parameter = target
            };
            return item;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}