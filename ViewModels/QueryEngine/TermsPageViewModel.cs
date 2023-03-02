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
    public class TermsPageViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="ITermsService"/> instance currently in use.
        /// </summary>
        private readonly ITermsService _termService;

        public TermsPageViewModel()
        {
            _termService = Ioc.Default.GetRequiredService<ITermsService>();
            _title = "Terms Page";
            GetXmlCommand = new AsyncRelayCommand(GetTermXmlAsync);
            GetXDocumentCommand = new AsyncRelayCommand(GetTermXDocumentAsync);
            GetCollectionCommand = new AsyncRelayCommand(GetTermCollectionAsync, () => !IsListRefreshing);
            GetAllDataCommand = new AsyncRelayCommand(GetAllTermDataAsync);
        }

        public IAsyncRelayCommand GetXmlCommand { get; }
        public IAsyncRelayCommand GetXDocumentCommand { get; }
        public IAsyncRelayCommand GetCollectionCommand { get; }
        public IAsyncRelayCommand GetAllDataCommand { get; }

        private ObservableCollection<Term> _terms = new ObservableCollection<Term>();
        public ObservableCollection<Term> Terms
        {
            get => _terms;
            set => SetProperty(ref _terms, value);

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
            set { SetProperty(ref _documentXml, value); DocumentXmlLength = DocumentXml.Length; }
        }

        private XDocument _xdocument;
        public XDocument XDocument
        {
            get => _xdocument;
            set => SetProperty(ref _xdocument, value);
        }

        private async Task GetTermXmlAsync()
        {
            IsListRefreshing = true;
            Status = "Loading";

            try
            {
                // Get posts from File
                DocumentXml = await _termService.GetXmlStringAsync().ConfigureAwait(true);
                OnPropertyChanged(nameof(GetTermXmlAsync));
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

        private async Task GetTermXDocumentAsync()
        {
            IsListRefreshing = true;
            Status = "Loading";

            try
            {
                // Get posts from File
                XDocument = await _termService.GetXDocumentAsync().ConfigureAwait(true);
                OnPropertyChanged(nameof(GetTermXDocumentAsync));
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

        private async Task GetTermCollectionAsync()
        {
            Debug.WriteLine("Refreshing list...");
            IsListRefreshing = true;
            Status = "Loading";

            try
            {
                // Use ConfigureAwait(true) to callback to the main thread
                // Get posts from File
                Terms = await _termService.GetCollectionAsync().ConfigureAwait(true);
                Count = Terms.Count;
                OnPropertyChanged(nameof(GetTermCollectionAsync));
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

        private async Task GetAllTermDataAsync()
        {
            Debug.WriteLine("Refreshing list...");
            IsListRefreshing = true;
            Status = "Loading";

            try
            {
                DocumentXml = await _termService.GetXmlStringAsync().ConfigureAwait(true);
                var xdoc = XDocument.Parse(DocumentXml);
                XDocument = xdoc;
                var root = xdoc.Root;
                var tokens = root.Descendants("Token");
                var terms = new ObservableCollection<Term>();
                Term newTerm = null;
                foreach (var token in tokens)
                {
                    newTerm = new Term()
                    {
                        TokenID = Int32.Parse(token.Attribute("tokenId").Value),
                        StableID= token.Attribute("stableId").Value,
                        Lexeme = token.Attribute("lexeme").Value,
                        Stemmed = token.Attribute("stemmed").Value,

                    };
                    terms.Add(newTerm);
                }
                Terms = terms;
                Count = Terms.Count;
                OnPropertyChanged(nameof(GetAllTermDataAsync));
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
