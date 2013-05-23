using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MonoGame.Framework;
using System;
using Hiromi;
using Microsoft.Advertising.WinRT.UI;

namespace Acorn
{
    /// <summary>
    /// The root page used to display the game.
    /// </summary>
    public sealed partial class GamePage : SwapChainBackgroundPanel, IAdRenderer
    {
        readonly AcornGame _game;
        private AdControl _adControl;

#if DEBUG
        private string applicationId = "d25517cb-12d4-4699-8bdc-52040c712cab";
        private string adUnitId = "10042998";
#else
        private string applicationId = "77eed847-a9a0-4ad8-8d01-894fdc36ddf1";
        private string adUnitId = "10072867";
#endif

        public GamePage(string launchArguments)
        {
            this.InitializeComponent();

            // Create the game.
            _game = XamlGame<AcornGame>.Create(launchArguments, Window.Current.CoreWindow, this);
            _game.SetAdRenderer(this);
        }

        private void adControl_ErrorOccurred(object sender, Microsoft.Advertising.WinRT.UI.AdErrorEventArgs e)
        {
            System.Diagnostics.Debug.Assert(true, e.Error.Message);
        }

        public void EnableAds()
        {
            _adControl = new AdControl() {
                ApplicationId = applicationId,
                AdUnitId = adUnitId,
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                Height = 90,
                Margin = new Thickness(0),
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top,
                Width = 728,
                IsEnabled = false
            };
            _adControl.ErrorOccurred += adControl_ErrorOccurred;

            this.Children.Add(_adControl);
        }

        public void DisableAds()
        {
            this.Children.Remove(_adControl);
        }
    }
}
