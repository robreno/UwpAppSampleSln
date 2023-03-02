using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

using UwpSample.DataModels;
using UwpSample.Services;
using UwpSample.ViewModels;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
// See: https://github.com/microsoft/Windows-universal-samples // BackgroundMediaPlayback

namespace UwpSample.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MediaPlayerPage : Page
    {
        public static Page Current;
        List<Scenario> itemCollection;

        private ISettingsService settingsServivce = Ioc.Default.GetRequiredService<ISettingsService>();

        MediaPlayer Player => PlaybackService.Instance.Player;

        MediaPlaybackList PlaybackList
        {
            get { return Player.Source as MediaPlaybackList; }
            set { Player.Source = value; }
        }
        MediaList MediaList
        {
            get { return PlaybackService.Instance.CurrentPlaylist; }
            set { PlaybackService.Instance.CurrentPlaylist = value; }
        }

        public PlayerViewModel PlayerViewModel { get; set; }

        public MediaPlayerPage()
        {
            this.InitializeComponent();
            Current = this;

            // Never reuse the cached page because the model is designed to be unloaded and disposed
            this.NavigationCacheMode = NavigationCacheMode.Disabled;

            // Setup MediaPlayer view model
            PlayerViewModel = new PlayerViewModel(Player, Dispatcher);

            // Handle page load events
            Loaded += MediaPlayerPage_Loaded;
            Unloaded += MediaPlayerPage_Unloaded;

            // Respond to playback rate changes.
            Player.MediaPlayerRateChanged += Player_MediaPlayerRateChanged;
            UpdatePlaybackSpeed();
        }

        private async void Player_MediaPlayerRateChanged(MediaPlayer sender, MediaPlayerRateChangedEventArgs args)
        {
            // Media callbacks use a worker thread so dispatch to UI as needed
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdatePlaybackSpeed);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Populate the scenario list from the SampleConfiguration.cs file
            var _itemCollection = new List<Scenario>();
            int i = 1;
            foreach (Scenario s in scenarios)
            {
                _itemCollection.Add(new Scenario { Title = $"{i++}) {s.Title}", ClassType = s.ClassType });
            }
            itemCollection = _itemCollection;
        }

        public List<Scenario> Scenarios
        {
            get { return itemCollection; }
        }

        private async void MediaPlayerPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("MediaPlayerPage_Loaded");

            // Update controls according to settings
            UpdateControlVisibility();
            //settingsService.UseCustomControlsChanged += SettingsService_UseCustomControlsChanged;

            // Bind player to element
            mediaPlayerElement.SetMediaPlayer(Player);

            // Load the playlist data model if needed
            if (MediaList == null)
            {
                // Create the playlist data model
                MediaList = new MediaList();
                await MediaList.LoadFromApplicationUriAsync("ms-appx:///Assets/JsonData/playlist.json");
            }

            // Create a new playback list matching the data model if one does not exist
            if (PlaybackList == null)
                PlaybackList = MediaList.ToPlaybackList();

            // Subscribe to playback list item failure events
            PlaybackList.ItemFailed += PlaybackList_ItemFailed;

            // Create the view model list from the data model and playback model
            // and assign it to the player
            PlayerViewModel.MediaList = new MediaListViewModel(MediaList, PlaybackList, Dispatcher);
        }

        private void MediaPlayerPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("MediaPlayerPage_Unloaded");

            // Ensure the page is no longer holding references and force a GC
            // to ensure these are unloaded immediately since the app has
            // only a short timeframe to reduce working set to avoid suspending
            // on background transition.

            //settingsService.UseCustomControlsChanged -= SettingsService_UseCustomControlsChanged;

            PlaybackList.ItemFailed -= PlaybackList_ItemFailed;
            PlayerViewModel.Dispose();
            PlayerViewModel = null;

            GC.Collect();
        }

        private void SettingsService_UseCustomControlsChanged(object sender, EventArgs e)
        {
            UpdateControlVisibility();
        }

        void UpdateControlVisibility()
        {

            if (settingsServivce.UseCustomControls)
            {
                mediaPlayerElement.AreTransportControlsEnabled = false;
                customButtons.Visibility = Visibility.Visible;
            }
            else
            {
                mediaPlayerElement.AreTransportControlsEnabled = true;
                customButtons.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Handle item failures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void PlaybackList_ItemFailed(MediaPlaybackList sender, MediaPlaybackItemFailedEventArgs args)
        {
            // Media callbacks use a worker thread so dispatch to UI as needed
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var error = string.Format("Item failed to play: {0} | 0x{1:x}",
                    args.Error.ErrorCode, args.Error.ExtendedError.HResult);
                //MediaPlayer.Current.NotifyUser(error, NotifyType.ErrorMessage);
            });
        }

        public async void speedButton_Click(object sender, RoutedEventArgs e)
        {
            // Create menu and add commands
            var popupMenu = new PopupMenu();

            popupMenu.Commands.Add(new UICommand("4.0x", command => Player.PlaybackSession.PlaybackRate = 4.0));
            popupMenu.Commands.Add(new UICommand("2.0x", command => Player.PlaybackSession.PlaybackRate = 2.0));
            popupMenu.Commands.Add(new UICommand("1.5x", command => Player.PlaybackSession.PlaybackRate = 1.5));
            popupMenu.Commands.Add(new UICommand("1.0x", command => Player.PlaybackSession.PlaybackRate = 1.0));
            popupMenu.Commands.Add(new UICommand("0.5x", command => Player.PlaybackSession.PlaybackRate = 0.5));

            // Get button transform and then offset it by half the button
            // width to center. This will show the popup just above the button.
            var control = sender as Control;
            var transform = control.TransformToVisual(null);
            var point = transform.TransformPoint(new Point(control.ActualWidth / 2, 0));

            // Show popup
            var selected = await popupMenu.ShowAsync(point);
        }

        //private void Player_MediaPlayerRateChanged(MediaPlayerPage sender, MediaPlayerRateChangedEventArgs e)
        //{
        //    // Media callbacks use a worker thread so dispatch to UI as needed
        //    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdatePlaybackSpeed);
        //}

        private void UpdatePlaybackSpeed()
        {
            speedButton.Content = String.Format("{0:0.0}x", Player.PlaybackSession.PlaybackRate);
        }
    }
}
