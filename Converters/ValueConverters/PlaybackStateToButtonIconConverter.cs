using System;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace UwpSample.Converters.ValueConverters
{
    public class PlaybackStateToButtonIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is MediaPlaybackState)
            {
                var state = (MediaPlaybackState)value;

                if (state == MediaPlaybackState.Playing)
                {
                    return Symbol.Pause;
                }
                else
                {
                    return Symbol.Play;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
