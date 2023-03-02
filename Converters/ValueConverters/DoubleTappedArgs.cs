namespace UwpSample.Converters.ValueConverters
{

    public class DoubleTappedArgs : IDoubleTappedArgs
    {
        public DoubleTappedArgs(string targetName, string targetType, string targetJson = "")
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
