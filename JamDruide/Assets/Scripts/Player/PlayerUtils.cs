using UnityEngine;

namespace Player
{
    public struct FrameInput {
        public float x;
        public bool jumpDown;
        public bool jumpUp;
    }

    public struct RayRange
    {
        public RayRange(float x1, float y1, float x2, float y2, Vector2 dir)
        {
            start = new Vector2(x1, y1);
            end = new Vector2(x2, y2);
            this.dir = dir;
        }

        public readonly Vector2 start, end, dir;
    }
}