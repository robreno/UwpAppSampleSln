using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UwpSample.Dtos.Interfaces;

namespace UwpSample.Dtos
{
    public class EnglishTermDto : ITermDto
    {
        private string _langPrefix = "En";
        private string _culture = "en-US";
        public EnglishTermDto() { }
        public EnglishTermDto(string index, string text, string meaning, string explanation = "", string example = "")
        {
            Index = index;
            Text = text;
            Meaning = meaning;
            Explanation = explanation;
            Example = example;
        }
        public string Culture
        {
            get => _culture;
        }
        public string LanguagePrefix
        {
            get => _langPrefix;
        }
        public string Index { get; set; }
        public string Text { get; set; }
        public string Meaning { get; set; }
        public string Explanation { get; set; }
        public string Example { get; set; }
    }
}
