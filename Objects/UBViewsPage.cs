using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.Media;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

using UwpSample.ViewModels;

namespace UwpSample.Objects
{
    public class UBViewsPage : Page
    {
        private string[] _thumbnails = { "UB606_100x100.png", "shroud-rostro-jesucristo2.jpg", "shrouldlarge_face.jpg", "BookCover512by512.jpg", "UrantiaCircles2.png" };

        SystemMediaTransportControls _mediaControls;
        MediaElement _mediaElement;

        public SystemMediaTransportControls SystemMediaTransportControls
        {
            get { return _mediaControls; }
            set { _mediaControls = value; }
        }

        public MediaElement MediaElement
        {
            get { return _mediaElement; }
            set { _mediaElement = value; }
        }

        public string[] Thumbnails
        {
            get { return _thumbnails; }
        }

        public UBViewsPage()
        {
            
        }

        //Task IViewModelHost.ShowAlertAsync(ErrorBucket errors)
        //{
        //    return PageExtender.ShowAlertAsync(this, errors).AsTask();
        //}

        //Task IViewModelHost.PlayAudioAsync(string message)
        //{
        //    return PageExtender.PlayAudioAsync(this, message).AsTask();
        //}

        //Task IViewModelHost.PlayAudioAsync(ErrorBucket errors)
        //{
        //    return PageExtender.PlayAudioAsync(this, errors).AsTask();
        //}

        //Task IViewModelHost.PlayAudioRangeAsync(string message)
        //{
        //    return PageExtender.PlayAudioRangeAsync(this, message).AsTask();
        //}

        //Task IViewModelHost.PlayAudioRangeAsync(ErrorBucket errors)
        //{
        //    return PageExtender.PlayAudioRangeAsync(this, errors).AsTask();
        //}

        //Task IViewModelHost.StopAudioAsync(string message)
        //{
        //    return PageExtender.StopAudioAsync(this, message).AsTask();
        //}

        //Task IViewModelHost.StopAudioAsync(ErrorBucket errors)
        //{
        //    return PageExtender.StopAudioAsync(this, errors).AsTask();
        //}

        //Task IViewModelHost.SetAudioStateAsync(string message)
        //{
        //    return PageExtender.SetAudioStateAsync(this, message, _mediaControls).AsTask();
        //}

        //Task IViewModelHost.SetAudioStateAsync(ErrorBucket errors)
        //{
        //    return PageExtender.SetAudioStateAsync(this, errors).AsTask();
        //}

        //Task IViewModelHost.SetScrollViewerAsync(string message)
        //{
        //    return PageExtender.SetScrollViewerAsync(this, message).AsTask();
        //}

        //Task IViewModelHost.SetScrollViewerAsync(ErrorBucket errors)
        //{
        //    return PageExtender.SetScrollViewerAsync(this, errors).AsTask();
        //}

        //public void ShowView(Type viewModelType, object parameter = null)
        //{
        //    // get the concrete handler and as the frame to flip... (note we use the ViewFactory,
        //    // not the ViewModelFactory here...)

        //    foreach (var type in this.GetType().GetTypeInfo().Assembly.GetTypes())
        //    {
        //        var attr = (ViewModelAttribute)type.GetCustomAttribute<ViewModelAttribute>();
        //        if (attr != null && viewModelType.IsAssignableFrom(attr.ViewModelInterfaceType))
        //        {
        //            // show...
        //            this.Frame.Navigate(type);
        //        }
        //    }
        //}

        /*
        public void OnHyperlinkRef(Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            var note = this.FindName("Endnote" + sender.Name.Replace("HyperlinkRef", "")) as Paragraph;
            var flyout = new NotesFlyout();
            flyout.HeadingText = "Note:";
            flyout.SetBodyText(note);
            flyout.ShowIndependent();
        }
        */

        //protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);

        //    // ok...
        //    this.GetModel().Activated();
        //}

        //public void ShowAppBar()
        //{
        //    if (this.BottomAppBar != null)
        //        this.BottomAppBar.IsOpen = true;
        //}

        //public void HideAppBar()
        //{
        //    if (this.BottomAppBar != null)
        //        this.BottomAppBar.IsOpen = false;
        //}
    }
}
