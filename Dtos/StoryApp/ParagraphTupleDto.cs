using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Dtos
{
    public class ParagraphTupleDto
    {
        public int SequenceID { get; set; }
        public string Langs { get; set; }
        public List<ParagraphDto> Paragraphs { get; set; }
    }
}
