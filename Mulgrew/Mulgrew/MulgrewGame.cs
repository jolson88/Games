using Hiromi;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mulgrew.Screens;

namespace Mulgrew
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MulgrewGame : HiromiGame
    {
        protected override Screen GetInitialScreen()
        {
            return new PlayScreen();
        }
    }
}
