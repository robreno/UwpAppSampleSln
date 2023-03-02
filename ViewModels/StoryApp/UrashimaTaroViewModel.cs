using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Windows.UI.Xaml.Controls;
using Windows.Media;
using Windows.ApplicationModel.Core;
using System.Globalization;
using Windows.UI.Core;
using Windows.UI.Xaml.Documents;

using Newtonsoft.Json.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Text;

using UwpSample.Converters.ValueConverters;
using UwpSample.Services;
using UwpSample.Dtos;
using UwpSample.Models;
using UwpSample.Models.Interfaces;
using Windows.UI.Xaml.Input;



// // https://docs.microsoft.com/en-us/windows/uwp/app-resources/localize-strings-ui-manifest
// https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/commanding
// x:Bind
// https://docs.microsoft.com/en-us/windows/uwp/xaml-platform/x-bind-markup-extension
// Media Player Elment
// See: http://stackoverflow.com/questions/10631748/mvvm-pattern-violation-mediaelement-play

namespace UwpSample.ViewModels
{
    public class UrashimaTaroViewModel : ViewModelBase
    {
        #region // Page and UserControl State
        public Page rootPage;
        public UserControl rootUserControl;
        #endregion

        #region // Media Controls
        SystemMediaTransportControls _mediaControls;
        MediaElement _mediaElement;
        #endregion

        #region // Services
        /// <summary>
        /// The <see cref="IStoryService"/> instance currently in use.
        /// </summary>
        private readonly IStoryService _storyService;

        /// <summary>
        /// The <see cref="IAlertMessageService"/> instance currently in use.
        /// </summary>
        private readonly IAlertMessageService _alertMessageService;

        /// <summary>
        /// The <see cref="IResourceLoaderService"/> instance currently in use.
        /// </summary>
        private readonly IResourceLoaderService _resourceLoader;
        #endregion

        #region // IAsyncRelayCommand 
        public IAsyncRelayCommand GetXDocumentCommand { get; }
        public IAsyncRelayCommand LoadStoryCommand { get; }
        public IAsyncRelayCommand TappedCommand { get; }
        public IAsyncRelayCommand DoubleTappedCommand { get; }
        public IAsyncRelayCommand PlayAudioCommand { get; }
        public IAsyncRelayCommand PauseAudioCommand { get; }
        public IAsyncRelayCommand PlayAudioRangeCommand { get; }
        public IAsyncRelayCommand MarkerReachedCommand { get; }
        public IAsyncRelayCommand MediaEndedCommand { get; }
        public IAsyncRelayCommand MediaStateChangedCommand { get; }
        #endregion

        public UrashimaTaroViewModel()
        {
            _storyService = Ioc.Default.GetRequiredService<IStoryService>();
            _alertMessageService = Ioc.Default.GetRequiredService<IAlertMessageService>();
            _resourceLoader = Ioc.Default.GetRequiredService<IResourceLoaderService>();

            GetXDocumentCommand = new AsyncRelayCommand(GetXDocumentAsync);
            LoadStoryCommand = new AsyncRelayCommand<IRequestStory>(LoadStoryAsync, o => true);
            TappedCommand = new AsyncRelayCommand<ITappedArgs>(TappedAsync, o => true);
            DoubleTappedCommand = new AsyncRelayCommand<IDoubleTappedArgs>(DoubleTappedAsync, o => true);
            PlayAudioCommand = new AsyncRelayCommand<IAudioMediaEventArgs>(PlayAudioAsync, o => true);
            PauseAudioCommand = new AsyncRelayCommand<IAudioMediaEventArgs>(PauseAudioAsync, o => true);
            PlayAudioRangeCommand = new AsyncRelayCommand<IAudioMediaEventArgs>(PlayAudioRangeAsync, o => true);
            MarkerReachedCommand = new AsyncRelayCommand<IMarkerReached>(MarkerReachedFor, o => true);
            MediaEndedCommand = new AsyncRelayCommand<IMediaElementEnded>(MediaEndedFor, o => true);
            MediaStateChangedCommand = new AsyncRelayCommand<IMediaElementStateChanged>(MediaStateChangedFor, o => true);
        }

