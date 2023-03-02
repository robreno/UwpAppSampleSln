using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

using UwpSample.Models;
using UwpSample.Services;

namespace UwpSample.ViewModels
{
    public class PostingListsPageViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="IPostingListsService"/> instance currently in use.
        /// </summary>
        private readonly IPostingListsService _postingListService;

        public PostingListsPageViewModel()
        {
            _title = "PostingList Page";
            _postingListService = Ioc.Default.GetRequiredService<IPostingListsService>();
            GetXmlCommand = new AsyncRelayCommand(GetPostingListXmlAsync);
            GetXDocumentCommand = new AsyncRelayCommand(GetPostingListXDocumentAsync);
            GetCollectionCommand = new AsyncRelayCommand(GetPostingListCollectionAsync, () => !IsListRefreshing);
            GetAllDataCommand = new AsyncRelayCommand(GetAllPostingListDataAsync, () => !IsListRefreshing);
        }

        public IAsyncRelayCommand GetXmlCommand { get; }
        public IAsyncRelayCommand GetXDocumentCommand { get; }
        public IAsyncRelayCommand GetCollectionCommand { get; }
        public IAsyncRelayCommand GetAllDataCommand { get; }

        private ObservableCollection<PostingList> _postingLists = new ObservableCollection<PostingList>();
        public ObservableCollection<PostingList> PostingLists
        {
            get => _postingLists;
            set => SetProperty(ref _postingLists, value);
        }

        private bool _collectionLoaded;
        public bool CollectionLoaded
        {
            get => _collectionLoaded;
            set => SetProperty(ref _collectionLoaded, value);
        }

        private bool _isListRefreshing;
        public bool IsListRefreshing
        {
            get => _isListRefreshing;
            set => SetProperty(ref _isListRefreshing, value);
        }

        private string _status;
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private int _count;
        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        private int _length;
        public int DocumentXmlLength
        {
            get => _length;
            set => SetProperty(ref _length, value);
        }

        private string _documentXml;
        public string DocumentXml
        {
            get => _documentXml;
            set
            {
                SetProperty(ref _documentXml, value);
                DocumentXmlLength = DocumentXml.Length;
            }
        }

        private XDocument _xdocument;
        public XDocument XDocument
        {
            get => _xdocument;
            set => SetProperty(ref _xdocument, value);
        }

        private async Task GetPostingListXmlAsync()
        {
            IsListRefreshing = true;
            Status = "Loading";

            try
            {
                // Get posts from File
                DocumentXml = await _postingListService.GetXmlStringAsync().ConfigureAwait(true);
                OnPropertyChanged(nameof(GetPostingListXmlAsync));
            }
            catch (Exception e)
            {
                Status = "Failed Loading";
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                Status = "Loaded";
                IsListRefreshing = false;
                Debug.WriteLine("Refreshing list complete!");
            }
        }

        private async Task GetPostingListXDocumentAsync()
        {
            IsListRefreshing = true;
            Status = "Loading";

            try
            {
                // Get posts from File
                XDocument = await _postingListService.GetXDocumentAsync().ConfigureAwait(true);
                OnPropertyChanged(nameof(GetPostingListXDocumentAsync));
            }
            catch (Exception e)
            {
                Status = "Failed Loading";
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                IsListRefreshing = false;
                Status = "Loaded";
                Debug.WriteLine("Refreshing list complete!");
            }
        }

        private async Task GetPostingListCollectionAsync()
        {
            Debug.WriteLine("Refreshing list...");
            IsListRefreshing = true;
            Status = "Loading";

            try
            {
                // Use ConfigureAwait(true) to callback to the main thread

                // Get posts from File
                PostingLists = await _postingListService.GetCollectionAsync().ConfigureAwait(true);
                Count = PostingLists.Count;
                OnPropertyChanged(nameof(GetPostingListCollectionAsync));
            }
            catch (Exception e)
            {
                Status = "Failed Loading";
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                IsListRefreshing = false;
                Status = "Loaded";
                Debug.WriteLine("Refreshing list complete!");
            }
        }

        private async Task GetAllPostingListDataAsync()
        {
            Debug.WriteLine("Refreshing list...");
            IsListRefreshing = true;
            Status = "Loading";

            try
            {
                DocumentXml = await _postingListService.GetXmlStringAsync().ConfigureAwait(true);
                var xdoc = XDocument.Parse(DocumentXml);
                XDocument = xdoc;
                var root = xdoc.Root;
                var plLists = root.Descendants("PostingList");
                var postingLists = new ObservableCollection<PostingList>();
                PostingList newPL = null;
                foreach (var pl in plLists)
                {
                    newPL = new PostingList()
                    {
                        TokenID = Int32.Parse(pl.Attribute("tokenId").Value),
                        StableID = pl.Attribute("stableId").Value,
                        Lexeme = pl.Attribute("lexeme").Value,
                    };
                    postingLists.Add(newPL);
                }
                PostingLists = postingLists;
                Count = PostingLists.Count;
                OnPropertyChanged(nameof(GetAllPostingListDataAsync));
            }
            catch (Exception e)
            {
                Status = "Failed Loading";
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                IsListRefreshing = false;
                Status = "Loaded";
                Debug.WriteLine("Refreshing list complete!");
            }
        }
    }
}
