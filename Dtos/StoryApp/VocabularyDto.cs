using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UwpSample.Dtos.Interfaces;

namespace UwpSample.Dtos
{
    public class VocabularyDto : IVocabularyDto
    {
        public VocabularyDto() { }
        public VocabularyDto(int sequenceId, string lang, string culture, List<ITermDto> terms)
        {
            SequenceID = sequenceId;
            LanguagePrefix = lang;
            Culture = culture;
            Terms = terms;
        }
        public int SequenceID { get; set; }
        public string LanguagePrefix { get; set; }
        public string Culture { get; set; }
        public List<ITermDto> Terms { get; set; }
    }
}
