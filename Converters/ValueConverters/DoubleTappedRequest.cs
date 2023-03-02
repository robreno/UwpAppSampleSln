using Windows.ApplicationModel.Search;

namespace UwpSample.Converters.ValueConverters
{
    public class DoubleTappedRequest2 : IRequestQuery
    {
        public DoubleTappedRequest2(string request, string queryText)
        {
            Request = request;
            QueryText = queryText;
        }
        public string Request { get; set; }
        public string QueryText { get; set; }
    }
}
