using Windows.ApplicationModel.Search;

namespace UwpSample.ViewModels
{
    public interface IRequestStory
    {
        public string StoryID { get; set; }
        public string PrimaryLanguage { get; set; }
        public string SecondaryLanguage { get; set; }
    }
}
