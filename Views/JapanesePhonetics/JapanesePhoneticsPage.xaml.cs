using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Text;
using Windows.Globalization;

using UwpSample.DataModels.JapanesePhonetics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UwpSample.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class JapanesePhoneticsPage : Page
    {
        static readonly string para0 = "序文";
        static readonly string para1 = "それがあなたの世界の名前であるユランチアの人間の心には 神、神性、および神格のような用語の意味に関連して多大の混乱がある。人間は、これらの数多くの名称により明示される神性人格の関係性においてより混乱し、また確信がない。かくも多くの思考上の混乱に関わるこの概念上の貧困さゆえに、私は、オーヴォントンの真理の啓示者部隊がユランチアの英語への翻訳を認可されたそれらの論文で使用されるかもしれないように、特定の言語記号に置かれるべき意味の説明においてこの導入声明文を定式化するよう指示を受けた。";
        static readonly string para2 = "宇宙についての意識を拡大し、崇高な認識を強化する我々の努力において拡大された概念と高度の真理を提示することは、領域の限定的言語の使用において制約される我々とり非常に難しい。しかし、我々への命令は、英語の言語記号の使用により我々の意味を伝えるためのあらゆる努力をするようにと我々に諭している。我々は、そのような新概念が部分的に伝えたり、あるいは、多少の意味の歪曲を伝ようとも、英語での何らの専門用語も見いだせない場合に限り、新用語を紹介するように指示されている。";
        public JapanesePhoneticsPage()
        {
            this.InitializeComponent();
        }

        private void AnalyzeButton_Click(object sender, RoutedEventArgs e)
        {
            string jpnComma = "、";
            string[] sourceArray = para1.Split('、');
            string input = this.InputTextBox.Text;
            StringBuilder output = new StringBuilder();

            // monoRuby = false means that each element in the result corresponds to a single Japanese word.
            // monoRuby = true means that each element in the result corresponds to one or more characters which are pronounced as a single unit.
            bool monoRuby = MonoRubyRadioButton.IsChecked == true;

            // Analyze the Japanese text according to the specified algorithm.
            // The maximum length of the input string is 100 characters. 
            IReadOnlyList<JapanesePhoneme> words = JapanesePhoneticAnalyzer.GetWords(input, monoRuby);
            List<JpnPhonemeDto> termList = new List<JpnPhonemeDto>();
            foreach (JapanesePhoneme word in words)
            {
                termList.Add(new JpnPhonemeDto(word.DisplayText, word.YomiText));

                // Put each phrase on its own line.
                if (output.Length != 0 && word.IsPhraseStart)
                {
                    output.AppendLine();
                }
                // DisplayText is the display text of the word, which has same characters as the input of GetWords().
                // YomiText is the reading text of the word, as known as Yomi, which typically consists of Hiragana characters.
                // However, please note that the reading can contains some non-Hiragana characters for some display texts such as emoticons or symbols.
                output.AppendFormat("{0}({1})", word.DisplayText, word.YomiText);
            }



            // Display the result.
            string outputString = output.ToString();
            this.OutputTextBlock.Text = outputString;
        }
    }
}
