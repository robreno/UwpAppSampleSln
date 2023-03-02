using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Dtos.Interfaces
{
    public interface ITextDto
    {
        public int SequenceID { get; set; }
        public string LanguagePrefix { get; set; }
        public string Culture { get; set; }
        public List<IRunDto> Runs { get; set; }
    }
}
