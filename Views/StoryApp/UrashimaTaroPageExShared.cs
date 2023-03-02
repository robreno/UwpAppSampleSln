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

namespace UwpSample.Views
{
    public sealed partial class UrashimaTaroPageEx : Page
    {
        private string _targetControlName = string.Empty;
        private char _pauseMarker = '/';
        private char _singleSpaceChar = ' ';

        string[] timeSpans = new string[12]
           {
                "0:0:3:0_0:0:9:0",          // (1.0)
                "0:0:9:0_0:0:36:0",         // (1.1)
                "0:0:36:0_0:1:43:0",        // (1.2)
                "0:1:44:0_0:2:15:40",       // (1.3)
                "0:2:15:40_0:3:40:0",      // (1.4)
                "0:3:40:0_0:4:10:0",        // (1.5)
                "0:4:10:0_0:4:36:0",        // (1.6)
                "0:4:36:0_0:5:38:0",        // (1.7)
                "0:5:38:0_0:5:49:0",     // (1.8)
                "0:5:49:0_0:6:30:0",        // (1.9)
                "0:6:28:0_0:7:04:0",        // (1.10)
                "0:7:04:0_0:7:20:0"         // (1.11)
           };

        #region // MenuFlyout Impl
        private static void SetVisibility(string name, IList<MenuFlyoutItemBase> items, Visibility visibility)
        {
            var flyoutItem = (MenuFlyoutItem)items
                                .Where(i => i.Name.Equals(name))
                                .FirstOrDefault();
            flyoutItem.Visibility = visibility;
        }
        private void SetEnglishMenu(UIElement item, string langPrefix)
        {
            RichTextBlock rtb = (RichTextBlock)item;
            string postfix = rtb.Name.Split('_')[2];
            MenuFlyout menuFlyout = new MenuFlyout();
            menuFlyout.SetValue(NameProperty, langPrefix + "_ContextMenu_" + postfix);
            // Add Copy MenuFlyoutItem
            var copyItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_CopyFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemCopy/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Copy },
                Tag = rtb.Name,
                Visibility = Visibility.Collapsed
            };
            copyItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemCopyToolTip/Text"));
            copyItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(copyItem);

