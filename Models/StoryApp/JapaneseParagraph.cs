using System;
using System.Collections.Generic;
using System.Globalization;
using UwpSample.Models.Interfaces;
using Windows.UI.Xaml.Documents;

namespace UwpSample.Models
{
    public class JapaneseParagraph : IJapaneseParagraph
    {
        private int _seqId;
        public int SequenceID
        {
            get => _seqId;
            set => _seqId = value;
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private string _timeSpan;
        public string Timespan
        {
            get => _timeSpan;
            set => _timeSpan = value;
        }

        private TimeSpan _startMarker;
        public TimeSpan StartMarker
        {
            get => _startMarker;
            set => _startMarker = value;
        }

        private TimeSpan _endMarker;
        public TimeSpan EndMarker
        {
            get => _endMarker;
            set => _endMarker = value;
        }

        private CultureInfo _language;
        public CultureInfo Language
        {
            get => _language;
            set => _language = value;
        }

        private Paragraph _paragraph;
        public Paragraph Paragraph
        {
            get { return _paragraph; }
            set { _paragraph = value; }
        }

        private string _style;
        public string Style
        {
            get => _style;
            set => _style = value;
        }

        private string _text;
        public string Text
        {
            get => _text;
            set => _text = value;
        }

        private List<IRun> _runsList;
        public List<IRun> Runs
        {
            get => _runsList;
            set => _runsList = value;
        }

        private string _rubyText;
        public string RubyText
        {
            get => _rubyText;
            set => _rubyText = value;
        }

        private string _pauseText;
        public string PauseText 
        { 
            get => _pauseText; 
            set => _pauseText = value; 
        }

        private char _pauseMarker;
        public char PauseMarker
        {
            get => _pauseMarker;
            set => _pauseMarker = value;
        }

        private bool _isPauseText;
        public bool IsPauseText
        {
            get => _isPauseText;
            set => _isPauseText = value;
        }

        private bool _pauseVisible;
        public bool IsPauseTextVisible
        {
            get => _pauseVisible;
            set => _pauseVisible = value;
        }

        private bool _isRubyText;
        public bool IsRubyText 
        { 
            get => _isRubyText; 
            set => _isRubyText = value; 
        }

        private bool _rubyVisible;
        public bool IsRubyTextVisible 
        { 
            get => _rubyVisible; 
            set => _rubyVisible = value; 
        }

        private IVocabulary _vocabulary;
        public IVocabulary Vocabulary 
        { 
            get => _vocabulary; 
            set => _vocabulary = value; 
        }

        public JapaneseParagraph() { }
        public JapaneseParagraph(int seqId,
                                 string name,
                                 string timeSpan,
                                 TimeSpan startTime,
                                 TimeSpan endTime,
                                 CultureInfo language,
                                 Paragraph paragraph,
                                 string style,
                                 string text,
                                 List<IRun> runs,
                                 IVocabulary vocabulary,
                                 string rubyText,
                                 string pauseText,
                                 char pauseMarker,
                                 bool isPause = true,
                                 bool isRuby = true,
                                 bool isPauseVisible = false,
                                 bool isRubyVisible = false)
        {
            SequenceID = seqId;
            Name = name;
            Timespan = timeSpan;
            StartMarker = startTime;
            EndMarker = endTime;
            Language = language;
            Paragraph = paragraph;
            Style = style;
            Text = text;
            Runs = runs;
            Vocabulary = vocabulary;
            RubyText = rubyText;
            PauseText = pauseText;
            PauseMarker = pauseMarker;
            IsPauseText = isPause;
            IsRubyText = isRuby;
            IsPauseTextVisible = isPauseVisible;
            IsRubyTextVisible = isRubyVisible;
        }
    }
}
