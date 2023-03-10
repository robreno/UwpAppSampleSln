using System;

namespace UwpSample.Converters.ValueConverters
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public class BooleanVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null ? Visibility.Collapsed : ((bool)value ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
