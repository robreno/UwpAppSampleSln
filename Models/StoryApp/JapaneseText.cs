using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UwpSample.Models.Interfaces;

namespace UwpSample.Models
{
    public class JapaneseText : IText
    {
        public JapaneseText(int sequenceId, string langPrefix, string langCulture, List<IRun> runs)
        {
            SequenceID = sequenceId;
            LanguagePrefix = langPrefix;
            Culture = langCulture;
            Runs = runs;
        }
        public int SequenceID { get; set; }
        public string LanguagePrefix { get; set; }
        public string Culture { get; set; }
        public List<IRun> Runs { get; set; }
        public PauseRun PauseRun { get; set; }
        public RubyRun RubyRun { get; set; }
    }
}
