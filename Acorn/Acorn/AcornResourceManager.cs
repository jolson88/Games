using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;

namespace Acorn
{
    public static class Resources
    {
        public static string Background = "Sprites\\Background";
        public static string CloudSprite = "Sprites\\Cloud";
        public static string StopButtonSprite = "Sprites\\StopButton";
        public static string StopButtonPressedSprite = "Sprites\\StopButtonPressed";
        public static string CardBack = "Sprites\\CardBack";
        public static string CardZero = "Sprites\\Card0";
        public static string CardOne = "Sprites\\Card1";
        public static string CardTwo = "Sprites\\Card2";
    }

    public static class AcornResourceManager
    {
        private static Dictionary<string, object> _cachedResources = new Dictionary<string,object>();

        public static Background GetBackground()
        {
            return RetrieveResource<Background>(Resources.Background, () =>
            {
                return new Background(ContentService.Instance.Content.Load<Texture2D>("Sprites\\Background"));
            });
        }

        public static Sprite GetCloudSprite()
        {
            return RetrieveResource<Sprite>(Resources.CloudSprite, () =>
            {
                var cloud = new Sprite(ContentService.Instance.Content.Load<Texture2D>(Resources.CloudSprite));
                cloud.Center = new Vector2(cloud.Texture.Width / 2, cloud.Texture.Height / 2);
                return cloud;
            });
        }

        public static Sprite GetStopButtonSprite()
        {
            return RetrieveResource<Sprite>(Resources.StopButtonSprite, () =>
            {
                var stopButton = new Sprite(ContentService.Instance.Content.Load<Texture2D>(Resources.StopButtonSprite));
                stopButton.Center = new Vector2(stopButton.Texture.Width / 2, stopButton.Texture.Height / 2);
                return stopButton;
            });
        }

        public static Sprite GetStopButtonPressedSprite()
        {
            return RetrieveResource<Sprite>(Resources.StopButtonPressedSprite, () =>
            {
                var stopButton = new Sprite(ContentService.Instance.Content.Load<Texture2D>(Resources.StopButtonPressedSprite));
                stopButton.Center = new Vector2(stopButton.Texture.Width / 2, stopButton.Texture.Height / 2);
                return stopButton;
            });
        }

        public static Sprite GetCardBackSprite()
        {
            return RetrieveResource<Sprite>(Resources.CardBack, () =>
            {
                return new Sprite(ContentService.Instance.Content.Load<Texture2D>(Resources.CardBack));
            });
        }

        public static Sprite GetCardZeroSprite()
        {
            return RetrieveResource<Sprite>(Resources.CardZero, () =>
            {
                return new Sprite(ContentService.Instance.Content.Load<Texture2D>(Resources.CardZero));
            });
        }

        public static Sprite GetCardOneSprite()
        {
            return RetrieveResource<Sprite>(Resources.CardOne, () =>
            {
                return new Sprite(ContentService.Instance.Content.Load<Texture2D>(Resources.CardOne));
            });
        }

        public static Sprite GetCardTwoSprite()
        {
            return RetrieveResource<Sprite>(Resources.CardTwo, () =>
            {
                return new Sprite(ContentService.Instance.Content.Load<Texture2D>(Resources.CardTwo));
            });
        }

        private static T RetrieveResource<T>(string resourceId, Func<T> creator)
        {
            if (!_cachedResources.ContainsKey(resourceId))
            {
                _cachedResources.Add(resourceId, creator());
            }
            return (T)_cachedResources[resourceId];
        }
    }
}
