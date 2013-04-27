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
    public class MenuHumanView : HumanGameView
    {
        private GeneralInputSystem _inputSystem;
        private GameObject _playButton;

        public MenuHumanView(MessageManager messageManager, GameObjectManager gameObjectManager)
            : base(messageManager, gameObjectManager)
        {
            _inputSystem = new GeneralInputSystem(messageManager);

            messageManager.AddListener<NewGameObjectMessage>(msg => OnNewGameObject((NewGameObjectMessage)msg));
            messageManager.AddListener<ButtonPressMessage>(msg => OnButtonPress((ButtonPressMessage)msg));
        }

        private void OnNewGameObject(NewGameObjectMessage msg)
        {
            if (msg.GameObject.Tag.Equals("PlayButton"))
            {
                _playButton = msg.GameObject;
            }
        }

        private void OnButtonPress(ButtonPressMessage msg)
        {
            if (msg.GameObjectId == _playButton.Id)
            {
                this.MessageManager.QueueMessage(new RequestChangeStateMessage(new PlayState()));
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            _inputSystem.Update(gameTime);
        }
    }
}
