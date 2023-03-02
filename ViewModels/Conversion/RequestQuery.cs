using Windows.ApplicationModel.Search;

namespace UwpSample.ViewModels
{

    public class RequestQuery : IRequestQuery
    {
        public string Request { get; set; }
        public string QueryText { get; set; }
        public RequestQuery() { }
        public RequestQuery(string request, string queryText)
        {
            Request = request;
            QueryText = queryText;
        }
    }
}
