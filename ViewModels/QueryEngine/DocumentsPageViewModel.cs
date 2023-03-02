using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

using UwpSample.Models;
using UwpSample.Services;


namespace UwpSample.ViewModels
{
    public class DocumentsPageViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="IDocumentsService"/> instance currently in use.
        /// </summary>
        private readonly IDocumentsService _documentService;

        public DocumentsPageViewModel()
        {
            _documentService = Ioc.Default.GetRequiredService<IDocumentsService>();
            _title = "Document Page";;
            GetXmlCommand = new AsyncRelayCommand(GetDocumentXmlAsync);
            GetXDocumentCommand = new AsyncRelayCommand(GetDocumentXDocumentAsync);
            GetCollectionCommand = new AsyncRelayCommand(GetDocumentCollectionAsync, () => !IsListRefreshing);
            GetAllDataCommand = new AsyncRelayCommand(GetAllDocumentDataAsync);
        }

        public IAsyncRelayCommand GetXmlCommand { get; }
        public IAsyncRelayCommand GetXDocumentCommand { get; }
        public IAsyncRelayCommand GetCollectionCommand { get; }
        public IAsyncRelayCommand GetAllDataCommand { get; }

        #region // TaskNotifier
        //private TaskNotifier<string> _loadXmlSourceTask;
        //public Task<string> LoadXmlSourceTask
        //{
        //    get => _loadXmlSourceTask;
        //    private set
        //    {
        //        SetPropertyAndNotifyOnCompletion(ref _loadXmlSourceTask, value);
        //    }
        //}
        //public string LoadXmlSourceTaskResult =>
        //   LoadXmlSourceTask == null ? "?" : LoadXmlSourceTask.Status == TaskStatus.RanToCompletion 
        //                             ? LoadXmlSourceTask.Result : "(loading xml source)";

        //private TaskNotifier<string> _loadXDocumentTask;
        //public Task<string> LoadXDocumentTask
        //{
        //    get => _loadXDocumentTask;
        //    private set
        //    {
        //        SetPropertyAndNotifyOnCompletion(ref _loadXDocumentTask, value);
        //    }
        //}
        //public string LoadXDocTaskResult =>
        //   LoadXDocumentTask == null ? "?" : LoadXDocumentTask.Status == TaskStatus.RanToCompletion
        //                        ? LoadXDocumentTask.Result : "(loading XDocument)";

        //private TaskNotifier<string> _loadCollectionTask;
        //public Task<string> LoadCollectionTask
        //{
        //    get => _loadCollectionTask;
        //    private set
        //    {
        //        SetPropertyAndNotifyOnCompletion(ref _loadCollectionTask, value);
        //    }
        //}
        //public string LoadCollTaskResult =>
        //   LoadCollectionTask == null ? "?" : LoadCollectionTask.Status == TaskStatus.RanToCompletion
        //                        ? LoadCollectionTask.Result : "(loading collection)";
        #endregion

        private ObservableCollection<Document> _documents = new ObservableCollection<Document>();
        public ObservableCollection<Document> Documents
        {
            get => _documents;
            set => SetProperty(ref _documents, value);
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

        private async Task GetDocumentXmlAsync()
        {
            
            IsListRefreshing = true;
            Status = "Loading";

            try
            {
                // Get posts from File
                DocumentXml = await _documentService.GetXmlStringAsync().ConfigureAwait(true);
                OnPropertyChanged(nameof(GetDocumentXmlAsync));
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

        private async Task GetDocumentXDocumentAsync()
        {
            IsListRefreshing = true;
            Status = "Loading";

            try
            {
                // Get posts from File
                XDocument = await _documentService.GetXDocumentAsync().ConfigureAwait(true);
                OnPropertyChanged(nameof(GetDocumentXDocumentAsync));
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

        private async Task GetDocumentCollectionAsync()
        {
            Debug.WriteLine("Refreshing list...");
            IsListRefreshing = true;
            Status = "Loading";

            try
            {
                // Use ConfigureAwait(true) to callback to the main thread
                // Get posts from File
                Documents = await _documentService.GetCollectionAsync().ConfigureAwait(true);
                Count = Documents.Count;
                OnPropertyChanged(nameof(GetDocumentCollectionAsync));
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

        private async Task GetAllDocumentDataAsync()
        {
            Debug.WriteLine("Refreshing list...");
            IsListRefreshing = true;
            Status = "Loading";

            try
            {
                DocumentXml = await _documentService.GetXmlStringAsync().ConfigureAwait(true);
                var xdoc = XDocument.Parse(DocumentXml);
                XDocument = xdoc;
                var root = xdoc.Root;
                var docs = root.Descendants("Document");
                var documents = new ObservableCollection<Document>();
                Document newDoc = null;
                foreach (var doc in docs)
                {
                    newDoc = new Document()
                    {
                        ID = Int32.Parse(doc.Attribute("id").Value),
                        Title = doc.Attribute("title").Value,
                        ETextID = doc.Attribute("etextId").Value,
                        FileName = doc.Attribute("source").Value,
                        Uri = "ms-appdata:///local/" + doc.Attribute("source").Value
                    };
                    documents.Add(newDoc);
                }
                Documents = documents;
                Count = Documents.Count;
                OnPropertyChanged(nameof(GetAllDocumentDataAsync));
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
