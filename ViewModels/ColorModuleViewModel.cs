using Microsoft.Toolkit.Mvvm.Messaging;
using System.Threading.Tasks;
using Windows.UI;
using AppCommandingSample.Models;
using AppCommandingSample.Services.Dialogs;
using AppCommandingSample.Services.Logging;
using AppCommandingSample.Services.Messenger.Messages;

namespace AppCommandingSample.ViewModels
{
    public class ColorModuleViewModel : ViewModelBase
    {
        private Color _color;
        private Theme _theme;
        private ILoggingService _loggingService;
        private ModalView _dialogService;

        public ColorModuleViewModel(ILoggingService loggingService, ModalView modalView)
        {
            _loggingService = loggingService;
            _dialogService = modalView;

            // 'ThemeAwareViewModel'
            _theme =  Messenger.Send<ThemeRequestMessage>();
            _loggingService.Log($"ColorModule requested theme and received {_theme.Name}.");
            if (_theme.Name == "Red")
            {
                Color = Colors.Red;
            }
            else
            {
                Color = Colors.Blue;
            }
        }

        public Color Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        public void LogColor()
        {
            _loggingService.Log($"ColorModule confirms that the color is {ColorHelper.ToDisplayName(_color)}.");
        }

        public async Task<bool> GetUserConsentAsync()
        {
            return await _dialogService.ConfirmationDialogAsync("Do you like this color?", "I'm excited!", "It's gross ...");
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            Messenger.Register<ThemeChangedMessage>(this, (r, m) =>
            {
                _loggingService.Log($"ColorModule received change to {m.Value.Name}.");

                if (m.Value.Name == "Red")
                {
                    Color = Colors.Red;
                }
                else
                {
                    Color = Colors.Blue;
                }
            });
        }

        // Note: This is handled automatically by ObservableRecipient
        //protected override void OnDeactivated()
        //{
        //    Messenger.UnregisterAll(this);
        //    base.OnDeactivated();
        //}
    }
}
