using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.Media;
using System.Globalization;
using Newtonsoft.Json.Linq;

using UwpSample.AttachedProperties;
using UwpSample.ViewModels;
using UwpSample.Models;
using UwpSample.Models.Interfaces;
using UwpSample.Dtos;
using UwpSample.Services;


namespace UwpSample.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StoryAppMainPage : Page
    {
        private string _targetControlName = string.Empty;
        private char _pauseMarker = '/';
        private char _singleSpaceChar = ' ';

        private string _defaultLanguage = string.Empty;
        private string _storyId = string.Empty;
        private string _storyLanguage = string.Empty;
        private string _storyName = string.Empty;
        private string _storyTitle = string.Empty;
        private string _storyRubyTitle = string.Empty;
        private string _localAudioUri = string.Empty;

        #region // Services
        private IResourceLoaderService _resourceLoader;
        #endregion

        private SystemMediaTransportControls _mediaControls;
        private Page rootPage;

        public StoryAppMainPage()
        {

            this.InitializeComponent();

            InitializeContent();

            Style titleStyle = (Style)this.Resources["JaTitleParagraph"];
            Style regularStyle = (Style)this.Resources["JaRegularParagraph"];
            _storyLanguage = (string)this.Resources["storyLanguage"];

            string[] timeSpans = new string[12]
            {
                "0:0:3:0_0:0:9:0",          // (1.0)
                "0:0:9:0_0:0:36:0",         // (1.1)
                "0:0:36:0_0:1:43:0",        // (1.2)
                "0:1:44:0_0:2:15:40",       // (1.3)
                "00:2:15:40_0:3:40:0",      // (1.4)
                "0:3:40:0_0:4:10:0",        // (1.5)
                "0:4:10:0_0:4:36:0",        // (1.6)
                "0:4:36:0_0:5:38:0",        // (1.7)
                "0:5:38:0_0:5:49:0 ",     // (1.8)
                "0:5:49:0_0:6:30:0",        // (1.9)
                "0:6:28:0_0:7:04:0",        // (1.10)
                "0:7:04:0_0:7:20:0"         // (1.11)
            };

            var currentLangTag = Windows.Globalization.Language.CurrentInputMethodLanguageTag;

            SetSystemMediaTransportControls();

        }
        private void InitializeContent()
        {
            _defaultLanguage = Windows.Globalization.ApplicationLanguages.Languages[0];
            _resourceLoader = Ioc.Default.GetRequiredService<IResourceLoaderService>();
            rootPage = this;
            viewModel.rootPage = rootPage;
            CreateContent();
        }

        private void CreateContent()
        {

        }

        private void SetViewText(object sender, IRequestQuery args)
        {
            var query = args.QueryText;
            var request = args.Request;

            //var primaryParas = viewModel.ParagraphTuples.Select(p => p.PrimaryParagraph);
            //var secondaryParas = viewModel.ParagraphTuples.Select(p => p.SecondaryParagraph);

            foreach (var tuple in viewModel.ParagraphTuples)
            {
                var primaryPara = tuple.PrimaryParagraph;
                var secondaryPara = tuple.SecondaryParagraph;
                SetRichTextBlockText(primaryPara, secondaryPara);
            }
        }
        private void SetRichTextBlockText(IParagraph primaryPara, IParagraph secondaryPara)
        {
            var primaryLang = primaryPara.Language.EnglishName;
            var secondaryLang = secondaryPara.Language.Name;
            var secondaryText = secondaryPara.Text;
            var rtb = CreateRichTextBlock(primaryPara, primaryLang);
            rtb.SetValue(AttachedProperties.SecondLanguage.CultureProperty, secondaryLang);
            rtb.SetValue(AttachedProperties.SecondLanguage.TextProperty, secondaryText);
            MainStackPanel.Children.Add(rtb);
        }

        private RichTextBlock CreateRichTextBlock(IParagraph primaryPara, string primaryLanguage)
        {
            RichTextBlock rtb = new RichTextBlock();
            Paragraph docParagraph = null;
            Run docRun = null;
            string name = string.Empty;
            CultureInfo culture = null;
            string timeSpan = string.Empty;
            TimeSpan? startMarker = null;
            TimeSpan? endMarker = null;
            string text = string.Empty;

            string toolTipPlacement = "Top";
            string toolTipText = _resourceLoader.GetString("ContextMenuTooltip/Text");

            string langPrefix = primaryLanguage.Substring(0, 2);
            Style titleStyle = (Style)this.Resources[langPrefix + "TitleParagraph"];
            Style regularStyle = (Style)this.Resources[langPrefix + "RegularParagraph"];

            switch (primaryLanguage)
            {
                case "English":
                { 
                    var paragraph = (EnglishParagraph)primaryPara;
                    name = paragraph.Name;
                    timeSpan = paragraph.Timespan;
                    startMarker = paragraph.StartMarker;
                    endMarker = paragraph.EndMarker;
                    text = paragraph.Text;
                    break;
                }
                case "Japanese":
                { 
                    var paragraph = (JapaneseParagraph)primaryPara;
                    var prefix = paragraph.Name.Split('_')[0];
                    var postfix = paragraph.Name.Split('_')[2];
                    Style style = paragraph.SequenceID == 0 ? titleStyle : regularStyle;
                    name = paragraph.Name;
                    culture = paragraph.Language;
                    timeSpan = paragraph.Timespan;
                    startMarker = paragraph.StartMarker;
                    endMarker = paragraph.EndMarker;
                    text = paragraph.Text;
                    var pauseText = paragraph.PauseText;
                    var pauseMarker = paragraph.PauseMarker;
                    var rubyText = paragraph.RubyText;
                    docParagraph = new Paragraph() { Language = culture.Name};
                    docParagraph.SetValue(NameProperty, "Paragraph_" + postfix);
                    docRun = new Run() { Text = text };
                    docRun.SetValue(NameProperty, "Run_" + postfix);
                    docParagraph.Inlines.Add(docRun);
                    rtb = CreateRichTextBlock(name, 
                                              culture, 
                                              docParagraph, 
                                              timeSpan, 
                                              toolTipPlacement, 
                                              toolTipText, 
                                              style, 
                                              "", // string secondLangCulture
                                              "", // string secondLangText
                                              pauseMarker, 
                                              pauseText, 
                                              rubyText);
                    break;
                }
                default:
                    break;
            };
            return rtb;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (viewModel.Status == "ViewModelCStor")
                viewModel.rootPage = rootPage;

            base.OnNavigatedTo(e);
            if (viewModel != null)
            {
                viewModel.PlayAudioRequested += PlayAudio;
                viewModel.PauseAudioRequested += PauseAudio;
                viewModel.StopAudioRequested += StopAudio;
                viewModel.PlayAudioRangeRequested += PlayAudioRange;
                viewModel.SetViewTextRequested += SetViewText;
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (viewModel != null)
            {
                viewModel.PlayAudioRequested -= PlayAudio;
                viewModel.PauseAudioRequested -= PauseAudio;
                viewModel.StopAudioRequested -= StopAudio;
                viewModel.PlayAudioRangeRequested -= PlayAudioRange;
                viewModel.SetViewTextRequested -= SetViewText;
            }
        }

        private void SetSystemMediaTransportControls()
        {
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
            //this.mediaElement.CurrentStateChanged += OnMediaElementCurrentStateChanged;

            // Update the system-wide state whenever the MediaElement state changes
            //_mediaControls.ButtonPressed += OnSystemMediaTransportControlsStateChanged;
        }

        private RichTextBlock CreateRichTextBlock(string name, 
                                                  CultureInfo cultureInfo, 
                                                  Paragraph paragraph,
                                                  string timeSpan,
                                                  string toolTipPlacement,
                                                  string toolTipText, 
                                                  Style style,
                                                  string secondLangCulture,
                                                  string secondLangText,
                                                  char pauseMarker,
                                                  string pauseText,
                                                  string rubyText)
        {
            RichTextBlock rtb = new RichTextBlock();
            rtb.Name = name;
            rtb.Language = cultureInfo.TwoLetterISOLanguageName;
            rtb.Style = style;
            rtb.IsTextSelectionEnabled = true;
            rtb.IsRightTapEnabled = true;
            rtb.IsDoubleTapEnabled = true;
            rtb.SetValue(ToolTipService.PlacementProperty, toolTipPlacement);
            rtb.SetValue(ToolTipService.ToolTipProperty, toolTipText);
            rtb.SetValue(Audio.TimeSpanProperty, timeSpan);
            rtb.SetValue(PauseRun.IsPauseProperty, pauseText.Length > 0 ? true : false);
            rtb.SetValue(PauseRun.IsVisibleProperty, false);
            rtb.SetValue(PauseRun.PauseMarkProperty, pauseMarker.ToString());
            rtb.SetValue(PauseRun.PauseTextProperty, pauseText);
            rtb.SetValue(RubyRun.IsRubyProperty, true);
            rtb.SetValue(RubyRun.IsVisibleProperty, false);
            rtb.SetValue(RubyRun.RubyTextProperty, rubyText);
            rtb.SetValue(SecondLanguage.CultureProperty, secondLangCulture);
            rtb.SetValue(SecondLanguage.TextProperty, secondLangText);
            rtb.AddHandler(DoubleTappedEvent, new DoubleTappedEventHandler(this.OnDoubleTapped), true);
            SetContextMenu(rtb);
            rtb.Blocks.Add(paragraph);
            return rtb;
        }

        private void OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs args)
        {
            string controlName = string.Empty;
            string tagJson = string.Empty;
            string controlType = string.Empty;
            string[] markers = { "00:00:00", "00:00:00" };
            string controlLanguage = string.Empty;
            // Attached Property
            string controlTimspan = string.Empty;

            // Japanese Text Attached Properties
            bool? isPauseRun = false;
            bool? isRubyRun = false;
            string pauseRun = string.Empty;
            string pauseMark = string.Empty;
            string rubyRun = string.Empty;

            if (args.OriginalSource != null)
            {
                var obj = args.OriginalSource;
                if (obj.GetType().Name.Equals("TextBlock"))
                {
                    controlName = ((TextBlock)obj).Name;
                    tagJson = ((TextBlock)obj).Tag as string;
                    controlType = "TextBlock";

                }
                if (obj.GetType().Name.Equals("RichTextBlock"))
                {
                    controlType = "RichTextBlock";
                    controlName = ((RichTextBlock)obj).Name;
                    controlTimspan = ((RichTextBlock)obj).GetValue(Audio.TimeSpanProperty) as string;
                    controlLanguage = ((RichTextBlock)obj).Language;
                    tagJson = ((RichTextBlock)obj).Tag as string;

                    if (controlLanguage.Equals("ja"))
                    {
                        isPauseRun = ((RichTextBlock)obj).GetValue(PauseRun.IsPauseProperty) as bool?;
                        if (isPauseRun.Value)
                        {
                            pauseRun = ((RichTextBlock)obj).GetValue(PauseRun.PauseTextProperty) as string;
                            pauseMark = ((RichTextBlock)obj).GetValue(PauseRun.PauseMarkProperty) as string;
                        }
                        isRubyRun = ((RichTextBlock)obj).GetValue(RubyRun.IsRubyProperty) as bool?;
                        if (isRubyRun.Value)
                        {
                            rubyRun = ((RichTextBlock)obj).GetValue(RubyRun.RubyTextProperty) as string;
                        }
                    }

                    markers = controlTimspan.Split('_');
                    var audioSetting = new CurrentAudioInfoDto.AudioSetting
                    {
                        ParagraphName = controlName,
                        StartMarker = markers[0],
                        EndMarker = markers[1]
                    };

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(audioSetting);

                    IDoubleTappedArgs item = new DoubleTappedArgs(controlName, controlType, tagJson)
                    {
                        TargetName = controlName,
                        TargetType = controlType,
                        TargetJson = json
                    };

                    viewModel.DoubleTappedCommand.Execute(item);
                }
            }
        }

        private void PlayAudio(object sender, IAudioMediaEventArgs args)
        {
            this.mediaElement.Play();
        }

        private void PauseAudio(object sender, IAudioMediaEventArgs args)
        {
            this.mediaElement.Pause();
        }

        private void StopAudio(object sender, IAudioMediaEventArgs args)
        {
            this.mediaElement.Stop();

            JObject json = JObject.Parse(args.TargetJson);
            var markerDto = new MarkerReachedDto()
            {
                ParagraphName = (string)json["paragraphName"],
                Marker = (string)json["marker"],
                ActionRequest = (string)json["actionRequest"]
            };

            // Set ContextMenu Items Visibility
            string postFix = args.TargetName.Split('_')[2];
            var targetRichTextBlock = args.TargetName;
            RichTextBlock rtb = (RichTextBlock)this.FindName(targetRichTextBlock);
            MenuFlyout menuFlyout = (MenuFlyout)rtb.FindName("ContextMenu_" + postFix);
            var items = menuFlyout.Items;
            SetVisibility("PlayAudioFlyout_" + postFix, items, Visibility.Visible);
            SetVisibility("PauseAudioFlyout_" + postFix, items, Visibility.Collapsed);
            SetVisibility("StopAudioFlyout_" + postFix, items, Visibility.Collapsed);
        }

        private void PlayAudioRange(object sender, IAudioMediaEventArgs args)
        {
            MediaElement mediaElement = this.FindName("mediaElement") as MediaElement;
            var mediaSource = mediaElement.Source as Uri;

            JObject json = JObject.Parse(args.TargetJson);
            var audioSetting = new CurrentAudioInfo.AudioSetting
            {
                ParagraphName = (string)json["paragraphName"],
                StartMarker = (string)json["startMarker"],
                EndMarker = (string)json["endMarker"]
            };

            // Note: this code copied from Ex file that uses
            // View Model rather than code behind. Was off by
            // 1 in args.TargetName.Split('_')[2];
            string postFix1 = args.TargetName.Split('_')[2];
            var targetRichTextBlock1 = args.TargetName;
            RichTextBlock rtb1 = (RichTextBlock)this.FindName(targetRichTextBlock1);
            var controlType = "RichTextBlock";
            var controlName = rtb1.Name;

            var json2 = Newtonsoft.Json.JsonConvert.SerializeObject(audioSetting);
            ITappedArgs item = new TappedArgs(controlName, controlType, json2)
            {
                TargetName = controlName,
                TargetType = controlType,
                TargetJson = json2
            };
            // end debugging code

            string[] start = audioSetting.StartMarker.Split(':');
            string[] end = audioSetting.EndMarker.Split(':');

            TimeSpan startTime = new TimeSpan(Int32.Parse(start[0]), Int32.Parse(start[1]), Int32.Parse(start[2])/*, Int32.Parse(start[3])*/);
            TimeSpan endTime = new TimeSpan(Int32.Parse(end[0]), Int32.Parse(end[1]), Int32.Parse(end[2])/*, Int32.Parse(end[3])*/);

            mediaElement.Markers.Clear();
            TimelineMarker marker = new TimelineMarker();
            marker.Text = audioSetting.ParagraphName;
            marker.Time = endTime;

            if (mediaElement.CurrentState.Equals(MediaElementState.Playing))
                mediaElement.Stop();
            mediaElement.Position = startTime;
            mediaElement.Markers.Add(marker);

            // Set ContextMenu Items Visibility
            string postFix = args.TargetName.Split('_')[2];
            var targetRichTextBlock = args.TargetName;
            RichTextBlock rtb = (RichTextBlock)this.FindName(targetRichTextBlock);
            MenuFlyout menuFlyout = (MenuFlyout)rtb.FindName("ContextMenu_" + postFix);
            var items = menuFlyout.Items;
            SetVisibility("PlayAudioFlyout_" + postFix, items, Visibility.Collapsed);
            SetVisibility("PauseAudioFlyout_" + postFix, items, Visibility.Visible);
            SetVisibility("StopAudioFlyout_" + postFix, items, Visibility.Visible);

            mediaElement.Play();
        }

        #region // IAsync Methods
        private IAsyncOperation<IUICommand> ShowAlertAsync(string message)
        {
            MessageDialog dialog = new MessageDialog(message != null ? message : string.Empty);
            return dialog.ShowAsync();
        }
        private IAsyncAction PlayAudioRangeActionAsync(string message)
        {
            return Task.Run(() => { }).AsAsyncAction();
        }
        #endregion

        //private async Task PlayAudioRangeAsync(object sender, IAudioMediaEventArgs args)
        //{
        //    try
        //    {
        //        await Task.Run(() => PlayAudioRange(sender, args));
        //    }
        //    catch(Exception e)
        //    {
        //        // handle error
        //    }
        //    finally
        //    {
        //        // clean up
        //    }
        //}

        private void MenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var obj = e.OriginalSource;
            MenuFlyoutItem currentMenuFlyoutItem = null;
            string name = string.Empty;
            string tag = string.Empty;
            string text = string.Empty;
            if (obj != null)
            { 
                currentMenuFlyoutItem = (MenuFlyoutItem)obj;
                name = currentMenuFlyoutItem.Name;
                tag  = (string)currentMenuFlyoutItem.Tag;
                text = currentMenuFlyoutItem.Text;
            }
            var prefix = viewModel.PrimaryLanguage.Name;
            var postFix = name.Split('_')[1];
            var targetRichTextBlock = prefix + "_RichTextBlock_" + postFix;
            RichTextBlock rtb = (RichTextBlock)this.FindName(targetRichTextBlock);
            MenuFlyout menuFlyout = (MenuFlyout)rtb.FindName("ContextMenu_" + postFix);
            var items = menuFlyout.Items;


            // Japanese Text Attached Properties
            bool? isPauseRun = false;
            bool? isRubyRun = false;
            isPauseRun = rtb.GetValue(PauseRun.IsPauseProperty) as bool?;
            isRubyRun = rtb.GetValue(RubyRun.IsRubyProperty) as bool?;

            string pauseRun = string.Empty;
            string pauseMark = string.Empty;
            string rubyRun = string.Empty;

            var isRubyTextVisible = rtb.GetValue(RubyRun.IsVisibleProperty) as bool?;
            var isPauseTextVisible = rtb.GetValue(PauseRun.IsVisibleProperty) as bool?;

            var act = name.Split('_')[0];
            Run run;
            switch (act)
            {
                case "CopyFlyout":
                    break;
                case "SelectAllFlyout":
                    break;
                case "VocabFlyout":
                    break;
                case "PlayAudioFlyout":
                    // Check to see if media is already playing and handle here
                    SetVisibility("PlayAudioFlyout_" + postFix, items, Visibility.Collapsed);
                    SetVisibility("PauseAudioFlyout_" + postFix, items, Visibility.Visible);
                    SetVisibility("StopAudioFlyout_" + postFix, items, Visibility.Visible);
                    break;
                case "PauseAudioFlyout":
                    // Check to see if media is already playing and handle here
                    SetVisibility("PlayAudioFlyout_" + postFix, items, Visibility.Visible);
                    SetVisibility("PauseAudioFlyout_" + postFix, items, Visibility.Collapsed);
                    SetVisibility("StopAudioFlyout_" + postFix, items, Visibility.Visible);
                    break;
                case "StopAudioFlyout":
                    // Check to see if media is already playing and handle here
                    SetVisibility("PlayAudioFlyout_" + postFix, items, Visibility.Visible);
                    SetVisibility("PauseAudioFlyout_" + postFix, items, Visibility.Collapsed);
                    SetVisibility("StopAudioFlyout_" + postFix, items, Visibility.Collapsed);
                    break;
                case "RegularTextFlyout":
                    run = (Run)rtb.FindName("Run_" + postFix);
                    var tagText = rtb.GetValue(TagProperty) as string;
                    run.Text = tagText;
                    rtb.SetValue(RubyRun.IsVisibleProperty, false);
                    rtb.SetValue(PauseRun.IsVisibleProperty, false);
                    SetVisibility("PauseTextFlyout_" + postFix, items, Visibility.Visible);
                    SetVisibility("RubyTextFlyout_" + postFix, items, Visibility.Visible);
                    SetVisibility("RegularTextFlyout_" + postFix, items, Visibility.Collapsed);
                    break;
                case "PauseTextFlyout":
                    if (isPauseRun.Value)
                    {
                        isRubyTextVisible = rtb.GetValue(RubyRun.IsVisibleProperty) as bool?;
                        run = (Run)rtb.FindName("Run_" + postFix);
                        if (isRubyTextVisible.Value)
                        {
                            rtb.SetValue(RubyRun.IsVisibleProperty, false);
                        }
                        else
                        {
                            rtb.SetValue(TagProperty, run.Text);
                        }
                        pauseRun = rtb.GetValue(PauseRun.PauseTextProperty) as string;
                        pauseMark = rtb.GetValue(PauseRun.PauseMarkProperty) as string;
                        var spaceMarker = pauseMark.ToCharArray()[0];
                        pauseRun = pauseRun.Replace(spaceMarker, _singleSpaceChar);
                        run.Text = pauseRun;
                        rtb.SetValue(PauseRun.IsVisibleProperty, true);
                        SetVisibility("PauseTextFlyout_" + postFix, items, Visibility.Collapsed);
                        SetVisibility("RubyTextFlyout_" + postFix, items, Visibility.Visible);
                        SetVisibility("RegularTextFlyout_" + postFix, items, Visibility.Visible);
                    }
                    break;
                case "RubyTextFlyout":
                    if (isRubyRun.Value)
                    {
                        isPauseTextVisible = rtb.GetValue(PauseRun.IsVisibleProperty) as bool?;
                        run = (Run)rtb.FindName("Run_" + postFix);
                        if (isPauseTextVisible.Value)
                        {
                            rtb.SetValue(PauseRun.IsVisibleProperty, false);
                        }
                        else
                        {
                            rtb.SetValue(TagProperty, run.Text);
                        }
                        rubyRun = rtb.GetValue(RubyRun.RubyTextProperty) as string;
                        pauseMark = rtb.GetValue(PauseRun.PauseMarkProperty) as string;
                        var spaceMarker = pauseMark.ToCharArray()[0];
                        rubyRun = rubyRun.Replace(spaceMarker, _singleSpaceChar);
                        run.Text = rubyRun;
                        rtb.SetValue(RubyRun.IsVisibleProperty, true);
                        SetVisibility("RubyTextFlyout_" + postFix, items, Visibility.Collapsed);
                        SetVisibility("PauseTextFlyout_" + postFix, items, Visibility.Visible);
                        SetVisibility("RegularTextFlyout_" + postFix, items, Visibility.Visible);
                    }
                    break;
                case "MoreFlyout":
                    break;
            }
        }

        private static void SetVisibility(string name, IList<MenuFlyoutItemBase> items, Visibility visibility)
        {
            var flyoutItem = (MenuFlyoutItem)items
                                .Where(i => i.Name.Equals(name))
                                .FirstOrDefault();
            flyoutItem.Visibility = visibility;
        }

        private void SetContextMenu(UIElement item)
        {
            string resourceName = string.Empty;

            RichTextBlock rtb = (RichTextBlock)item;
            string name = rtb.Name;
            string prefix = name.Split('_')[0];
            string postfix = rtb.Name.Split('_')[2];
            MenuFlyout menuFlyout = new MenuFlyout();
            menuFlyout.SetValue(NameProperty, "ContextMenu_" + postfix);
            // Add Copy MenuFlyoutItem
            var copyItem = new MenuFlyoutItem()
            {
                Name = "CopyFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemCopy/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Copy },
                Tag = rtb.Name,
                Visibility = Visibility.Collapsed
            };
            copyItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemCopyToolTip/Text"));
            copyItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(copyItem);

            // Add Select MenuFlyoutItem
            var selectAllItem = new MenuFlyoutItem()
            {
                Name = "SelectAllFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemSelectAll/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.SelectAll },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            selectAllItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemSelectAllToolTip/Text"));
            selectAllItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(selectAllItem);

            // Add MenuFlyoutItemSeparator Bar
            menuFlyout.Items.Add(new MenuFlyoutSeparator());

            // Add Show Vocabulary MenuFlyoutItem
            var vocabItem = new MenuFlyoutItem()
            {
                Name = "VocabFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemVocab/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Page },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            vocabItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemVocabToolTip/Text"));
            vocabItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(vocabItem);

            // Add Audio Play MenuFlyoutItem
            var playAudioItem = new MenuFlyoutItem()
            {
                Name = "PlayAudioFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemPlayAudio/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Play },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            playAudioItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemPlayAudioToolTip/Text"));
            playAudioItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(playAudioItem);

            // Add Audio Pause MenuFlyoutItem
            var pauseAudioItem = new MenuFlyoutItem()
            {
                Name = "PauseAudioFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemPauseAudio/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Pause },
                Tag = rtb.Name,
                Visibility = Visibility.Collapsed
            };
            pauseAudioItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemPauseAudioToolTip/Text"));
            pauseAudioItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(pauseAudioItem);

            // Add Audio Pause MenuFlyoutItem
            var stopAudioItem = new MenuFlyoutItem()
            {
                Name = "StopAudioFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemStopAudio/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Stop },
                Tag = rtb.Name,
                Visibility = Visibility.Collapsed
            };
            stopAudioItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemStopAudioToolTip/Text"));
            stopAudioItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(stopAudioItem);

            // Add Pause Marker MenuFlyoutItem
            var regularItem = new MenuFlyoutItem()
            {
                Name = "RegularTextFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemRegular/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.View },
                Tag = rtb.Name,
                Visibility = Visibility.Collapsed
            };
            regularItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemRegularToolTip/Text"));
            regularItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(regularItem);

            // Add Pause Marker MenuFlyoutItem
            var pauseMarkerItem = new MenuFlyoutItem()
            {
                Name = "PauseTextFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemPauseMarker/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.View },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            pauseMarkerItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemPauseMarkerToolTip/Text"));
            pauseMarkerItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(pauseMarkerItem);

            // Add Ruby Text MenuFlyoutItem
            var rubyTextItem = new MenuFlyoutItem()
            {
                Name = "RubyTextFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemRuby/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.View },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            rubyTextItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemRubyToolTip/Text"));
            rubyTextItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(rubyTextItem);

            // Add MenuFlyoutItemSeparator Bar
            menuFlyout.Items.Add(new MenuFlyoutSeparator());

            // Add More MenuFlyoutItem
            var moreItem = new MenuFlyoutItem()
            {
                Name = "MoreFlyout_" + postfix,
                Text = _resourceLoader.GetString("MenuFlyoutItemMore/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.More },
                Tag = rtb.Name,
                Visibility = Visibility.Visible
            };
            moreItem.SetValue(ToolTipService.ToolTipProperty,
                _resourceLoader.GetString("MenuFlyoutItemMoreToolTip/Text"));
            moreItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(moreItem);

            // Set item.ContextFlyout Property
            item.ContextFlyout = menuFlyout;
            return;
        }
    }
}