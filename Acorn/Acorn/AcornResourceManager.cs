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
        public static string RedSquirrel = "Sprites\\RedSquirrel";
        public static string BlueSquirrel = "Sprites\\BlueSquirrel";
        public static string ScoreMeter = "Sprites\\ScoreMeter";
    }

    public static class AcornResourceManager
    {
        private static Dictionary<string, object> _cachedResources = new Dictionary<string,object>();

        public static Background GetBackground()
        {
            return RetrieveResource<Background>(Resources.Background, () =>
            {
                return new Background(ContentService.Instance.Content.Load<Texture2D>(Resources.Background));
            });
        }

        public static Sprite GetCloudSprite()
        {
            return RetrieveResource<Sprite>(Resources.CloudSprite, () =>
            {
                return new Sprite(ContentService.Instance.Content.Load<Texture2D>(Resources.CloudSprite));
            });
        }

        public static Sprite GetStopButtonSprite()
        {
            return RetrieveResource<Sprite>(Resources.StopButtonSprite, () =>
            {
                return new Sprite(ContentService.Instance.Content.Load<Texture2D>(Resources.StopButtonSprite));
            });
        }

        public static Sprite GetStopButtonPressedSprite()
        {
            return RetrieveResource<Sprite>(Resources.StopButtonPressedSprite, () =>
            {
                return new Sprite(ContentService.Instance.Content.Load<Texture2D>(Resources.StopButtonPressedSprite));
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

        public static Sprite GetRedSquirrelSprite()
        {
            return RetrieveResource<Sprite>(Resources.RedSquirrel, () =>
            {
                return new Sprite(ContentService.Instance.Content.Load<Texture2D>(Resources.RedSquirrel));
            });
        }

        public static Sprite GetBlueSquirrelSprite()
        {
            return RetrieveResource<Sprite>(Resources.BlueSquirrel, () =>
            {
                return new Sprite(ContentService.Instance.Content.Load<Texture2D>(Resources.BlueSquirrel));
            });
        }

        public static Sprite GetScoreMeterSprite()
        {
            return RetrieveResource<Sprite>(Resources.ScoreMeter, () =>
            {
                return new Sprite(ContentService.Instance.Content.Load<Texture2D>(Resources.ScoreMeter));
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
