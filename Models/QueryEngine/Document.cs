using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Models
{
    public class Document
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ETextID { get; set; }
        public string FileName { get; set; }
        public string Uri { get; set; }
    }
}
