using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UwpSample.Models.Interfaces;

namespace UwpSample.Models
{
    public class Content
    {
        private int _sequenceId;
        private string _langPrefix;
        private string _culture;
        private IText _text;
        private IVocabulary _vocabulary;

        public Content() { }
        public Content(int sequenceId, string langPrefix, string culture, IText text, IVocabulary vocabulary)
        {
            SequenceID = sequenceId;
            LanguagePrefix = langPrefix;
            Culture = culture;
            Text = text;
            Vocabulary = vocabulary;
        }
        public string LanguagePrefix
        {
            get => _langPrefix;
            private set => _langPrefix = value;
        }
        public string Culture
        {
            get => _culture;
            private set => _culture = value;
        }
        public int SequenceID 
        { 
            get => _sequenceId; 
            private set => _sequenceId = value; 
        }
        public IText Text 
        { 
            get => _text; 
            private set => _text = value; 
        }
        public IVocabulary Vocabulary 
        { 
            get => _vocabulary; 
            set => _vocabulary = value; 
        }
    }
}
