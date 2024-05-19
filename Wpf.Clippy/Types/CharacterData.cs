using System.Collections.Generic;

namespace Wpf.Clippy.Types
{
    public enum AnimationMode
    {
        Once,
        Loop
    }

    public class CharacterData
    {
        public class AnimationFrame
        {
            public int Duration { get; set; }
            public string Sound { get; set; }
            public int[][] Images { get; set; }
        }

        public class CharacterAnimation
        {
            public AnimationFrame[] Frames { get; set; }
        }

        public int OverlayCount { get; set; }
        public string[] Sounds { get; set; }
        public int[] FrameSize { get; set; }
        public Dictionary<string, CharacterAnimation> Animations { get; set; }
    }
}
