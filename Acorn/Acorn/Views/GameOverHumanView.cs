using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Hiromi;
using Acorn.States;

namespace Acorn.Views
{
    public class GameOverHumanView : HumanGameView
    {
        private GeneralInputSystem _inputSystem;
        private GameObject _playButton;
        private GameObject _menuButton;

        protected override void OnInitialize()
        {
            _inputSystem = new GeneralInputSystem(this.MessageManager, this.SceneGraph);

            this.MessageManager.AddListener<NewGameObjectMessage>(OnNewGameObject);
            this.MessageManager.AddListener<ButtonPressMessage>(OnButtonPress);
        }

        private void OnNewGameObject(NewGameObjectMessage msg)
        {
            if (msg.GameObject.Tag.Equals("PlayButton"))
            {
                _playButton = msg.GameObject;
            }
            else if (msg.GameObject.Tag.Equals("MenuButton"))
            {
                _menuButton = msg.GameObject;
            }
        }

        private void OnButtonPress(ButtonPressMessage msg)
        {
            if (msg.GameObjectId == _playButton.Id)
            {
                this.MessageManager.QueueMessage(new RequestChangeStateMessage(new PlayState()));
            }
            else if (msg.GameObjectId == _menuButton.Id)
            {
                this.MessageManager.QueueMessage(new RequestChangeStateMessage(new MenuState()));
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            _inputSystem.Update(gameTime);
        }
    }
}
