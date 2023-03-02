using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using UwpSample.Models;

namespace UwpSample.Services.Messenger.Messages
{
    public class ThemeChangedMessage : ValueChangedMessage<Theme>
    {
        public ThemeChangedMessage(Theme value) : base(value)
        {
        }
    }
}
