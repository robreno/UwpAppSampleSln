using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.Media;
using Windows.UI.Xaml.Media;
using Newtonsoft.Json.Linq;

namespace UwpSample.Objects
{
    // extension methods for presenting MessageDialog instances...
    internal static class PageExtender
    {
        internal static IAsyncOperation<IUICommand> ShowAlertAsync(this Page page, ErrorBucket errors)
        {
            return ShowAlertAsync(page, errors.GetErrorsAsString());
        }

        internal static IAsyncOperation<IUICommand> ShowAlertAsync(this Page page, string message)
        {
            // show...
            MessageDialog dialog = new MessageDialog(message != null ? message : string.Empty);
            return dialog.ShowAsync();
        }

        internal static IAsyncAction PlayAudioAsync(this Page page, ErrorBucket errors)
        {
            return PlayAudioAsync(page, errors.GetErrorsAsString());
        }

        internal static IAsyncAction PlayAudioAsync(this Page page, string message)
        {
            MediaElement currentMedia = page.FindName("mediaElement") as MediaElement;
            AppBarButton playPauseBtn = page.FindName("PlayPauseBtn") as AppBarButton;
            AppBarButton stopBtn = page.FindName("ResetAudioBtn") as AppBarButton;

            var mediaSource = currentMedia.Source as Uri;
            var paperName = currentMedia.Tag;

            JObject json;
            if (message != "mediaElement")
                json = JObject.Parse(message);

            // Using the Tag property value lets you localize 
            // the Label value without affecting the app code.
            string mode = playPauseBtn.Tag as string;
            if (mode.Equals("Playing"))
            {
                // Bug Here on Play
                playPauseBtn.Icon = new SymbolIcon(Symbol.Play);
                playPauseBtn.Tag = "Paused";
                playPauseBtn.Label = "Play Audio";
                currentMedia.Pause();
            }
            else if (mode.Equals("Paused") || mode.Equals("Stopped"))
            {
                playPauseBtn.Icon = new SymbolIcon(Symbol.Pause);
                playPauseBtn.Tag = "Playing";
                playPauseBtn.Label = "Pause Audio";
                currentMedia.Play();
                stopBtn.IsEnabled = true;
            }
            playPauseBtn.UpdateLayout();
            return Task.Run(() => {}).AsAsyncAction();
        }

        internal static IAsyncAction PlayAudioRangeAsync(this Page page, ErrorBucket errors)
        {
            return PlayAudioRangeAsync(page, errors.GetErrorsAsString());
        }

        internal static IAsyncAction PlayAudioRangeAsync(this Page page, string message)
        {
            MediaElement mediaElement = page.FindName("mediaElement") as MediaElement;
            AppBarButton playPauseBtn = page.FindName("PlayPauseBtn") as AppBarButton;
            AppBarButton stopBtn = page.FindName("ResetAudioBtn") as AppBarButton;

            var mediaSource = mediaElement.Source as Uri;
            var paperName = mediaElement.Tag;

            JObject json = JObject.Parse(message);
            var audioSetting = new CurrentAudioInfo.AudioSetting
            {
                ParagraphName = (string)json["paragraphName"],
                StartMarker = (string)json["startMarker"],
                EndMarker = (string)json["endMarker"]
            };

            string[] start = audioSetting.StartMarker.Split(':');
            string[] end = audioSetting.EndMarker.Split(':');

            TimeSpan startTime = new TimeSpan(Int32.Parse(start[0]), Int32.Parse(start[1]), Int32.Parse(start[2]));
            TimeSpan endTime = new TimeSpan(Int32.Parse(end[0]), Int32.Parse(end[1]), Int32.Parse(end[2]));

            mediaElement.Markers.Clear();
            TimelineMarker marker = new TimelineMarker();
            marker.Text = audioSetting.ParagraphName;
            marker.Time = endTime;

            if (mediaElement.CurrentState.Equals(MediaElementState.Playing))
                page.StopAudioAsync("mediaElement").AsTask();
            mediaElement.Position = startTime;
            mediaElement.Markers.Add(marker);
            page.PlayAudioAsync("mediaElement").AsTask();
            return Task.Run(() => { }).AsAsyncAction();
        }

        internal static IAsyncAction StopAudioAsync(this Page page, ErrorBucket errors)
        {
            return StopAudioAsync(page, errors.GetErrorsAsString());
        }

