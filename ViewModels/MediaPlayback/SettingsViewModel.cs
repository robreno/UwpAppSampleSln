using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;

using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

using AppCommandingSample.Services;


namespace AppCommandingSample.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        ISettingsService settingsService = Ioc.Default.GetRequiredService<ISettingsService>();
        public bool UseCustomControls
        {
            get
            {
                return settingsService.UseCustomControls;
            }
            set
            {
                if (settingsService.UseCustomControls != value)
                {
                    settingsService.UseCustomControls = value;
                    RaisePropertyChanged("UseCustomControls");
                }
            }
        }
    }
}
