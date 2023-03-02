using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Models
{
    public class Term
    {
        public int TokenID { get; set; }
        public string StableID { get; set; }
        public string Lexeme { get; set; }
        public string Stemmed { get; set; }
    }
}
