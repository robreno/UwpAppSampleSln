using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using UwpSample.Models;
using UwpSample.Services.Messenger.Messages;

namespace UwpSample.ViewModels
{
    public class ShellViewModel : ObservableRecipient
    {
        private Theme _theme = Theme.Default;

        public ShellViewModel()
        {
            Messenger.Register<ThemeRequestMessage>(this, (r, m) =>
            {
                m.Reply(_theme); 
            });

            Messenger.Register<ThemeChangedMessage>(this, (r, m) =>
            {
                _theme = m.Value;
            });
        }
    }
}
