using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using UwpSample.Views;
using NavigationView = Microsoft.UI.Xaml.Controls.NavigationView;
using NavigationViewItem = Microsoft.UI.Xaml.Controls.NavigationViewItem;
using NavigationViewBackRequestedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs;
using NavigationViewItemInvokedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs;



namespace UwpSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        private readonly IReadOnlyCollection<AppPageView> NavigationItems;

        public Shell()
        {
            this.InitializeComponent();

            NavigationItems = new[]
            {
                new AppPageView(DocumentsPageViewItem, typeof(DocumentsPage)),
                new AppPageView(TermsPageViewItem, typeof(TermsPage)),
                new AppPageView(PostingListsPageViewItem, typeof(PostingListsPage)),
                new AppPageView(JapanesePhoneticsViewItem, typeof(JapanesePhoneticsPage)),
                new AppPageView(MediaPlayerViewItem, typeof(MediaPlayerPage)),
                new AppPageView(SystemMediaPlayerViewItem, typeof(SystemMediaPlayerPage)),
                new AppPageView(JapanesePhoneticsViewItem, typeof(JapanesePhoneticsPage)),
                new AppPageView(StoryAppViewItem, typeof(UrashimaTaroPage)),
                new AppPageView(StoryAppViewUCItem, typeof(UrashimaMainPage)),
                new AppPageView(StoryAppMainViewItem, typeof(StoryAppMainPage)),
            };

            // Set the custom title bar to act as a draggable region
            Window.Current.SetTitleBar(TitleBarBorder);
        }

        // Navigates to a sample page when a button is clicked
        private void NavigationView_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (NavigationItems.FirstOrDefault(item => item.Item == args.InvokedItemContainer)?.PageType is Type pageType)
            {
                NavigationFrame.Navigate(pageType);
            }
        }

        // Sets whether or not the back button is enabled
        private void NavigationFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            NavigationView.IsBackEnabled = ((Frame)sender).BackStackDepth > 0;
        }

        // Navigates back
        private void NavigationView_OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (NavigationFrame.BackStack.LastOrDefault() is PageStackEntry entry)
            {
                NavigationView.SelectedItem = NavigationItems.First(item => item.PageType == entry.SourcePageType).Item;

                NavigationFrame.GoBack();
            }
        }

        // Select the introduction item when the shell is loaded
        private void Shell_OnLoaded(object sender, RoutedEventArgs e)
        {
            //NavigationView.SelectedItem = DocumentsPageViewItem;
            //NavigationFrame.Navigate(typeof(DocumentsPage));

            NavigationView.SelectedItem = StoryAppMainViewItem;
            NavigationFrame.Navigate(typeof(StoryAppMainPage));

        }

        // Updates the search results
        private void SearchBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                // Not a simple tokenized search, but good enough for now
                string query = sender.Text.ToLowerInvariant();

                sender.ItemsSource = NavigationItems.Where(item => item.Tags?.Contains(query) == true);
            }
        }

        // Navigates to a selected item
        private void AutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            AppPageView entry = (AppPageView)args.SelectedItem;

            NavigationFrame.Navigate(entry.PageType);

            NavigationView.SelectedItem = entry.Item;

            sender.Text = string.Empty;
        }
    }

    /// <summary>
    /// A simple model for tracking sample pages associated with buttons.
    /// </summary>
    public sealed class AppPageView
    {
        public AppPageView(NavigationViewItem viewItem, Type pageType, string? name = null, string? tags = null)
        {
            Item = viewItem;
            PageType = pageType;
            Name = name;
            Tags = tags;
        }

        /// <summary>
        /// The navigation item for the current entry.
        /// </summary>
        public NavigationViewItem Item { get; }

        /// <summary>
        /// The associated page type for the current entry.
        /// </summary>
        public Type PageType { get; }

        /// <summary>
        /// Gets the name of the current entry.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Gets the tag for the current entry, if any.
        /// </summary>
        public string? Tags { get; }
    }

}
