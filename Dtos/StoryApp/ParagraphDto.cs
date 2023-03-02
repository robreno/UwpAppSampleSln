using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Dtos
{
    public class ParagraphDto
    {
        private int _sequenceId;
        private string _langPrefix;
        private string _culture;
        private string _timeSpan;

        public ParagraphDto() { }
        public ParagraphDto(int sequenceId, string langPrefix, string culture)
        {
            SequenceID = sequenceId;
            LanguagePrefix = langPrefix;
            Culture = culture;
        }
        public string LanguagePrefix
        {
            get => _langPrefix;
            set => _langPrefix = value;
        }
        public string Culture
        {
            get => _culture;
            set => _culture = value;
        }
        public int SequenceID 
        { 
            get => _sequenceId; 
            set => _sequenceId = value; 
        }
        public string TimeSpan
        {
            get => _timeSpan;
            set => _timeSpan= value;
        }
        public ContentDto Content { get; set; }
    }
}
