using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        }

        protected override GameState GetInitialState()
        {
            return new MenuState();
        }

        protected override Vector2 GetDesignedScreenSize()
        {
            return new Vector2(1600, 900);
        }
    }
}
