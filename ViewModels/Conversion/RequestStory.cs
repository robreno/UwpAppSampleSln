namespace UwpSample.ViewModels
{

    public class RequestStory : IRequestStory
    {
        public RequestStory(string id, string primaryLanguage, string secondaryLanguage)
        {
            StoryID = id;
            PrimaryLanguage = primaryLanguage;
            SecondaryLanguage = secondaryLanguage;
        }
        public string StoryID { get; set; }
        public string PrimaryLanguage { get; set; }
        public string SecondaryLanguage { get; set; }
    }
}
