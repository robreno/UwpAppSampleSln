using Windows.ApplicationModel.Search;

namespace UwpSample.ViewModels
{
    public interface IDoubleTappedArgs
    {
        string TargetName { get; set; }
        string TargetType { get; set; }
        string TargetJson { get; set; }
    }
}
