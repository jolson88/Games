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
        protected override GameState GetInitialState()
        {
            return new MenuState();
        }
    }
}