        #region // EvenHandler Delegates
        public event EventHandler<IAudioMediaEventArgs> PlayAudioRequested;
        public event EventHandler<IAudioMediaEventArgs> PauseAudioRequested;
        public event EventHandler<IAudioMediaEventArgs> StopAudioRequested;
        public event EventHandler<IAudioMediaEventArgs> PlayAudioRangeRequested;
        public event EventHandler<IRequestQuery> SetViewTextRequested;
        #endregion

        #region // ObservableCollections
        private ObservableCollection<Story> _stories = new ObservableCollection<Story>();
        private ObservableCollection<ParagraphTuple> _paragraphTuples = new ObservableCollection<ParagraphTuple>();
        private ObservableCollection<IParagraph> _paragraphs = new ObservableCollection<IParagraph>();

        public ObservableCollection<Story> Stories
        {
            get => _stories;
            set => SetProperty(ref _stories, value);
        }
        public ObservableCollection<ParagraphTuple> ParagraphTuples
        {
            get => _paragraphTuples;
            set => SetProperty(ref _paragraphTuples, value);
        }
        public ObservableCollection<IParagraph> Paragraphs
        {
            get => _paragraphs;
            set => SetProperty(ref _paragraphs, value);
        }
        #endregion

        #region // Properties
        private bool _isStoryLoading;
        public bool IsStoryLoading
        {
            get => _isStoryLoading;
            set => SetProperty(ref _isStoryLoading, value);
        }

        private string _status;
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private string _storyId;
        public string StoryID
        {
            get { return _storyId; }
            set => SetProperty(ref _storyId, value);
        }

        private string _storyName;
        public string StoryName
        {
            get => _storyName;
            set => SetProperty(ref _storyName, value);
        }

        private string _currentStartMarker;
        public string CurrentStartMarker
        {
            get => _currentStartMarker;
            set => SetProperty(ref _currentStartMarker, value);
        }

        private string _currentEndMarker;
        public string CurrentEndMarker
        {
            get => _currentEndMarker;
            set => SetProperty(ref _currentEndMarker, value);
        }

        private TimeSpan _currentMediaPosition;
        public TimeSpan CurrentMediaPosition
        {
            get => _currentMediaPosition;
            set => SetProperty(ref _currentMediaPosition, value);
        }

        private Duration _currentMediaDuration;
        [RestorableState]
        public Duration CurrentMediaDuration
        {
            get => _currentMediaDuration;
            set => SetProperty(ref _currentMediaDuration, value);
        }

        private double _currentMediaProgressValue;
        public double CurrentMediaProgressValue
        {
            get => _currentMediaProgressValue;
            set => SetProperty(ref _currentMediaProgressValue, value);
        }

        private bool _isAudioPlaying;
        public bool IsAudioPlaying
        {
            get => _isAudioPlaying;
            set => SetProperty(ref _isAudioPlaying, value);
        }

        private bool _isAudioPaused;
        public bool IsAudioPaused
        {
            get => _isAudioPaused;
            set => SetProperty(ref _isAudioPaused, value);
        }

        private string _currentParagraphName;
        public string CurrentParagraphName
        {
            get => _currentParagraphName;
            set => SetProperty(ref _currentParagraphName, value);
        }

        CultureInfo _primaryLanguage;
        public CultureInfo PrimaryLanguage
        {
            get { return _primaryLanguage; }
            set => SetProperty(ref _primaryLanguage, value);
        }

        CultureInfo _secondaryLanguage;
        public CultureInfo SecondaryLanguage
        {
            get { return _secondaryLanguage; }
            set => SetProperty(ref _secondaryLanguage, value);
        }

        private XDocument _xdocument;
        public XDocument XDocument
        {
            get => _xdocument;
            set => SetProperty(ref _xdocument, value);
        }
        #endregion