        internal static IAsyncAction StopAudioAsync(this Page page, string message)
        {
            MediaElement currentMedia = page.FindName("mediaElement") as MediaElement;
            AppBarButton playPauseBtn = page.FindName("PlayPauseBtn") as AppBarButton;
            AppBarButton stopBtn = page.FindName("ResetAudioBtn") as AppBarButton;

            // Using the Tag property value lets you localize 
            // the Label value without affecting the app code.
            string currentMode = playPauseBtn.Tag as string;
            if (currentMode.Equals("Playing"))
            {
                playPauseBtn.Icon = new SymbolIcon(Symbol.Play);
                playPauseBtn.Tag = "Stopped";
                currentMedia.Stop();
                currentMedia.Position.Subtract(currentMedia.Position);
                stopBtn.IsEnabled = false;
            }
            else if (currentMode.Equals("Paused"))
            {
                playPauseBtn.Icon = new SymbolIcon(Symbol.Play);
                playPauseBtn.Tag = "Playing";
                currentMedia.Stop();
                currentMedia.Position.Subtract(currentMedia.Position);
            }
            playPauseBtn.UpdateLayout();
            return Task.Run(() => { }).AsAsyncAction();
        }

        internal static IAsyncAction SetAudioStateAsync(this Page page, ErrorBucket errors)
        {
            return SetAudioStateAsync(page, errors.GetErrorsAsString());
        }

        internal static IAsyncAction SetAudioStateAsync(this Page page, string message, SystemMediaTransportControls mediaControls = null)
        {
            MediaElement currentMedia = page.FindName(message) as MediaElement;
            AppBarButton playPauseBtn = page.FindName("PlayPauseBtn") as AppBarButton;
            AppBarButton stopBtn = page.FindName("ResetAudioBtn") as AppBarButton;

            string mode = playPauseBtn.Tag as string;
            switch (currentMedia.CurrentState)
            {
                case MediaElementState.Playing: // Current Media Element State = Playing while Media Controls Playback
                    mediaControls.PlaybackStatus = MediaPlaybackStatus.Playing;
                    if (mode != "Playing")
                    { 
                        playPauseBtn.Tag = "Playing";
                        playPauseBtn.Icon = new SymbolIcon(Symbol.Pause);
                        playPauseBtn.Label = "Pause Audio";
                        playPauseBtn.UpdateLayout();
                    }
                    break;
                case MediaElementState.Paused:
                    mediaControls.PlaybackStatus = MediaPlaybackStatus.Paused;
                    if (mode != "Paused")
                    {
                        playPauseBtn.Tag = "Paused";
                        playPauseBtn.Icon = new SymbolIcon(Symbol.Play);
                        playPauseBtn.Label = "Play Audio";
                        playPauseBtn.UpdateLayout();
                        stopBtn.IsEnabled = false;
                    }
                    break;
                case MediaElementState.Stopped:
                    mediaControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
                    if (mode != "Stopped")
                    {
                        playPauseBtn.Tag = "Stopped";
                        playPauseBtn.Icon = new SymbolIcon(Symbol.Play);
                        playPauseBtn.Label = "Play Audio";
                        playPauseBtn.UpdateLayout();
                    }
                    break;
                case MediaElementState.Closed:
                    mediaControls.PlaybackStatus = MediaPlaybackStatus.Closed;
                    break;
                case MediaElementState.Buffering:
                    // Nothing to do for this state
                    break;
                default:
                    // Nothing to do for this state
                    break;
            }
            return Task.Run(() => { }).AsAsyncAction();
        }

        internal static IAsyncAction DownloadAudioAsync(this Page page, ErrorBucket errors)
        {
            return DownloadAudioAsync(page, errors.GetErrorsAsString());
        }

        internal static IAsyncAction DownloadAudioAsync(this Page page, string message)
        {
            DownloadPaperServiceProxy downloadServiceProxy = new DownloadPaperServiceProxy(message);
            Task<DownloadPaperResult> result = downloadServiceProxy.DownloadPaperAsync();
            return Task.Run(() => { }).AsAsyncAction();
        }

        internal static IAsyncAction SetConnectivityAsync(this Page page, ErrorBucket errors)
        {
            return SetConnectivityAsync(page, errors.GetErrorsAsString());
        }

