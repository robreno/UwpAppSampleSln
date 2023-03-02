using System;
using Windows.UI.Xaml.Data;

using UwpSample.ViewModels;

namespace UwpSample.Converters.ValueConverters
{
    class RequestStoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string languages)
        {
            string storyId = (string)parameter;
            string langs = (string)languages;
            string primaryLangauge = langs.Split('|')[0];
            string secondaryLanguage = langs.Split('|')[1];
            return new RequestStory(storyId, primaryLangauge, secondaryLanguage);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string languages)
        {
            throw new NotImplementedException();
        }
    }
}
