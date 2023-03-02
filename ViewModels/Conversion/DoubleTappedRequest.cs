using Windows.ApplicationModel.Search;

namespace UwpSample.ViewModels
{

    public class DoubleTappedRequest : IRequestQuery
    {
        public DoubleTappedRequest(string request, string queryText)
        {
            Request = request;
            QueryText = queryText;
        }
        public string Request { get; set; }
        public string QueryText { get; set; }
    }
}
