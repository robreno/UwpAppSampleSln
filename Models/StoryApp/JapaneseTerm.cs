using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UwpSample.Models.Interfaces;

namespace UwpSample.Models
{
    public class JapaneseTerm : ITerm
    {
        private string _langPrefix = "Ja";
        private string _culture = "ja-JP";
        public JapaneseTerm() { }
        public JapaneseTerm(string index, string text, string rubyText, string meaning, string explanation = "", string example = "")
        {
            Index = index;
            Text = text;
            RubyText = rubyText;
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
        public string RubyText { get; set; }
    }
}
