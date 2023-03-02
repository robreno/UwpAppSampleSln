using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UwpSample.Dtos.Interfaces;

namespace UwpSample.Dtos
{
    public class ContentDto
    {
        private int _sequenceId;
        private string _langPrefix;
        private string _culture;
        private ITextDto _text;
        private IVocabularyDto _vocabulary;

        public ContentDto() { }
        public ContentDto(int sequenceId, string langPrefix, string culture, ITextDto text, IVocabularyDto vocabulary)
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
        public ITextDto Text 
        { 
            get => _text; 
            private set => _text = value; 
        }
        public IVocabularyDto Vocabulary 
        { 
            get => _vocabulary; 
            set => _vocabulary = value; 
        }
    }
}
