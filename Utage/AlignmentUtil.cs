namespace Utage
{
    using System;
    using UnityEngine;

    public static class AlignmentUtil
    {
        public static Vector2 GetAlignmentValue(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Center:
                    return new Vector2(0.5f, 0.5f);

                case Alignment.TopLeft:
                    return new Vector2(0f, 1f);

                case Alignment.TopCenter:
                    return new Vector2(0.5f, 1f);

                case Alignment.TopRight:
                    return new Vector2(1f, 1f);

                case Alignment.LeftCenter:
                    return new Vector2(0f, 0.5f);

                case Alignment.RightCenter:
                    return new Vector2(1f, 0.5f);

                case Alignment.BottomLeft:
                    return new Vector2(0f, 0f);

                case Alignment.BottomCenter:
                    return new Vector2(0.5f, 0f);

                case Alignment.BottomRight:
                    return new Vector2(1f, 0f);
            }
            return new Vector2(0.5f, 0.5f);
        }
    }
}