        #region // ViewModel Commands
        private async Task GetXDocumentAsync()
        {
            string errorMessage = string.Empty;
            try
            {
                // Get posts from File
                XDocument = await _storyService.GetXDocumentAsync().ConfigureAwait(true);
                OnPropertyChanged(nameof(GetXDocumentAsync));
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    await _alertMessageService.ShowAsync(errorMessage,
                            _resourceLoader.GetString("/ErrorMessages/Error"));
                }
            }
        }
        private async Task LoadStoryAsync(IRequestStory args)
        {
            IsStoryLoading = true;
            Status = "Loadeding";
            string errorMessage = string.Empty;
            try
            {
                await GetXDocumentAsync();
                InitializeViewModel(args);
                SetParagraphText();
                OnPropertyChanged(nameof(LoadStoryAsync));
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                IsStoryLoading = false;
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    Status = "LoadStoryAsync_Failed";
                    await _alertMessageService.ShowAsync(
                            _resourceLoader.GetString("/ErrorMessages/ErrorServiceUnreachable"),
                            _resourceLoader.GetString("/ErrorMessages/Error"));
                }
                else
                {
                    Status = "Loaded";
                }
            }
        }
        private async Task TappedAsync(ITappedArgs args)
        {
            string errorMessage = string.Empty;
            try
            {
                var paragraphName = args.TargetName;
                var controlType = args.TargetType;
                var targetJson = args.TargetJson;

                JObject o = JObject.Parse(targetJson);
                var audioSetting = new CurrentAudioInfo.AudioSetting()
                {
                    ParagraphName = (string)o["paragraphName"],
                    StartMarker = (string)o["startMarker"],
                    EndMarker = (string)o["endMarker"]
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(audioSetting);
                IAudioMediaEventArgs newItem = new AudioMediaEventArgs()
                {
                    TargetName = paragraphName,
                    TargetType = controlType,
                    TargetJson = json,
                    Message = "PlayAudioRange"
                };

                await PlayAudioRangeAsync(newItem);
                OnPropertyChanged(nameof(TappedAsync));
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                Debug.WriteLine("DoubleTappedForAsync Completed!");
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    await _alertMessageService.ShowAsync(
                            _resourceLoader.GetString("/ErrorMessages/ErrorServiceUnreachable"),
                            _resourceLoader.GetString("/ErrorMessages/Error"));
                }
            }
        }
        private async Task DoubleTappedAsync(IDoubleTappedArgs args)
        {
            string errorMessage = string.Empty;
            try
            {
                var paragraphName = args.TargetName;
                var controlType = args.TargetType;
                var targetJson = args.TargetJson;

                JObject o = JObject.Parse(targetJson);
                var audioSetting = new CurrentAudioInfo.AudioSetting()
                {
                    ParagraphName = (string)o["paragraphName"],
                    StartMarker = (string)o["startMarker"],
                    EndMarker = (string)o["endMarker"]
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(audioSetting);
                IAudioMediaEventArgs newItem = new AudioMediaEventArgs()
                {
                    TargetName = paragraphName,
                    TargetType = controlType,
                    TargetJson = json,
                    Message = "PlayAudioRange"
                };

                await PlayAudioRangeAsync(newItem);
                OnPropertyChanged(nameof(DoubleTappedAsync));
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                Debug.WriteLine("DoubleTappedForAsync Completed!");
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    await _alertMessageService.ShowAsync(
                            _resourceLoader.GetString("/ErrorMessages/ErrorServiceUnreachable"),
                            _resourceLoader.GetString("/ErrorMessages/Error"));
                }
            }
        }
        private async Task PlayAudioAsync(IAudioMediaEventArgs args)
        {
            string errorMessage = string.Empty;
            try
            {
                var paragraphName = args.TargetName;
                var controlType = args.TargetType;
                var message = args.Message;

                JObject o = JObject.Parse(args.TargetJson);
                var audioSetting = new CurrentAudioInfo.AudioSetting()
                {
                    ParagraphName = (string)o["paragraphName"],
                    StartMarker = (string)o["startMarker"],
                    EndMarker = (string)o["endMaker"]
                };

                CurrentParagraphName = audioSetting.ParagraphName;
                CurrentStartMarker = audioSetting.StartMarker;
                CurrentEndMarker = audioSetting.EndMarker;

                var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                await dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                {
                    // Fire the event on UI thread
                    PlayAudioRequested?.Invoke(this, args);
                });
                IsAudioPlaying = true;
                OnPropertyChanged(nameof(PlayAudioAsync));
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    await _alertMessageService.ShowAsync(
                            _resourceLoader.GetString("/ErrorMessages/ErrorServiceUnreachable"),
                            _resourceLoader.GetString("/ErrorMessages/Error"));
                }
            }
        }
        private async Task PauseAudioAsync(IAudioMediaEventArgs args)
        {
            string errorMessage = string.Empty;
            try
            {
                var paragraphName = args.TargetName;
                var controlType = args.TargetType;
                var message = args.Message;

                JObject o = JObject.Parse(args.TargetJson);
                var audioSetting = new CurrentAudioInfo.AudioSetting()
                {
                    ParagraphName = (string)o["paragraphName"],
                    StartMarker = (string)o["startMarker"],
                    EndMarker = (string)o["endMaker"]
                };

                CurrentParagraphName = audioSetting.ParagraphName;
                CurrentStartMarker = audioSetting.StartMarker;
                CurrentEndMarker = audioSetting.EndMarker;

                var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                await dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                {
                    // Fire the event on UI thread
                    PauseAudioRequested?.Invoke(this, args);
                }).AsTask();
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
            finally
            {
                // do nothing
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    await _alertMessageService.ShowAsync(
                            _resourceLoader.GetString("/ErrorMessages/ErrorServiceUnreachable"),
                            _resourceLoader.GetString("/ErrorMessages/Error"));
                }
            }
        }
        private async Task StopAudioAsync(string message)
        {
            string errorMessage = string.Empty;
            try
            {
                JObject o = JObject.Parse(message);
                var audioSetting = new CurrentAudioInfo.AudioSetting()
                {
                    ParagraphName = (string)o["paragraphName"],
                    StartMarker = (string)o["startMarker"],
                    EndMarker = (string)o["endMaker"]
                };

                string paragraphName = audioSetting.ParagraphName;
                string controlType = "RichTextBlock";
                string actionRequest = "PlayAudioRange";

                IAudioMediaEventArgs item = new AudioMediaEventArgs(paragraphName, controlType, message, actionRequest)
                {
                    TargetName = paragraphName,
                    TargetType = controlType,
                    TargetJson = message,
                    Message = actionRequest
                };

                //var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
                // See: http://stackoverflow.com/questions/10579027/run-code-on-ui-thread-in-winrt
                // http://stackoverflow.com/questions/16477190/correct-way-to-get-the-coredispatcher-in-a-windows-store-app
                // ((Page)rootPage).Dispatcher;
                var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                await dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                {
                    // Fire the event on UI thread
                    StopAudioRequested?.Invoke(this, item);
                }).AsTask();
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
            finally
            {
                // do nothing
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    await _alertMessageService.ShowAsync(
                            _resourceLoader.GetString("/ErrorMessages/ErrorServiceUnreachable"),
                            _resourceLoader.GetString("/ErrorMessages/Error"));
                }
            }
        }
        private async Task PlayAudioRangeAsync(IAudioMediaEventArgs args)
        {
            Status = "PlayAudioRangeAsync";
            string errorMessage = string.Empty;
            try
            {
                var paragraphName = args.TargetName;
                var controlType = args.TargetType;
                var targetJson = args.TargetJson;
                var message = args.Message;

                JObject o = JObject.Parse(targetJson);
                var audioSetting = new CurrentAudioInfo.AudioSetting()
                {
                    ParagraphName = (string)o["paragraphName"],
                    StartMarker = (string)o["startMarker"],
                    EndMarker = (string)o["endMarker"]
                };

                CurrentParagraphName = audioSetting.ParagraphName;
                CurrentStartMarker = audioSetting.StartMarker;
                CurrentEndMarker = audioSetting.EndMarker;

                var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                await dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                {
                    // Fire the event on UI thread
                    PlayAudioRangeRequested?.Invoke(this, args);
                });
                IsAudioPlaying = true;
                OnPropertyChanged(nameof(PlayAudioRangeAsync));
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
            finally
            {
                Debug.WriteLine("PlayAudioRangeAsync!");
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    await _alertMessageService.ShowAsync(
                            _resourceLoader.GetString("/ErrorMessages/ErrorServiceUnreachable"),
                            _resourceLoader.GetString("/ErrorMessages/Error"));
                }
            }
        }
        private async Task MarkerReachedFor(IMarkerReached item)
        {
            string errorMessage = string.Empty;
            try
            {
                var markerDto = new MarkerReachedDto()
                {
                    ParagraphName = item.ParagraphName,
                    Marker = item.Marker,
                    ActionRequest = item.ActionRequest
                };
                string paragraphName = markerDto.ParagraphName;
                string marker = markerDto.Marker;
                string actionRequest = markerDto.ActionRequest;
                string controlType = paragraphName.Split('_')[0];
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(markerDto);

                IAudioMediaEventArgs newItem = new AudioMediaEventArgs(paragraphName, controlType, json, actionRequest)
                {
                    TargetName = paragraphName,
                    TargetType = controlType,
                    TargetJson = json,
                    Message = actionRequest
                };

                switch (actionRequest)
                {
                    case "Stop":
                        // Could pass json here for message
                        //await this.StopAudioAsync("mediaElement");
                        var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                        await dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                        {
                            // Fire the event on UI thread
                            StopAudioRequested?.Invoke(this, newItem);
                        });
                        break;
                    default:
                        //await this.Host.StopAudioAsync("mediaElement");
                        break;
                }
                OnPropertyChanged(nameof(MarkerReachedFor));
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                Debug.WriteLine("MarkerReachedFor");
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    await _alertMessageService.ShowAsync(
                            _resourceLoader.GetString("/ErrorMessages/ErrorServiceUnreachable"),
                            _resourceLoader.GetString("/ErrorMessages/Error"));
                }
            }
        }
        private async Task MediaEndedFor(IMediaElementEnded item)
        {
            string errorMessage = string.Empty;
            try
            {
                var param = item.Parameter;

                var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                await dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                {
                    // Fire the event on UI thread
                    //StopAudioRequested?.Invoke(this, item);
                });
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                Debug.WriteLine("MediaEndedFor");
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    await _alertMessageService.ShowAsync(
                            _resourceLoader.GetString("/ErrorMessages/ErrorServiceUnreachable"),
                            _resourceLoader.GetString("/ErrorMessages/Error"));
                }
            }
        }
        private async Task MediaStateChangedFor(IMediaElementStateChanged item)
        {
            string errorMessage = string.Empty;
            try
            {
                var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                await dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                {
                    // Fire the event on UI thread
                    // OnMediaElementCurrentStateChanged(object sender, RoutedEventArgs e)
                    //StopAudioRequested?.Invoke(this, item);
                });
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                Debug.WriteLine("MediaStateChangedFor");
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    await _alertMessageService.ShowAsync(
                            _resourceLoader.GetString("/ErrorMessages/ErrorServiceUnreachable"),
                            _resourceLoader.GetString("/ErrorMessages/Error"));
                }
            }
        }
        private async void SetParagraphText()
        {
            IRequestQuery item = new RequestQuery()
            {
                QueryText = "All",
                Request = "SetText"
            };
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                // Fire the event on UI thread
                SetViewTextRequested?.Invoke(this, item);
            });
            OnPropertyChanged(nameof(SetParagraphText));
        }
        #endregion

        #region // Internal Methods
        private void InitializeViewModel(IRequestStory args)
        {
            string errorMessage = string.Empty;
            try
            {
                StoryID = args.StoryID;
                PrimaryLanguage = new CultureInfo(args.PrimaryLanguage);
                SecondaryLanguage = new CultureInfo(args.SecondaryLanguage);
                var primaryLangPrefix = PrimaryLanguage.TwoLetterISOLanguageName;
                var secondaryLangPrefix = SecondaryLanguage.TwoLetterISOLanguageName;

                var root = XDocument.Root;
                var count = root.Attribute("count").Value;
                var storyElm = root.Descendants("Story")
                    .Where(e => e.Attribute("storyId").Value == StoryID).FirstOrDefault();

                var primaryTitle = storyElm.Attribute(primaryLangPrefix + "Title").Value;
                var secondaryTitle = storyElm.Attribute(secondaryLangPrefix + "Title").Value;

                var story = new Story()
                {
                    ID = Int32.Parse(StoryID),
                    PrimaryLanguage = PrimaryLanguage,
                    SecondaryLanguage = SecondaryLanguage,
                    PriamryLanguageTitle = primaryTitle,
                    SecondaryLanguageTitle = secondaryTitle,
                    Paragraphs = new List<IParagraph>(),
                    ParagraphTuples = new List<ParagraphTuple>()
                };

                var stories = new ObservableCollection<Story>();
                var paragraphTuples = new ObservableCollection<ParagraphTuple>();
                var paragraphs = new ObservableCollection<IParagraph>();
                
                var tuplesElm = storyElm.Descendants("ParagraphTuple");
                ParagraphTuple tuple = null;
                foreach (var paraTuple in tuplesElm)
                {
                    tuple = CreateParagraphTuple(paraTuple);
                    paragraphs.Add(tuple.PrimaryParagraph);
                    story.Paragraphs.Add(tuple.PrimaryParagraph);
                    story.ParagraphTuples.Add(tuple);
                    paragraphTuples.Add(tuple);
                }

                stories.Add(story);
                Stories = stories;
                ParagraphTuples = paragraphTuples;
                Paragraphs = paragraphs;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    _alertMessageService.ShowAsync(
                        _resourceLoader.GetString("/ErrorMessages/ErrorServiceUnreachable"),
                        _resourceLoader.GetString("/ErrorMessages/Error"));
                }
            }
        }
        private ParagraphTuple CreateParagraphTuple(XElement tuple)
        {
            ParagraphTuple newTuple = null;
            var primaryLang = PrimaryLanguage.TwoLetterISOLanguageName;
            var secondaryLang = SecondaryLanguage.TwoLetterISOLanguageName;
            var paragraphs = tuple.Descendants("Paragraph");
            var primaryPara = paragraphs
                              .Where(e => e.Attribute("lang").Value == primaryLang)
                              .FirstOrDefault();
            var secondaryPara = paragraphs
                                .Where(e => e.Attribute("lang").Value == secondaryLang)
                                .FirstOrDefault();

            var primaryParagraph = CreateParagraph(primaryPara);
            var secondaryParagraph = CreateParagraph(secondaryPara);

            newTuple = new ParagraphTuple()
            {
                PrimaryLanguage = new CultureInfo(primaryLang),
                PrimaryParagraph = primaryParagraph,
                SecondaryLanguage = new CultureInfo(secondaryLang),
                SecondaryParagraph = secondaryParagraph
            };
            return newTuple;
        }
        private IParagraph CreateParagraph(XElement element)
        {
            IParagraph newPara = null;
            var sequenceId = element.Attribute("sequenceId").Value;
            var lang = element.Attribute("lang").Value;
            var timeSpan = element.Attribute("timeSpan").Value;
            var style = element.Attribute("style").Value;
            var stylePrefix = lang.Substring(0, 1).ToUpper() + lang.Substring(1, 1);
            var styleName = stylePrefix + style + "Paragraph";

            string controlName = lang + "_RichTextBlock_" + Int32.Parse(sequenceId).ToString("00");
            string[] markers = { "00:00:00", "00:00:00" };
            markers = timeSpan.Split('_');
            var audioSetting = new CurrentAudioInfo.AudioSetting
            {
                ParagraphName = controlName,
                StartMarker = markers[0],
                EndMarker = markers[1]
            };

            string[] start = audioSetting.StartMarker.Split(':');
            string[] end = audioSetting.EndMarker.Split(':');

            TimeSpan startTime = new TimeSpan(Int32.Parse(start[0]), Int32.Parse(start[1]), Int32.Parse(start[2])/*, Int32.Parse(start[3])*/);
            TimeSpan endTime = new TimeSpan(Int32.Parse(end[0]), Int32.Parse(end[1]), Int32.Parse(end[2])/*, Int32.Parse(end[3])*/);

            var contentElm = element.Descendants("Content").FirstOrDefault();
            var textElm = contentElm.Descendants("Text").FirstOrDefault();
            var vocabularyElm = contentElm.Descendants("Vocabulary").FirstOrDefault();

            List<IRun> iruns = CreateIRunList(textElm);
            Paragraph docPara = CraeteDocumentParagraph(iruns);
            docPara.Language = lang;
            IVocabulary ivocab = CreateVocabulary(vocabularyElm);

            string text = GetText(iruns);

            switch (lang)
            {
                case "en":
                    newPara = new EnglishParagraph() 
                    {
                        SequenceID = Int32.Parse(sequenceId),
                        Name = controlName,
                        Timespan = timeSpan,
                        StartMarker = startTime,
                        EndMarker = endTime,
                        Language = new CultureInfo(lang),
                        Paragraph = docPara,
                        Style = styleName,
                        Text = text,
                        Runs = iruns,
                        Vocabulary = ivocab
                    };
                    break;
                case "ja":
                    var pauseElm = textElm.Descendants("PauseRun").FirstOrDefault();
                    bool isPause = pauseElm == null ? false : true;
                    string pauseText = string.Empty;
                    if (isPause)
                        pauseText = pauseElm.Attribute("text").Value;
                    var rubyElm = textElm.Descendants("RubyRun").FirstOrDefault();
                    var rubyText = rubyElm.Attribute("text").Value;
                    newPara = new JapaneseParagraph()
                    {
                        SequenceID = Int32.Parse(sequenceId),
                        Name = controlName,
                        Timespan = timeSpan,
                        StartMarker = startTime,
                        EndMarker = endTime,
                        Language = new CultureInfo(lang),
                        Paragraph = docPara,
                        Style = styleName,
                        Text = text,
                        Runs = iruns,
                        Vocabulary = ivocab,
                        PauseText = pauseText,
                        RubyText = rubyText,
                        PauseMarker = '/',
                        IsPauseText = isPause,
                        IsPauseTextVisible = false,
                        IsRubyText = true,
                        IsRubyTextVisible = false
                    };
                    break;
            }
            return newPara;
        }
        private Paragraph CraeteDocumentParagraph(List<IRun> iruns)
        {
            Paragraph paragraph = new Paragraph();
            foreach (var run in iruns)
            {
                FontStyle style = new FontStyle();
                if (run.FontStyle.Equals("Regular"))
                    style = FontStyle.Normal;
                if (run.FontStyle.Equals("Italic"))
                    style = FontStyle.Italic;
                Run newRun = new Run() { Text = run.Text, FontStyle = style };
                paragraph.Inlines.Add(newRun);
            }
            return paragraph;
        }
        private IVocabulary CreateVocabulary(XElement element)
        {
            IVocabulary newVocab = null;

            var seqId = element.Attribute("sequenceId").Value;
            var lang = element.Attribute("lang").Value;
            var terms = element.Descendants("Term");

            newVocab = new Vocabulary()
            {
                SequenceID = Int32.Parse(seqId),
                Language = new CultureInfo(lang),
                Terms = new List<ITerm>()
            };
            
            List<ITerm> termList = new List<ITerm>();
            foreach (var term in terms)
            {
                var idx = term.Attribute("idx").Value;
                var text = term.Attribute("text").Value;
                var meaning = term.Attribute("meaning").Value;
                var explanation = term.Attribute("explanation");
                var example = term.Attribute("example");

                ITerm newTerm;
                switch (lang)
                {
                    case "en":
                        newTerm = new EnglishTerm()
                        {
                            Index = idx,
                            Text = text,
                            Meaning = meaning,
                            Explanation = explanation == null ? "" : explanation.Value,
                            Example = example == null ? "" : example.Value
                        };
                        newVocab.Terms.Add(newTerm);
                        break;
                    case "ja":
                        var rubyAttr = term.Attribute("rubyText");
                        var rubyText = rubyAttr == null ? "" : rubyAttr.Value;
                        newTerm = new JapaneseTerm()
                        {
                            Index = idx,
                            Text = text,
                            RubyText = rubyText,
                            Meaning = meaning,
                            Explanation = explanation == null ? "" : explanation.Value,
                            Example = example == null ? "" : example.Value
                        };
                        newVocab.Terms.Add(newTerm);
                        break;
                }
            }
            return newVocab;
        }
        private List<IRun> CreateIRunList(XElement element)
        {
            var lang = element.Attribute("lang").Value;
            var prefix = lang.Substring(0, 1).ToUpper() + lang.Substring(1, 1);
            var runs = element.Descendants("RegularRun");
            List<IRun> runList = new List<IRun>();
            foreach (var run in runs)
            {
                var text = run.Attribute("text").Value;
                var fontStyle = run.Attribute("fontStyle").Value;
                IRun newRun = new RegularRun() { Text = text, FontStyle = fontStyle };
                runList.Add(newRun);
            }
            return runList;
        }
        private string GetText(List<IRun> runs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var run in runs)
            {
                sb.Append(run.Text);
            }
            return sb.ToString();
        }
        /// <summary>
        /// Click handler for the "Add marker" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMarker(int h, int m, int s, MediaElement mediaElement)
        {
            TimelineMarker marker = new TimelineMarker();
            TimeSpan end = new TimeSpan(h, m, s);
            marker.Time = end;
            mediaElement.Markers.Add(marker);
        }
        #endregion
    }
}
