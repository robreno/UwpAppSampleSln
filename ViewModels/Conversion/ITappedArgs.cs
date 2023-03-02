using Windows.ApplicationModel.Search;

namespace UwpSample.ViewModels
{
    public interface ITappedArgs
    {
        string TargetName { get; set; }
        string TargetType { get; set; }
        string TargetJson { get; set; }
    }
}
