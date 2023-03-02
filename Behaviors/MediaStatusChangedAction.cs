using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UwpSample.Converters.ValueConverters;

namespace UwpSample.Behaviors
{
    public class MediaStatusChangedAction : DependencyObject, IAction
    {
        IMediaElementStatusChanged mediaStatusChanged;

        public object Execute(object sender, object parameter)
        {
            //samplecd = new ContentDialogSample();
            //ShowCD();
            return null;
        }

        public async void ShowCD()
        {
            //await samplecd.ShowAsync();
        }
    }
}
