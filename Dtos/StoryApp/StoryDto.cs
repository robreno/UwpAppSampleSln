using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UwpSample.Dtos;

namespace UwpSample.Dtos
{
    public class StoryDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public List<LanguageDto> Languages { get; set; }
        public List<ParagraphTupleDto> ParagraphTuples { get; set; }
    }
}
