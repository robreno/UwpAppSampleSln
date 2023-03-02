using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

namespace UwpSample.ViewModels
{
    public sealed class DoubleTappedArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var args = (DoubleTappedRoutedEventArgs)value;

            if (args.OriginalSource != null)
            {
                var obj = args.OriginalSource;
            }
            
            var controlName = (string)parameter;

            if (args == null) return value;
            return controlName;
            /*
            IRequestQuery item = new DoubleTappedRequest(controlName, parameter)
            {
                Request = controlName,
                QueryText = parameter
            };
            return item;*/
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
