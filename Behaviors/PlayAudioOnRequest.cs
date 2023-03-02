using Microsoft.Xaml.Interactivity;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;

namespace UwpSample.Behaviors
{
    // Documentation on validating user input is at http://go.microsoft.com/fwlink/?LinkID=288817&clcid=0x409

    public class PlayAudioOnRequest : Behavior<FrameworkElement>
    {
        public ReadOnlyCollection<string> PropertyRequests
        {
            get { return (ReadOnlyCollection<string>)GetValue(PropertyRequestsProperty); }
            set { SetValue(PropertyRequestsProperty, value); }
        }

        public string HighlightStyleName
        {
            get { return (string)GetValue(HighlightStyleNameProperty); }
            set { SetValue(HighlightStyleNameProperty, value); }
        }

        public string OriginalStyleName
        {
            get { return (string)GetValue(OriginalStyleNameProperty); }
            set { SetValue(OriginalStyleNameProperty, value); }
        }

        public static DependencyProperty PropertyRequestsProperty =
            DependencyProperty.RegisterAttached("PropertyRequests", typeof(ReadOnlyCollection<string>), typeof(PlayAudioOnRequest), new PropertyMetadata(BindableValidator.EmptyErrorsCollection, OnPropertyRequestsChanged));

        // The default for this property only applies to TextBox controls.
        public static DependencyProperty HighlightStyleNameProperty =
            DependencyProperty.RegisterAttached("HighlightStyleName", typeof(string), typeof(PlayAudioOnRequest), new PropertyMetadata("HighlightTextBoxStyle"));

        // The default for this property only applies to TextBox controls.
        protected static DependencyProperty OriginalStyleNameProperty =
            DependencyProperty.RegisterAttached("OriginalStyleName", typeof(Style), typeof(PlayAudioOnRequest), new PropertyMetadata("BaseTextBoxStyle"));

        private static void OnPropertyRequestsChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if (args == null || args.NewValue == null)
            {
                return;
            }

            var control = ((Behavior<FrameworkElement>)d).AssociatedObject;
            var propertyRequests = (ReadOnlyCollection<string>)args.NewValue;

            Style style = (propertyRequests.Any()) ?
                (Style)Application.Current.Resources[((PlayAudioOnRequest)d).HighlightStyleName] :
                (Style)Application.Current.Resources[((PlayAudioOnRequest)d).OriginalStyleName];

            control.Style = style;
        }

        protected override void OnAttached()
        {
        }
    }
}
