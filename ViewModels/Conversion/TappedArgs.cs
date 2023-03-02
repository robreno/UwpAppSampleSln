namespace UwpSample.ViewModels
{

    public class TappedArgs : ITappedArgs
    {
        public TappedArgs(string targetName, string targetType, string targetJson = "")
        {
            TargetName = targetName;
            TargetType = targetType;
            TargetJson = targetJson;
        }
        public string TargetName { get; set; }
        public string TargetType { get; set; }
        public string TargetJson { get; set; }
    }
}
