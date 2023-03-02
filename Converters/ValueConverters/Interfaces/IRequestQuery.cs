using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Converters.ValueConverters
{
    public interface IRequestQuery
    {
        string Request { get; set; }
        string QueryText { get; set; }
    }
}
