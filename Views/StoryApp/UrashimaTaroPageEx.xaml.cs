using System;
using Windows.Media;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
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


namespace UwpSample.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UrashimaTaroPageEx : Page
    {
        private string _storyId = string.Empty;
        private string _storyLanguage = string.Empty;
        private string _storyName = string.Empty;
        private string _storyTitle = string.Empty;
        private string _storyRubyTitle = string.Empty;
        private string _localAudioUri = string.Empty;

        #region // Services
        private IResourceLoaderService _resourceLoader;
        #endregion

        private Page rootPage;

        public UrashimaTaroPageEx()
        {
            InitializeComponent();
            InitializeContent();
        }

        private void InitializeContent()
        {
            _resourceLoader = Ioc.Default.GetRequiredService<IResourceLoaderService>();
            rootPage = this;
            SetSystemMediaTransportControls();

            var jaItems = JaContentStackPanel.Children
                .Where(c => ((RichTextBlock)c).Name.Contains("_"));

            foreach (var item in jaItems)
            {
                item.AddHandler(TappedEvent, new TappedEventHandler(this.OnTapped), true);
                item.AddHandler(DoubleTappedEvent, new DoubleTappedEventHandler(this.OnDoubleTapped), true);
                item.SetValue(ToolTipService.ToolTipProperty, _resourceLoader.GetString("ContextMenuTooltip/Text"));
                SetContextMenu(item, "ja");
            }

            var enItems = EnContentStackPanel.Children
                .Where(c => ((RichTextBlock)c).Name.Contains("_"));

            foreach (var item in enItems)
            {
                item.AddHandler(TappedEvent, new TappedEventHandler(this.OnTapped), true);
                item.AddHandler(DoubleTappedEvent, new DoubleTappedEventHandler(this.OnDoubleTapped), true);
                item.SetValue(ToolTipService.ToolTipProperty, _resourceLoader.GetString("ContextMenuTooltip/Text"));
                SetContextMenu(item, "en");
            }
        }
        private void SetContextMenu(UIElement item, string langPrefix)
        {
            string resourceName = string.Empty;

            if (langPrefix == "ja")
                SetJapaneseMenu(item, langPrefix);
            if (langPrefix == "en")
                SetEnglishMenu(item, langPrefix);
            return;
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
                SetRichTextBlockText(primaryPara);
                SetRichTextBlockText(secondaryPara);
            }
        }

        private void SetRichTextBlockText(IParagraph p)
        {
            var lang = p.Language.EnglishName;

            if (lang == "Japanese")
            {
                var paragraph = (JapaneseParagraph)p;
                var name = paragraph.Name;
                var timeSpan = paragraph.Timespan;
                var startMarker = paragraph.StartMarker;
                var endMarker = paragraph.EndMarker;
                var text = paragraph.Text;
                var pauseText = paragraph.PauseText;
                var rubyText = paragraph.RubyText;
                var rtb = this.FindName(name) as RichTextBlock;

                rtb.SetValue(AttachedProperties.Audio.TimeSpanProperty, timeSpan);
                rtb.SetValue(AttachedProperties.PauseRun.PauseTextProperty, pauseText);
                rtb.SetValue(AttachedProperties.RubyRun.RubyTextProperty, rubyText);

                var rtbArry = rtb.Name.Split('_');
                var prefix = rtbArry[0] + "_";
                var postfix = "_" + rtbArry[2];
                var rtbRun = rtb.FindName(prefix + "Run" + postfix) as Run;
                rtbRun.Text = text;
            }
            else if (lang == "English")
            {
                var paragraph = (EnglishParagraph)p;
                var name = paragraph.Name;
                var timeSpan = paragraph.Timespan;
                var startMarker = paragraph.StartMarker;
                var endMarker = paragraph.EndMarker;
                var text = paragraph.Text;
                var rtb = this.FindName(name) as RichTextBlock;

                rtb.SetValue(AttachedProperties.Audio.TimeSpanProperty, timeSpan);

                var rtbArry = rtb.Name.Split('_');
                var prefix = rtbArry[0] + "_";
                var postfix = "_" + rtbArry[2];
                var rtbRun = rtb.FindName(prefix + "Run" + postfix) as Run;
                rtbRun.Text = text;
            }
            else
            {
                throw new Exception("Unknown Language.");
            }
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

        private void OnTapped(object sender, TappedRoutedEventArgs args)
        {
            var rtb = (RichTextBlock)sender;
            var controlType = "RichTextBlock";
            var controlName = rtb.Name;
            var timeSpan = rtb.GetValue(Audio.TimeSpanProperty) as string;
            string[] markers = timeSpan.Split('_');
            var audioSetting = new CurrentAudioInfo.AudioSetting
            {
                ParagraphName = controlName,
                StartMarker = markers[0],
                EndMarker = markers[1]
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(audioSetting);
            ITappedArgs item = new TappedArgs(controlName, controlType, json)
            {
                TargetName = controlName,
                TargetType = controlType,
                TargetJson = json
            };
            viewModel.TappedCommand.Execute(item);
        }
        private void OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs args)
        {
            var rtb = (RichTextBlock)sender;
            var controlType = "RichTextBlock";
            var controlName = rtb.Name;
            var timeSpan = rtb.GetValue(Audio.TimeSpanProperty) as string;
            string[] markers = timeSpan.Split('_');
            var audioSetting = new CurrentAudioInfo.AudioSetting
            {
                ParagraphName = controlName,
                StartMarker = markers[0],
                EndMarker = markers[1]
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(audioSetting);
            IDoubleTappedArgs item = new DoubleTappedArgs(controlName, controlType, json)
            {
                TargetName = controlName,
                TargetType = controlType,
                TargetJson = json
            };

            viewModel.DoubleTappedCommand.Execute(item);
        }
        private void OnMenuFlyoutItemClick(object sender, RoutedEventArgs e)
        {
            var obj = e.OriginalSource;
            MenuFlyoutItem currentMenuFlyoutItem = null;
            if (obj != null)
                currentMenuFlyoutItem = (MenuFlyoutItem)obj;
            var name = currentMenuFlyoutItem.Name;
            string tag = (string)currentMenuFlyoutItem.Tag;
            var postFix = tag.Split('_')[2];
            var langPrefix = tag.Split('_')[0];
            var targetRichTextBlock = tag;
            RichTextBlock rtb = (RichTextBlock)this.FindName(targetRichTextBlock);
            MenuFlyout menuFlyout = (MenuFlyout)rtb.FindName(langPrefix + "_ContextMenu_" + postFix);
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

            var controlName = rtb.Name;
            var timeSpan = rtb.GetValue(Audio.TimeSpanProperty) as string;
            string[] markers = timeSpan.Split('_');
            var audioSetting = new CurrentAudioInfo.AudioSetting
            {
                ParagraphName = controlName,
                StartMarker = markers[0],
                EndMarker = markers[1]
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(audioSetting);
            IAudioMediaEventArgs item = new AudioMediaEventArgs()
            {
                TargetName = audioSetting.ParagraphName,
                TargetType = "RichTextBlock",
                TargetJson = json,
                Message = "PlayAudioRange"
            };

            var act = name.Split('_')[1];
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
                    SetVisibility(langPrefix + "_PlayAudioFlyout_" + postFix, items, Visibility.Collapsed);
                    SetVisibility(langPrefix + "_PauseAudioFlyout_" + postFix, items, Visibility.Visible);
                    SetVisibility(langPrefix + "_StopAudioFlyout_" + postFix, items, Visibility.Visible);
                    // Execute ViewModel Command
                    viewModel.PlayAudioRangeCommand.Execute(item);
                    break;
                case "PauseAudioFlyout":
                    // Check to see if media is already playing and handle here
                    SetVisibility(langPrefix + "_PlayAudioFlyout_" + postFix, items, Visibility.Visible);
                    SetVisibility(langPrefix + "_PauseAudioFlyout_" + postFix, items, Visibility.Collapsed);
                    SetVisibility(langPrefix + "_StopAudioFlyout_" + postFix, items, Visibility.Visible);
                    viewModel.PauseAudioCommand.Execute(item);
                    break;
                case "StopAudioFlyout":
                    // Check to see if media is already playing and handle here
                    SetVisibility(langPrefix + "_PlayAudioFlyout_" + postFix, items, Visibility.Visible);
                    SetVisibility(langPrefix + "_PauseAudioFlyout_" + postFix, items, Visibility.Collapsed);
                    SetVisibility(langPrefix + "_StopAudioFlyout_" + postFix, items, Visibility.Collapsed);
                    break;
                case "RegularTextFlyout":
                    run = (Run)rtb.FindName(langPrefix + "_Run_" + postFix);
                    var tagText = rtb.GetValue(TagProperty) as string;
                    run.Text = tagText;
                    rtb.SetValue(RubyRun.IsVisibleProperty, false);
                    rtb.SetValue(PauseRun.IsVisibleProperty, false);
                    SetVisibility(langPrefix + "_PauseTextFlyout_" + postFix, items, Visibility.Visible);
                    SetVisibility(langPrefix + "_RubyTextFlyout_" + postFix, items, Visibility.Visible);
                    SetVisibility(langPrefix + "_RegularTextFlyout_" + postFix, items, Visibility.Collapsed);
                    break;
                case "PauseTextFlyout":
                    if (isPauseRun.Value)
                    {
                        isRubyTextVisible = rtb.GetValue(AttachedProperties.RubyRun.IsVisibleProperty) as bool?;
                        run = (Run)rtb.FindName(langPrefix + "_Run_" + postFix);
                        if (isRubyTextVisible.Value)
                        {
                            rtb.SetValue(AttachedProperties.RubyRun.IsVisibleProperty, false);
                        }
                        else
                        {
                            rtb.SetValue(TagProperty, run.Text);
                        }
                        pauseRun = rtb.GetValue(AttachedProperties.PauseRun.PauseTextProperty) as string;
                        pauseMark = rtb.GetValue(AttachedProperties.PauseRun.PauseMarkProperty) as string;
                        var spaceMarker = pauseMark.ToCharArray()[0];
                        pauseRun = pauseRun.Replace(spaceMarker, _singleSpaceChar);
                        run.Text = pauseRun;
                        rtb.SetValue(AttachedProperties.PauseRun.IsVisibleProperty, true);
                        SetVisibility(langPrefix + "_PauseTextFlyout_" + postFix, items, Visibility.Collapsed);
                        SetVisibility(langPrefix + "_RubyTextFlyout_" + postFix, items, Visibility.Visible);
                        SetVisibility(langPrefix + "_RegularTextFlyout_" + postFix, items, Visibility.Visible);
                    }
                    break;
                case "RubyTextFlyout":
                    if (isRubyRun.Value)
                    {
                        isPauseTextVisible = rtb.GetValue(PauseRun.IsVisibleProperty) as bool?;
                        run = (Run)rtb.FindName(langPrefix + "_Run_" + postFix);
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
                        SetVisibility(langPrefix + "_RubyTextFlyout_" + postFix, items, Visibility.Collapsed);
                        SetVisibility(langPrefix + "_PauseTextFlyout_" + postFix, items, Visibility.Visible);
                        SetVisibility(langPrefix + "_RegularTextFlyout_" + postFix, items, Visibility.Visible);
                    }
                    break;
                case "MoreFlyout":
                    break;
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
            _mediaElement = this.mediaElement;
            var mediaSource = mediaElement.Source as Uri;

            JObject json = JObject.Parse(args.TargetJson);
            var audioSetting = new CurrentAudioInfo.AudioSetting
            {
                ParagraphName = (string)json["paragraphName"],
                StartMarker = (string)json["startMarker"],
                EndMarker = (string)json["endMarker"]
            };

            // Set ContextMenu Items Visibility
            string langPrefix = args.TargetName.Split('_')[0];
            string postFix = args.TargetName.Split('_')[2];
            var targetRichTextBlock = args.TargetName;
            RichTextBlock rtb = (RichTextBlock)this.FindName(targetRichTextBlock);
            MenuFlyout menuFlyout = (MenuFlyout)rtb.FindName(langPrefix + "_ContextMenu_" + postFix);
            var items = menuFlyout.Items;
            SetVisibility(langPrefix + "_PlayAudioFlyout_" + postFix, items, Visibility.Visible);
            SetVisibility(langPrefix + "_PauseAudioFlyout_" + postFix, items, Visibility.Collapsed);
            SetVisibility(langPrefix + "_StopAudioFlyout_" + postFix, items, Visibility.Collapsed);

            if (mediaElement.CurrentState.Equals(MediaElementState.Playing) || 
                mediaElement.CurrentState.Equals(MediaElementState.Paused))
                mediaElement.Stop();
        }
        private void PlayAudioRange(object sender, IAudioMediaEventArgs args)
        {
            mediaElement = this.MediaElement;
            var mediaSource = mediaElement.Source as Uri;

            JObject json = JObject.Parse(args.TargetJson);
            var audioSetting = new CurrentAudioInfo.AudioSetting
            {
                ParagraphName = (string)json["paragraphName"],
                StartMarker = (string)json["startMarker"],
                EndMarker = (string)json["endMarker"]
            };

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
            string langPrefix= args.TargetName.Split('_')[0];
            string postFix = args.TargetName.Split('_')[2];
            var targetRichTextBlock = args.TargetName;
            RichTextBlock rtb = (RichTextBlock)this.FindName(targetRichTextBlock);
            MenuFlyout menuFlyout = (MenuFlyout)rtb.FindName(langPrefix + "_ContextMenu_" + postFix);
            var items = menuFlyout.Items;
            SetVisibility(langPrefix + "_PlayAudioFlyout_" + postFix, items, Visibility.Collapsed);
            SetVisibility(langPrefix + "_PauseAudioFlyout_" + postFix, items, Visibility.Visible);
            SetVisibility(langPrefix + "_StopAudioFlyout_" + postFix, items, Visibility.Visible);

            mediaElement.Play();
        }
    }
}
