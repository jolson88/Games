using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acorn
{
    public static class PlatformConfiguration
    {
        public static class SoundLevels
        {
#if WINDOWS_PHONE
            public static float Background = 0.3f;
            public static float Applause = 0.56f;
            public static float AcornDing = 0.4f;
            public static float CardSelect = 0.4f;
            public static float ZeroCardBuzz = 0.24f;
            public static float AcornOnGround = 0.6f;
            public static float ButtonSelect = 0.6f;
#else
            public static float Background = 0.3f;
            public static float Applause = 0.35f;
            public static float AcornDing = 0.32f;
            public static float CardSelect = 0.2f;
            public static float ZeroCardBuzz = 0.06f;
            public static float AcornOnGround = 0.5f;
            public static float ButtonSelect = 0.6f;
#endif
        }
    }
}
