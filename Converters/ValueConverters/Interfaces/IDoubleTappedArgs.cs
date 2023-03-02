using Windows.ApplicationModel.Search;

namespace UwpSample.Converters.ValueConverters
{
    public interface IDoubleTappedArgs
    {
        string TargetName { get; set; }
        string TargetType { get; set; }
        string TargetJson { get; set; }
    }
}
