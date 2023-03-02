using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Documents;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

// Globalization Libraries
using Windows.Globalization;
using Windows.Globalization.Collation;
using Windows.Globalization.DateTimeFormatting;
using Windows.Globalization.Fonts;
using Windows.Globalization.NumberFormatting;

using UwpSample.ViewModels;
using UwpSample.Dtos;
using UwpSample.AttachedProperties;
using Microsoft.Toolkit.Uwp.UI.Controls;


namespace UwpSample.Controls
{
    public sealed partial class JaUrashimaTaroBladeItem : UserControl
    {
        private string _targetControlName = string.Empty;
        private char _pauseMarker = '/'; 
        private char _singleSpaceChar = ' ';

        private string _storyId = string.Empty;
        private string _storyLanguage = string.Empty;
        private string _storyName = string.Empty;
        private string _storyTitle = string.Empty;
        private string _storyRubyTitle = string.Empty;
        private string _localAudioUri = string.Empty;

        public JaUrashimaTaroBladeItem()
        {
            this.InitializeComponent();

            InitializeViewModel();

            _storyId = Resources.Where(p =>
            {
                return (p.Key as string) == "storyID";
            }).FirstOrDefault().Value as string;

            _storyLanguage = Resources.Where(p =>
            {
                return (p.Key as string) == "storyLanguage";
            }).FirstOrDefault().Value as string;

            _storyName = Resources.Where(p =>
            {
                return (p.Key as string) == "storyName";
            }).FirstOrDefault().Value as string;

            _storyTitle = Resources.Where(p =>
            {
                return (p.Key as string) == "storyTitle";
            }).FirstOrDefault().Value as string;

            _storyRubyTitle = Resources.Where(p =>
            {
                return (p.Key as string) == "storyRubyTitle";
            }).FirstOrDefault().Value as string;

            _localAudioUri = Resources.Where(p =>
            {
                return (p.Key as string) == "localAudioUri";
            }).FirstOrDefault().Value as string;

            // TappedEventHandler
            // RightTappedEventHandler
            // SelectionChangedEventHandler

            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            var items = ContentStackPanel.Children
                .Where(c => ((RichTextBlock)c).Name.Contains("_"));

            foreach (var item in items)
            {
                item.AddHandler(DoubleTappedEvent, new DoubleTappedEventHandler(this.OnDoubleTapped), true);
                item.SetValue(ToolTipService.ToolTipProperty, resourceLoader.GetString("ContextMenuTooltip/Text"));
                SetContextMenu(item);
            }
        }