            // Add Select MenuFlyoutItem
            var selectAllItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_SelectAllFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemSelectAll/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.SelectAll },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            selectAllItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemSelectAllToolTip/Text"));
            selectAllItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(selectAllItem);

            // Add MenuFlyoutItemSeparator Bar
            menuFlyout.Items.Add(new MenuFlyoutSeparator());

            // Add Show Vocabulary MenuFlyoutItem
            var vocabItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_VocabFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemVocab/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Page },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            vocabItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemVocabToolTip/Text"));
            vocabItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(vocabItem);

            // Add Audio Play MenuFlyoutItem
            var playAudioItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_PlayAudioFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemPlayAudio/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Play },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            playAudioItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemPlayAudioToolTip/Text"));
            playAudioItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(playAudioItem);

            // Add Audio Pause MenuFlyoutItem
            var pauseAudioItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_PauseAudioFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemPauseAudio/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Pause },
                Tag = rtb.Name,
                Visibility = Visibility.Collapsed
            };
            pauseAudioItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemPauseAudioToolTip/Text"));
            pauseAudioItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(pauseAudioItem);

            // Add Audio Pause MenuFlyoutItem
            var stopAudioItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_StopAudioFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemStopAudio/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Stop },
                Tag = rtb.Name,
                Visibility = Visibility.Collapsed
            };
            stopAudioItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemStopAudioToolTip/Text"));
            stopAudioItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(stopAudioItem);

            // Add MenuFlyoutItemSeparator Bar
            menuFlyout.Items.Add(new MenuFlyoutSeparator());

            // Add More MenuFlyoutItem
            var moreItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_MoreFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemMore/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.More },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            moreItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemMoreToolTip/Text"));
            moreItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(moreItem);

            // Set item.ContextFlyout Property
            item.ContextFlyout = menuFlyout;
            return;
        }
        private void SetJapaneseMenu(UIElement item, string langPrefix)
        {
            RichTextBlock rtb = (RichTextBlock)item;
            string postfix = rtb.Name.Split('_')[2];
            MenuFlyout menuFlyout = new MenuFlyout();
            menuFlyout.SetValue(NameProperty, langPrefix + "_ContextMenu_" + postfix);
            // Add Copy MenuFlyoutItem
            var copyItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_CopyFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemCopy/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Copy },
                Tag = rtb.Name,
                Visibility = Visibility.Collapsed
            };
            copyItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemCopyToolTip/Text"));
            copyItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(copyItem);

            // Add Select MenuFlyoutItem
            var selectAllItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_SelectAllFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemSelectAll/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.SelectAll },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            selectAllItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemSelectAllToolTip/Text"));
            selectAllItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(selectAllItem);

            // Add MenuFlyoutItemSeparator Bar
            menuFlyout.Items.Add(new MenuFlyoutSeparator());

            // Add Show Vocabulary MenuFlyoutItem
            var vocabItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_VocabFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemVocab/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Page },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            vocabItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemVocabToolTip/Text"));
            vocabItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(vocabItem);

            // Add Audio Play MenuFlyoutItem
            var playAudioItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_PlayAudioFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemPlayAudio/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Play },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            playAudioItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemPlayAudioToolTip/Text"));
            playAudioItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(playAudioItem);

            // Add Audio Pause MenuFlyoutItem
            var pauseAudioItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_PauseAudioFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemPauseAudio/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Pause },
                Tag = rtb.Name,
                Visibility = Visibility.Collapsed
            };
            pauseAudioItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemPauseAudioToolTip/Text"));
            pauseAudioItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(pauseAudioItem);

            // Add Audio Pause MenuFlyoutItem
            var stopAudioItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_StopAudioFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemStopAudio/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Stop },
                Tag = rtb.Name,
                Visibility = Visibility.Collapsed
            };
            stopAudioItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemStopAudioToolTip/Text"));
            stopAudioItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(stopAudioItem);

            // Add Pause Marker MenuFlyoutItem
            var regularItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_RegularTextFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemRegular/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.View },
                Tag = rtb.Name,
                Visibility = Visibility.Collapsed
            };
            regularItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemRegularToolTip/Text"));
            regularItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(regularItem);

            // Add Pause Marker MenuFlyoutItem
            var pauseMarkerItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_PauseTextFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemPauseMarker/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.View },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            pauseMarkerItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemPauseMarkerToolTip/Text"));
            pauseMarkerItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(pauseMarkerItem);

            // Add Ruby Text MenuFlyoutItem
            var rubyTextItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_RubyTextFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemRuby/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.View },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            rubyTextItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemRubyToolTip/Text"));
            rubyTextItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(rubyTextItem);

            // Add MenuFlyoutItemSeparator Bar
            menuFlyout.Items.Add(new MenuFlyoutSeparator());

            // Add More MenuFlyoutItem
            var moreItem = new MenuFlyoutItem()
            {
                Name = langPrefix + "_MoreFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemMore/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.More },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            moreItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemMoreToolTip/Text"));
            moreItem.Click += new RoutedEventHandler(OnMenuFlyoutItemClick);
            menuFlyout.Items.Add(moreItem);

            // Set item.ContextFlyout Property
            item.ContextFlyout = menuFlyout;
            return;
        }
        #endregion

        #region // MediaElement Impl
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
        private void SetSystemMediaTransportControls()
        {
            // Set MediaElement
            _mediaElement = mediaElement;

            // Enable the system-wide Play, Pause, and Stop buttons
            _mediaControls = SystemMediaTransportControls.GetForCurrentView();
            _mediaControls.IsPlayEnabled = true;
            _mediaControls.IsPauseEnabled = true;
            _mediaControls.IsStopEnabled = true;

            // This must be set before setting the music-specific properties
            _mediaControls.DisplayUpdater.Type = MediaPlaybackType.Music;

            // Now we can set the music-specific properties
            _mediaControls.DisplayUpdater.MusicProperties.Title = "Urashima Taro";
            _mediaControls.DisplayUpdater.MusicProperties.Artist = "AGMedia";

            //_mediaControls.DisplayUpdater.Thumbnail =
            //    RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/UB606_100x100.png"));

            // Commit the update
            _mediaControls.DisplayUpdater.Update();

            // Update the MediaElement state whenever the system-wide state changes
            //_mediaElement.CurrentStateChanged += OnMediaElementCurrentStateChanged;

            // Update the system-wide state whenever the MediaElement state changes
            //_mediaControls.ButtonPressed += OnSystemMediaTransportControlsStateChanged;
        }

        private void OnMediaElementCurrentStateChangedTest(object sender, RoutedEventArgs e)
        {

        }
        private void OnMediaElementCurrentStateChanged(object sender, RoutedEventArgs e)
        {
            SystemMediaTransportControls mediaControls = SystemMediaTransportControls.GetForCurrentView();

            switch (mediaElement.CurrentState)
            {
                case MediaElementState.Playing:
                    mediaControls.PlaybackStatus = MediaPlaybackStatus.Playing;
                    break;
                case MediaElementState.Paused:
                    mediaControls.PlaybackStatus = MediaPlaybackStatus.Paused;
                    break;
                case MediaElementState.Stopped:
                    mediaControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
                    break;
                case MediaElementState.Closed:
                    mediaControls.PlaybackStatus = MediaPlaybackStatus.Closed;
                    break;
                default:
                    // Nothing to do for this state
                    break;
            }
        }
        public async void OnSystemMediaTransportControlsStateChanged(SystemMediaTransportControls sender, 
                                                                     SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            // This is invoked on a background thread
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                switch (args.Button)
                {
                    // We only need to worry about the buttons we explicitly enabled
                    case SystemMediaTransportControlsButton.Play:
                        _mediaElement.Play();
                        break;
                    case SystemMediaTransportControlsButton.Pause:
                        _mediaElement.Pause();
                        break;
                    case SystemMediaTransportControlsButton.Stop:
                        _mediaElement.Stop();
                        break;
                    default:
                        // Unexpected button
                        break;
                }
            });
        }
        #endregion
    }
}
