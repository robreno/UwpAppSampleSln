using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

using UwpSample.Services;
using UwpSample.Dtos.Interfaces;
using UwpSample.Dtos;

namespace UwpSample.ViewModels
{
    public class StoryPageViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="IStoryService"/> instance currently in use.
        /// </summary>
        private readonly IStoryService _storyService;

        public StoryPageViewModel()
        {
            _storyService = Ioc.Default.GetRequiredService<IStoryService>();
            _title = "Document Page"; ;
            GetXmlCommand = new AsyncRelayCommand(GetDocumentXmlAsync);
            GetXDocumentCommand = new AsyncRelayCommand(GetDocumentXDocumentAsync);
            GetCollectionCommand = new AsyncRelayCommand(GetDocumentCollectionAsync, () => !IsListRefreshing);
            //GetAllDataCommand = new AsyncRelayCommand(GetAllDocumentDataAsync);

            DoubleTappedCommand = new AsyncRelayCommand(OnDoubleTappedCommandAsync);
        }

        public IAsyncRelayCommand GetXmlCommand { get; }
        public IAsyncRelayCommand GetXDocumentCommand { get; }
        public IAsyncRelayCommand GetCollectionCommand { get; }
        public IAsyncRelayCommand GetAllDataCommand { get; }

        public IAsyncRelayCommand DoubleTappedCommand { get; }

        private Dictionary<int, StoryDto> _storyDictionary = new Dictionary<int, StoryDto>();

        private ObservableCollection<StoryDto> _stories = new ObservableCollection<StoryDto>();
        private ObservableCollection<ParagraphTupleDto> _paragraphs = new ObservableCollection<ParagraphTupleDto>();

        public ObservableCollection<StoryDto> Stories
        {
            get => _stories;
            set => SetProperty(ref _stories, value);
        }