        private void InitializeViewModel()
        {
            viewModel.rootUserControl = this;
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

                    //viewModel.DoubleTappedRequest.Execute(item);
                }
            }
        }

        public async Task PlayAudioRangeUI(string message)
        {
            await Task.Run(() => Debug.WriteLine("Handling Double Tapped"));
        }

        //internal static Windows.Foundation.IAsyncAction PlayAudioRangeAsync(string message)
        //{
        //}


        // https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/menus
        // https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.symbol?view=winrt-19041
        // https://docs.microsoft.com/en-us/windows/uwp/design/style/icons
        // https://github.com/microsoft/Windows-universal-samples/blob/master/Samples/XamlContextMenu/shared/Scenario1.xaml

        // RichTextBlock Text Selection
        // https://stackoverflow.com/questions/51243602/richtextblock-selected-text-uwp

        private void RichTextBlock_SelectionChanged(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var obj = e.OriginalSource;
            RichTextBlock rtb = (RichTextBlock)sender;
            var postFix = rtb.Name.Split('_')[1];
            MenuFlyout menuFlyout = (MenuFlyout)rtb.FindName("ContextMenu_" + postFix);
            var items = menuFlyout.Items;

            //var isRubyTextVisible = rtb.GetValue(AttachedProperties.RubyRun.IsVisibleProperty) as bool?;
            //var isPauseTextVisible = rtb.GetValue(AttachedProperties.PauseRun.IsVisibleProperty) as bool?;

            //bool makeRegularTextVisible = (isRubyTextVisible.Value || isPauseTextVisible.Value);
            //if (makeRegularTextVisible)
            //{
            //    var regularFlyoutItem = (MenuFlyoutItem)items
            //    .Where(i => i.Name.Equals("RegularFlyout_" + postFix))
            //    .FirstOrDefault();
            //    regularFlyoutItem.Visibility = Visibility.Visible;
            //}
            
            var copyFlyoutItem = (MenuFlyoutItem)items
                .Where(i => i.Name.Equals("CopyFlyout_" + postFix))
                .FirstOrDefault();
            var selectAllFlyoutItem = (MenuFlyoutItem)items
                    .Where(i => i.Name.Equals("SelectAllFlyout_" + postFix))
                    .FirstOrDefault();

            Run run = (Run)rtb.FindName("Run_" + postFix);
            string runText = run.Text;
            if (rtb.SelectedText.Length > 0)
            {
                copyFlyoutItem.Visibility = Visibility;
                string selectedText = rtb.SelectedText;
                if (selectedText != runText)
                    selectAllFlyoutItem.Visibility = Visibility;
                else
                    selectAllFlyoutItem.Visibility = Visibility.Collapsed;
            }
            else
            {
                copyFlyoutItem.Visibility = Visibility.Collapsed;
                selectAllFlyoutItem.Visibility = Visibility;
            }
        }

        //private void RichTextBlock_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        //{
        //    var obj = e.OriginalSource;
        //    e.Handled = true;
        //    RichTextBlock richTextBlock = (RichTextBlock)sender;
        //    var postFix = richTextBlock.Name.Split('_')[1];
        //    MenuFlyout menuFlyout = (MenuFlyout)richTextBlock.FindName("ContextMenu_" + postFix);
        //    var items = menuFlyout.Items;
        //    if (richTextBlock.SelectedText.Length > 0)
        //    {
        //        var copyItem = (MenuFlyoutItem)richTextBlock.FindName("CopyFlyout_" + postFix);
        //        copyItem.Visibility = Visibility;
        //    }
        //    else
        //    {
        //        var copyItem = (MenuFlyoutItem)richTextBlock.FindName("CopyFlyout_" + postFix);
        //        copyItem.Visibility = Visibility.Collapsed;
        //        items[1].Visibility = Visibility;
        //    }
        //}

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var obj = e.OriginalSource;
            MenuFlyoutItem currentMenuFlyoutItem = null;
            if (obj != null)
                currentMenuFlyoutItem = (MenuFlyoutItem)obj;
            var name = currentMenuFlyoutItem.Name;
            var postFix = name.Split('_')[1];
            var targetRichTextBlock = "RichTextBlock_" + postFix;
            RichTextBlock rtb = (RichTextBlock)this.FindName(targetRichTextBlock);
            MenuFlyout menuFlyout = (MenuFlyout)rtb.FindName("ContextMenu_" + postFix);
            var items = menuFlyout.Items;


            // Japanese Text Attached Properties
            bool? isPauseRun = false;
            bool? isRubyRun = false;
            isPauseRun = rtb.GetValue(AttachedProperties.PauseRun.IsPauseProperty) as bool?;
            isRubyRun = rtb.GetValue(AttachedProperties.RubyRun.IsRubyProperty) as bool?;

            string pauseRun = string.Empty;
            string pauseMark = string.Empty;
            string rubyRun = string.Empty;

            var isRubyTextVisible = rtb.GetValue(AttachedProperties.RubyRun.IsVisibleProperty) as bool?;
            var isPauseTextVisible = rtb.GetValue(AttachedProperties.PauseRun.IsVisibleProperty) as bool?;

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
                case "PlayFlyout":
                    break;
                case "RegularTextFlyout":
                    run = (Run)rtb.FindName("Run_" + postFix);
                    var tagText = rtb.GetValue(TagProperty) as string;
                    run.Text = tagText;
                    rtb.SetValue(AttachedProperties.RubyRun.IsVisibleProperty, false);
                    rtb.SetValue(AttachedProperties.PauseRun.IsVisibleProperty, false);
                    SetVisibility("PauseTextFlyout_" + postFix, items, Visibility.Visible);
                    SetVisibility("RubyTextFlyout_" + postFix, items, Visibility.Visible);
                    SetVisibility("RegularTextFlyout_" + postFix, items, Visibility.Collapsed);
                    break;
                case "PauseTextFlyout":
                    if (isPauseRun.Value)
                    {
                        isRubyTextVisible = rtb.GetValue(AttachedProperties.RubyRun.IsVisibleProperty) as bool?;
                        run = (Run)rtb.FindName("Run_" + postFix);
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
                        SetVisibility("PauseTextFlyout_" + postFix, items, Visibility.Collapsed);
                        SetVisibility("RubyTextFlyout_" + postFix, items, Visibility.Visible);
                        SetVisibility("RegularTextFlyout_" + postFix, items, Visibility.Visible);
                    }
                    break;
                case "RubyTextFlyout":
                    if (isRubyRun.Value)
                    {
                        isPauseTextVisible = rtb.GetValue(AttachedProperties.PauseRun.IsVisibleProperty) as bool?;
                        run = (Run)rtb.FindName("Run_" + postFix);
                        if (isPauseTextVisible.Value)
                        {
                            rtb.SetValue(AttachedProperties.PauseRun.IsVisibleProperty, false);
                        }
                        else
                        {
                            rtb.SetValue(TagProperty, run.Text);
                        }
                        rubyRun = rtb.GetValue(AttachedProperties.RubyRun.RubyTextProperty) as string;
                        pauseMark = rtb.GetValue(AttachedProperties.PauseRun.PauseMarkProperty) as string;
                        var spaceMarker = pauseMark.ToCharArray()[0];
                        rubyRun = rubyRun.Replace(spaceMarker, _singleSpaceChar);
                        run.Text = rubyRun;
                        rtb.SetValue(AttachedProperties.RubyRun.IsVisibleProperty, true);
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
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            RichTextBlock richTextBlock = (RichTextBlock)item;
            string postfix = richTextBlock.Name.Split('_')[1];
            MenuFlyout menuFlyout = new MenuFlyout();
            menuFlyout.SetValue(NameProperty, "ContextMenu_" + postfix);
            // Add Copy MenuFlyoutItem
            var copyItem = new MenuFlyoutItem()
            {
                Name = "CopyFlyout_" + postfix,
                Text = resourceLoader.GetString("MenuFlyoutItemCopy/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Copy },
                Tag = richTextBlock.Name,
                Visibility = Visibility.Collapsed
            };
            copyItem.SetValue(ToolTipService.ToolTipProperty, resourceLoader.GetString("MenuFlyoutItemCopyToolTip/Text"));
            copyItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(copyItem);

            // Add Select MenuFlyoutItem
            var selectAllItem = new MenuFlyoutItem()
            {
                Name = "SelectAllFlyout_" + postfix,
                Text = resourceLoader.GetString("MenuFlyoutItemSelectAll/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.SelectAll },
                Tag = richTextBlock.Name,
                Visibility = Visibility.Visible
            };
            selectAllItem.SetValue(ToolTipService.ToolTipProperty, resourceLoader.GetString("MenuFlyoutItemSelectAllToolTip/Text"));
            selectAllItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(selectAllItem);

            // Add MenuFlyoutItemSeparator Bar
            menuFlyout.Items.Add(new MenuFlyoutSeparator());

            // Add Show Vocabulary MenuFlyoutItem
            var vocabItem = new MenuFlyoutItem()
            {
                Name = "VocabFlyout_" + postfix,
                Text = resourceLoader.GetString("MenuFlyoutItemVocab/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Page },
                Tag = richTextBlock.Name,
                Visibility = Visibility.Visible
            };
            vocabItem.SetValue(ToolTipService.ToolTipProperty, resourceLoader.GetString("MenuFlyoutItemVocabToolTip/Text"));
            vocabItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(vocabItem);

            // Add Play MenuFlyoutItem
            var playItem = new MenuFlyoutItem()
            {
                Name = "PlayFlyout_" + postfix,
                Text = resourceLoader.GetString("MenuFlyoutItemPlay/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.Play },
                Tag = richTextBlock.Name,
                Visibility = Visibility.Visible
            };
            playItem.SetValue(ToolTipService.ToolTipProperty, resourceLoader.GetString("MenuFlyoutItemPlayToolTip/Text"));
            playItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(playItem);

            // Add Pause Marker MenuFlyoutItem
            var regularItem = new MenuFlyoutItem()
            {
                Name = "RegularTextFlyout_" + postfix,
                Text = resourceLoader.GetString("MenuFlyoutItemRegular/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.View },
                Tag = richTextBlock.Name,
                Visibility = Visibility.Collapsed
            };
            regularItem.SetValue(ToolTipService.ToolTipProperty,
                resourceLoader.GetString("MenuFlyoutItemRegularToolTip/Text"));
            regularItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(regularItem);

            // Add Pause Marker MenuFlyoutItem
            var pauseMarkerItem = new MenuFlyoutItem()
            {
                Name = "PauseTextFlyout_" + postfix,
                Text = resourceLoader.GetString("MenuFlyoutItemPauseMarker/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.View },
                Tag = richTextBlock.Name,
                Visibility = Visibility.Visible
            };
            pauseMarkerItem.SetValue(ToolTipService.ToolTipProperty,
                resourceLoader.GetString("MenuFlyoutItemPauseMarkerToolTip/Text"));
            pauseMarkerItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(pauseMarkerItem);

            // Add Ruby Text MenuFlyoutItem
            var rubyTextItem = new MenuFlyoutItem()
            {
                Name = "RubyTextFlyout_" + postfix,
                Text = resourceLoader.GetString("MenuFlyoutItemRuby/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.View },
                Tag = richTextBlock.Name,
                Visibility = Visibility.Visible
            };
            rubyTextItem.SetValue(ToolTipService.ToolTipProperty,
                resourceLoader.GetString("MenuFlyoutItemRubyToolTip/Text"));
            rubyTextItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(rubyTextItem);

            // Add MenuFlyoutItemSeparator Bar
            menuFlyout.Items.Add(new MenuFlyoutSeparator());

            // Add More MenuFlyoutItem
            var moreItem = new MenuFlyoutItem()
            {
                Name = "MoreFlyout_" + postfix,
                Text = resourceLoader.GetString("MenuFlyoutItemMore/Text"),
                Icon = new SymbolIcon() { Symbol = Symbol.More },
                Tag = richTextBlock.Name,
                Visibility = Visibility.Visible
            };
            moreItem.SetValue(ToolTipService.ToolTipProperty, resourceLoader.GetString("MenuFlyoutItemMoreToolTip/Text"));
            moreItem.Click += new RoutedEventHandler(MenuFlyoutItem_Click);
            menuFlyout.Items.Add(moreItem);

            // Set item.ContextFlyout Property
            item.ContextFlyout = menuFlyout;
            return;
        }
    }
}
