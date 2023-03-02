using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UwpSample.Dtos.Interfaces;

namespace UwpSample.Dtos
{
    public class EnglishTextDto : ITextDto
    {
        public EnglishTextDto() { }
        public EnglishTextDto(int sequenceId, string langPrefix, string langCulture, List<IRunDto> runs)
        {
            SequenceID = sequenceId;
            LanguagePrefix = langPrefix;
            Culture = langCulture;
            Runs = runs;
        }
        public int SequenceID { get; set; }
        public string LanguagePrefix { get; set; }
        public string Culture { get; set; }
        public List<IRunDto> Runs { get; set; }
    }
}
