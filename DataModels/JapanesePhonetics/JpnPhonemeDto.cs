using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.DataModels.JapanesePhonetics
{
    public class JpnPhonemeDto
    {
        public JpnPhonemeDto(string displayText, string yomiText)
        {
            DisplayText = displayText;
            YomiText = yomiText;
            IsRubyText = !DisplayText.Equals(YomiText);
        }

        private string _displayText;
        public string DisplayText
        {
            get => _displayText;
            set => _displayText = value;
        }

        private string _yomiText;
        public string YomiText
        {
            get => _yomiText;
            set => _yomiText = value;
        }

        private bool _isRubyText;
        public bool IsRubyText
        {
            get => _isRubyText;
            set => _isRubyText = value;
        }

        public string CreateTextRun()
        {
            string run = string.Empty;
            if (IsRubyText)
                return _displayText + "(" + _yomiText + ")";
            else
                return _displayText;
        }

    }
}