        internal static IAsyncAction SetConnectivityAsync(this Page page, string message)
        {
            MediaElement currentMedia = page.FindName("mediaElement") as MediaElement;
            AppBarButton playPauseBtn = page.FindName("PlayPauseBtn") as AppBarButton;
            AppBarButton stopBtn = page.FindName("ResetAudioBtn") as AppBarButton;
            AppBarButton barsButton = page.FindName("BarsBtn") as AppBarButton;

            string mode = playPauseBtn.Tag as string;

            JObject json = JObject.Parse(message);

            var profileSetting = new CurrentConnectionInfo.ProfileSetting
            {
                ProfileName = (string)json["profileName"],
                ConnectivityLevel = (string)json["connectivityLevel"],
                IncomingBitsPerSec = (string)json["incomingBitsPerSec"],
                OutgoingBitsPerSec = (string)json["outgoingBitsPerSec"],
                Signals = (string)json["signals"],
                Signal = (string)json["signal"],
            };

            var signalValue = int.Parse(profileSetting.Signal);
            var signal = signalValue == 0 ? "ZeroBars" : 
                signalValue == 1 ? "OneBar" : 
                signalValue == 2 ? "TwoBars" : 
                signalValue == 3 ? "ThreeBars" : 
                signalValue >= 4 ? "FourBars" : "UnknownBars";

            SymbolIcon symbolIcon;

            switch (signal)
            {
                case "ZeroBars":
                    symbolIcon = new SymbolIcon(Symbol.ZeroBars);
                    break;
                case "OneBar":
                    symbolIcon = new SymbolIcon(Symbol.OneBar);
                    break;
                case "TwoBars":
                    symbolIcon = new SymbolIcon(Symbol.TwoBars);
                    break;
                case "ThreeBars":
                    symbolIcon = new SymbolIcon(Symbol.ThreeBars);
                    break;
                case "FourBars":
                    symbolIcon = new SymbolIcon(Symbol.FourBars);
                    break;
                default:
                    symbolIcon = new SymbolIcon(Symbol.ZeroBars);
                    break;
            }
            barsButton.Icon = symbolIcon;

            if (profileSetting.ConnectivityLevel == "InternetAccess") 
            { 
                barsButton.Label = profileSetting.ProfileName + " Online";
                if (!mode.Equals("Paused"))
                {
                    playPauseBtn.IsEnabled = true;
                    playPauseBtn.Icon = new SymbolIcon(Symbol.Play);
                    playPauseBtn.Label = "Play Audio";
                }
            }
            else if (mode == "Offline") 
            { 
                barsButton.Label = "Offline";
                playPauseBtn.IsEnabled = false;
                playPauseBtn.Icon = new SymbolIcon(Symbol.Audio);
                playPauseBtn.Label = "Audio Offline";
            }
            playPauseBtn.UpdateLayout();
            barsButton.UpdateLayout(); 
            
            return Task.Run(() => { }).AsAsyncAction();
        }

        internal static IAsyncAction SetScrollViewerAsync(this Page page, ErrorBucket errors)
        {
            return SetScrollViewerAsync(page, errors.GetErrorsAsString());
        }

        internal static IAsyncAction SetScrollViewerAsync(this Page page, string message)
        {
		    double? _horizontaloffset = 0, _verticaloffset = 0;
            string _targetControlName = message;

            ScrollViewer scrollViewer = page.FindName("contentScrollViewer") as ScrollViewer;

            if (message != string.Empty)
            {
                var control = scrollViewer.FindName(message) as RichTextBlock;
                var generalTransform = control.TransformToVisual(scrollViewer);

                var topLeft = generalTransform.TransformPoint(new Point(0, 0));
                var top = topLeft.Y;
                var left = topLeft.X;
                var bottom = top + control.ActualHeight;
                var right = left + control.ActualWidth;

                var verticalOffsetValue = scrollViewer.VerticalOffset;
                var maxVerticalOffsetValue = scrollViewer.ExtentHeight - scrollViewer.ViewportHeight; // 12545.714965820313

                if (maxVerticalOffsetValue < 0 || verticalOffsetValue == maxVerticalOffsetValue)
                {
                    // Scrolled to bottom

                }
                else if (verticalOffsetValue == 0)
                {
                    // Scrolled to top
                }

                double? scrollToY = null;
                if (top < 0)
                {
                    scrollToY = top + scrollViewer.VerticalOffset;
                }
                else if (bottom > scrollViewer.ViewportHeight)
                {
                    //scrollToY = bottom + contentScrollViewer.VerticalOffset - contentScrollViewer.ViewportHeight;
                    scrollToY = top + scrollViewer.VerticalOffset - (scrollViewer.ActualHeight - scrollViewer.ViewportHeight);
                }

                double? scrollToX = null;
                if (left < 0)
                {
                    scrollToX = left + scrollViewer.HorizontalOffset;
                }
                else if (right > scrollViewer.ViewportWidth)
                {
                    scrollToX = right + scrollViewer.HorizontalOffset - scrollViewer.ViewportWidth;
                }

                _horizontaloffset = scrollToX;
                _verticaloffset = scrollToY;
            }
            scrollViewer.ChangeView(_horizontaloffset, _verticaloffset, 1, false);

            return Task.Run(() => { }).AsAsyncAction();
        }

        internal static void InitializeViewModel(this IViewModelHost host, IViewModel model = null)
        {
            // if we don't get given a model?
            if (model == null)
            {
                var attr = (ViewModelAttribute)host.GetType().GetTypeInfo().GetCustomAttribute<ViewModelAttribute>();
                if (attr != null)
                    model = (IViewModel)TinyIoCContainer.Current.Resolve(attr.ViewModelInterfaceType);
                else
                    throw new InvalidOperationException(string.Format("Page '{0}' is not decorated with ViewModelAttribute."));
            }

            // setup...
            model.Initialize((IViewModelHost)host);
            ((Page)host).DataContext = model;
        }

        internal static IViewModel GetModel(this Page page)
        {
            return page.DataContext as IViewModel;
        }
    }
}
