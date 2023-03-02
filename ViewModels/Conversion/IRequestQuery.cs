using Windows.ApplicationModel.Search;

namespace UwpSample.ViewModels
{
    public interface IRequestQuery
    {
        string Request { get; set; }
        string QueryText { get; set; }
    }
}
