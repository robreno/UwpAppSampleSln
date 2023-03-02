using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

namespace UwpSample.Converters.ValueConverters
{
    public sealed class DoubleTappedArgsConverter : IValueConverter
    {
        /// <summary>
        /// Value converter that translates true to <see cref="Visibility.Visible"/> and false to
        /// <see cref="Visibility.Collapsed"/>.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var args = (DoubleTappedRoutedEventArgs)value;
            string controlName = string.Empty;
            string param = (string)parameter;
            string json = string.Empty;
            if (args.OriginalSource != null)
            {
                var obj = args.OriginalSource;
                if (obj.GetType().Name.Equals("TextBlock"))
                    controlName = ((TextBlock)obj).Name;
                if (obj.GetType().Name.Equals("RichTextBlock"))
                    controlName = ((RichTextBlock)obj).Name;
            }

            if (args == null) return value;
            IDoubleTappedArgs item = new DoubleTappedArgs(controlName, param, json)
            {
                TargetName = controlName,
                TargetType = param,
                TargetJson = json
            };
            return item;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
