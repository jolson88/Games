using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jarrett.Core;
using Jarrett.Views;

namespace Jarrett
{
    class JarrettLevelLoader : ILevelLoader
    {
        IGame m_game;
        string m_levelName;
        int m_humanPlayers;
        int m_totalPlayers;

        public JarrettLevelLoader(IGame game, string levelName, int humanPlayers, int totalPlayers)
        {
            m_game = game;
            m_levelName = levelName;
            m_humanPlayers = humanPlayers;
            m_totalPlayers = totalPlayers;
        }

        public List<GameActor> LoadActors()
        {
            var actors = new List<GameActor>();

            // TODO: Add actor that has BoardComponent
            return actors;
        }

        public List<IGameView> LoadViews()
        {
            var views = new List<IGameView>();

            var humanView = new HumanTicTacToeView();
            humanView.Initialize(m_game);

            views.Add(humanView);

            // TODO: Build views
            return views;
        }
    }
}
