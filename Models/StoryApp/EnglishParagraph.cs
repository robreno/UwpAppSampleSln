using System;
using System.Collections.Generic;
using System.Globalization;
using UwpSample.Models.Interfaces;
using Windows.UI.Xaml.Documents;


namespace UwpSample.Models
{
    public class EnglishParagraph : IParagraph
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
            get => _paragraph;
            set => _paragraph = value;
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

        private IVocabulary _vocabulary;
        public IVocabulary Vocabulary
        {
            get => _vocabulary;
            set => _vocabulary = value;
        }

        public EnglishParagraph() { }
        public EnglishParagraph(int seqId, 
                                string name,
                                string timeSpan,
                                TimeSpan startTime,
                                TimeSpan endTime,
                                CultureInfo language,
                                Paragraph paragraph,
                                string style,
                                string text,
                                List<IRun> runs,
                                IVocabulary vocab
                                )
        {
            SequenceID = seqId; 
            Name = name;
            Language = language;
            Paragraph = paragraph;
            Style = style;
            Text = text;
            Runs = runs;
            Vocabulary = vocab;
        }
    }
}
