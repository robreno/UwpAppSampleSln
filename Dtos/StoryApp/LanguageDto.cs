using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Dtos
{
    public class LanguageDto
    {
        public LanguageDto(string culture, string prefix, string name)
        {
            Culture = culture;
            Prefix = prefix;
            Name = name;
        }
        public string Culture { get; set; }
        public string Prefix { get; set; }
        public string Name { get; set; }
    }
}