        public ObservableCollection<ParagraphTupleDto> Paragraphs
        {
            get => _paragraphs;
            set => SetProperty(ref _paragraphs, value);
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
                DocumentXml = await _storyService.GetXmlStringAsync().ConfigureAwait(true);
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
                XDocument = await _storyService.GetXDocumentAsync().ConfigureAwait(true);
                InitializeViewModel();
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

        private async Task OnDoubleTappedCommandAsync()
        {
            IsListRefreshing = true;
            Status = "Loading";

            try
            {
                // Get posts from File
                XDocument = await _storyService.GetXDocumentAsync().ConfigureAwait(true);
                InitializeViewModel();
                OnPropertyChanged(nameof(OnDoubleTappedCommandAsync));
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

        private void InitializeViewModel()
        {
            var Count = XDocument.Root.Attribute("count").Value;
            var storyElements = XDocument.Root.Descendants("Story");
           
            StoryDto newStory = null;
            foreach (var story in storyElements)
            {
                var paraTuples = story.Descendants("ParagraphTuple");
                var langs = story.Attribute("langs").Value;
                newStory = new StoryDto()
                {
                    ID = Int32.Parse(story.Attribute("storyId").Value),
                    Name = story.Attribute("name").Value,
                    Title = story.Attribute("title").Value,
                    Languages = CreateLanguagesList(langs),
                    ParagraphTuples = new List<ParagraphTupleDto>()
                };

                ParagraphTupleDto newParaTuple = null;
                foreach (var tpl in paraTuples)
                {
                    newParaTuple = new ParagraphTupleDto()
                    {
                        SequenceID = Int32.Parse(tpl.Attribute("sequenceId").Value),
                        Langs = tpl.Attribute("langs").Value,
                        Paragraphs = CreateParagraphList(tpl),
                    };
                    newStory.ParagraphTuples.Add(newParaTuple);
                    Paragraphs.Add(newParaTuple);
                }
                _stories.Add(newStory);
                _storyDictionary.Add(newStory.ID, newStory);
            }
            return;
        }

        private List<ParagraphDto> CreateParagraphList(XElement tpl)
        {
            var paragraphs = tpl.Descendants("Paragraph");
            List<ParagraphDto> paragraphList = new List<ParagraphDto>();
            ParagraphDto newParagraph = null;
            foreach (var para in paragraphs)
            {
                var lang = para.Attribute("lang").Value;
                newParagraph = new ParagraphDto()
                {
                    SequenceID = Int32.Parse(para.Attribute("sequenceId").Value),
                    LanguagePrefix = CreateLanguagePrefix(lang),
                    Culture = CreateCulture(lang),
                    TimeSpan = para.Attribute("timeSpan").Value,
                    Content = CreateContent(para)
                };
                paragraphList.Add(newParagraph);
            };
            return paragraphList;
        }

        private ContentDto CreateContent(XElement para)
        {
            var content = para.Descendants("Content").FirstOrDefault();
            var seqId = Int32.Parse(content.Attribute("sequenceId").Value);
            var lang  = content.Attribute("lang").Value;
            var langPrefix = CreateLanguagePrefix(lang);
            var langCulture = CreateCulture(lang);
            var textRuns = content.Descendants("Text").FirstOrDefault();
            var vocabRuns = content.Descendants("Vocabulary").FirstOrDefault();

            List<IRunDto> runList = null;
            List<ITermDto> vocabList = null;
            ITextDto itext = null;
            IVocabularyDto ivocab = null;
            ContentDto newContent = null;

            var terms = vocabRuns.Descendants("Term");
            var runs = textRuns.Descendants("RegularRun");
            runList = new List<IRunDto>();
            vocabList = new List<ITermDto>();
            foreach (var term in terms)
            {
                var trmIdx = term.Attribute("idx").Value;
                var trmTxt = term.Attribute("text").Value;
                var trmMng = term.Attribute("meaning").Value;
                var trmExp = term.Attribute("explanation");
                var trmExa = term.Attribute("example");
                var trmExpText = string.Empty;
                var trmExaText = string.Empty;
                if (trmExp != null)
                    trmExpText = trmExp.Value;
                if (trmExa != null)
                    trmExaText = trmExa.Value;
                ITermDto newTerm;
                if (lang.Equals("ja"))
                    newTerm = new JapaneseTermDto(trmIdx, trmTxt, trmMng, trmExpText, trmExaText);
                else
                    newTerm = new EnglishTermDto(trmIdx, trmTxt, trmMng, trmExpText, trmExaText);
                vocabList.Add(newTerm);
            };
            ivocab = new VocabularyDto(seqId, langPrefix, langCulture, vocabList);

            switch (lang)
            {
                case "en":
                    foreach (var run in runs)
                    {
                        var text  = run.Attribute("text").Value;
                        var style = run.Attribute("fontSTyle");
                        string fontStyle = string.Empty;
                        if (style == null)
                            fontStyle = "none";
                        else
                            fontStyle = style.Value;

                        var newRun = new RegularRunDto()
                        {
                            Text = run.Attribute("text").Value,
                            FontStyle = fontStyle
                        };
                        runList.Add(newRun);
                    };
                    itext = new EnglishTextDto(seqId, langPrefix, langCulture, runList);
                    newContent = new ContentDto(seqId, langPrefix, langCulture, itext, ivocab);
                    break;
                case "ja":
                    var rglRun = textRuns.Descendants().Where(e => e.Name.LocalName == "RegularRun").FirstOrDefault();
                    var pasRun = textRuns.Descendants().Where(e => e.Name.LocalName == "PauseRun").FirstOrDefault();
                    var rbyRun = textRuns.Descendants().Where(e => e.Name.LocalName == "RubyRun").FirstOrDefault();
                    var regularRun = new RegularRunDto(rglRun.Attribute("text").Value, "");
                    var pauseRun = new PauseRunDto(pasRun.Attribute("text").Value, "");
                    var rubyRun = new RubyRunDto(rbyRun.Attribute("text").Value, "");
                    runList.Add(regularRun);
                    var jText = new JapaneseTextDto(seqId, langPrefix, langCulture, runList);
                    jText.PauseRun = pauseRun;
                    jText.RubyRun = rubyRun;
                    itext = jText;
                    newContent = new ContentDto(seqId, langPrefix, langCulture, itext, ivocab);
                    break;
            }
            return newContent;
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
                Stories = await _storyService.GetCollectionAsync().ConfigureAwait(true);
                Count = Stories.Count;
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

        #region // Utility Methods
        private string CreateCulture(string lang)
        {
            string v = string.Empty;
            switch (lang)
            {
                case "en":
                    v = "en-US";
                    break;
                case "ja":
                    v = "ja-JP";
                    break;
            }
            return v;
        }

        private string CreateLanguagePrefix(string lang)
        {
            string v = string.Empty;
            switch (lang)
            {
                case "en":
                    v = "En";
                    break;
                case "ja":
                    v = "Ja";
                    break;
            }
            return v;
        }

        private List<LanguageDto> CreateLanguagesList(string langs)
        {
            var langArry = langs.Split(';');
            List<LanguageDto> langList = new List<LanguageDto>();
            foreach (var lang in langArry)
            {
                switch (lang)
                {
                    case "en-US":
                        langList.Add(new LanguageDto("en-US", "En", "English"));
                        break;
                    case "ja-JP":
                        langList.Add(new LanguageDto("ja-JP", "En", "Japanese"));
                        break;
                }
            }
            return langList;
        }
        #endregion
    }       
 }
