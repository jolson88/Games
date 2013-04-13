using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Acorn.Hiromi;

namespace Acorn
{
    public static class Resources
    {
        public static string Background = "Sprites\\Background";
        public static string CloudSprite = "Sprites\\Cloud";
        public static string StopButtonSprite = "Sprites\\StopButton";
        public static string StopButtonPressedSprite = "Sprites\\StopButtonPressed";
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
                return new Sprite(ContentService.Instance.Content.Load<Texture2D>(Resources.CloudSprite));
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
