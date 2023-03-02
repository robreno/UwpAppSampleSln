using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Dtos.Interfaces
{
    public interface IVocabularyDto
    {
        public int SequenceID { get; set; }
        public string LanguagePrefix { get; set; }
        public string Culture { get; set; }
        public List<ITermDto> Terms { get; set; }
    }
}
