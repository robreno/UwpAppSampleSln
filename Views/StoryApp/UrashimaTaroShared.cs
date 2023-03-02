using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Linq;
using Windows.UI.Xaml;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Newtonsoft.Json.Linq;
using Windows.UI.Xaml.Documents;
using System.Collections.Generic;

using UwpSample.Services;
using UwpSample.Models;
using UwpSample.Models.Interfaces;
using UwpSample.ViewModels;
using UwpSample.Dtos;
using UwpSample.AttachedProperties;
using Windows.Media;
using Windows.UI.Core;
using System;
using Windows.UI.Xaml.Media;
using Windows.Media.Playback;
using Windows.Storage;

namespace UwpSample.Views
{
    public sealed partial class UrashimaTaroPage : Page
    {
        // MediaPlayerElement Code
        public static SystemMediaPlayerPage Current;
        private SystemMediaPlayerPage rootPage2 = null;
        private bool isInitialized = false;

        /// <summary>
        /// Indicates whether this scenario page is still active. Changes value during navigation 
        /// to or away from the page.
        /// </summary>
        private bool isThisPageActive = false;

        // same type as returned from Windows.Storage.Pickers.FileOpenPicker.PickMultipleFilesAsync()
        private IReadOnlyList<StorageFile> playlist = null;

        /// <summary>
        /// index to current media item in playlist
        /// </summary>
        private int currentItemIndex = 0;

        /// <summary>
        /// Indicates whetehr to stat the playlist again upon reaching the end. 
        /// </summary>
        private bool repeatPlaylist = false;

        private SystemMediaTransportControls systemMediaControls = null;

        private DispatcherTimer smtcPositionUpdateTimer = null;

        private bool pausedDueToMute = false;

        private MediaPlayerElement mediaPlayer;

        private MediaPlaybackItem mediaPlaybackItem;
    }
}
