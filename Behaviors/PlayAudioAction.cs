using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

using UwpSample.ViewModels;

namespace UwpSample.Behaviors
{
    public class PlayAudioAction : DependencyObject, IAction
    {
        public TimeSpan AudioPosition
        {
            get { return (TimeSpan)GetValue(AudioPositionProperty); }
            set { SetValue(AudioPositionProperty, value); }
        }

        public Brush HighlightBrush
        {
            get { return (Brush)GetValue(HighlightBrushProperty); }
            set { SetValue(HighlightBrushProperty, value); }
        }

        public static DependencyProperty AudioPositionProperty =
            DependencyProperty.RegisterAttached("AudioPosition", typeof(TimeSpan), typeof(PlayAudioAction), new PropertyMetadata(null));

        public static DependencyProperty HighlightBrushProperty =
            DependencyProperty.Register("HighlightBrush", typeof(Brush), typeof(HighlightSearchAction), new PropertyMetadata(new SolidColorBrush(Colors.Orange)));

        public object Execute(object sender, object parameter)
        {
            var button = sender as AppBarButton;
            if (button == null) return false;

            var audioPosition = AudioPosition;
            TimeSpan position = new TimeSpan(0, 0, 20);
            var vm = button.DataContext as UrashimaTaroViewModel;
            vm.CurrentMediaPosition = position;
            var content = ((Frame)Window.Current.Content);
            var sourcePageType = content.SourcePageType;

            Task.Factory.StartNew(() => vm.PlayAudioAsync());

            return true;
        }

        private Frame GetFrame(DependencyObject dependencyObject)
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            var parentFrame = parent as Frame;
            if (parentFrame != null) return parentFrame;
            return GetFrame(parent);
        }
    }
}