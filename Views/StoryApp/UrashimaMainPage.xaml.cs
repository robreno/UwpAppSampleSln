using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;

using UwpSample.Controls;

namespace UwpSample.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UrashimaMainPage : Page
    {
        public UrashimaMainPage()
        {
            this.InitializeComponent();
            LoadContent();
        }

        private void LoadContent()
        {
            BladeView bv = (BladeView)this.FindName("BladeView");
            UserControl jaUC = new JaUrashimaTaroBladeItem();
            UserControl enUC = new EnUrashimaTaroBladeItem();

            BladeItem jaBladeItem = (BladeItem)jaUC.FindName("BladeItem");
            BladeItem enBladeItem = (BladeItem)enUC.FindName("BladeItem");

            jaBladeItem.TitleBarVisibility = Visibility.Collapsed;
            enBladeItem.TitleBarVisibility = Visibility.Collapsed;

            bv.Items.Add(jaUC);
            bv.Items.Add(enUC);
        }
    }
}
