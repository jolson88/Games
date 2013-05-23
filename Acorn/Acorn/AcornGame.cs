using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Hiromi;
using Acorn.States;

namespace Acorn
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AcornGame : HiromiGame
    {
        protected override void Initialize()
        {
            base.Initialize();

            var applicationId = "3c91001e-d88b-4a77-af6c-9b4fb82baf22";
            var unitId = "10072805";

#if WINDOWS_PHONE
            // pubCenter doesn't work in an emulator, so use test ads
            if (Microsoft.Devices.Environment.DeviceType == Microsoft.Devices.DeviceType.Emulator)
            {
                applicationId = "test_client";
                unitId = "Image480_80";
            }
#endif

#if WINDOWS_PHONE
            var centerX = this.GraphicsDevice.Viewport.Width / 2;
            var locationX = (centerX - (480 / 2));
            this.InitializeAds(applicationId, unitId, new Rectangle(locationX, 0, 480, 80));
            this.DisableAds();
#endif
        }

        protected override GameState GetInitialState()
        {
            return new MenuState();
        }

        protected override Vector2 GetDesignedScreenSize()
        {
            return new Vector2(1600, 900);
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        protected override Texture2D GetPauseImage()
        {
            return ContentService.Instance.GetAsset<Texture2D>(AcornAssets.PauseImage);
        }
    }
}
