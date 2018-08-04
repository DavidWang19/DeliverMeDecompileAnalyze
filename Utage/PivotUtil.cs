namespace Utage
{
    using System;
    using UnityEngine;

    public static class PivotUtil
    {
        public static Vector2 PivotEnumToVector2(Pivot pivot)
        {
            switch (pivot)
            {
                case Pivot.TopLeft:
                    return new Vector2(0f, 1f);

                case Pivot.Top:
                    return new Vector2(0.5f, 1f);

                case Pivot.TopRight:
                    return new Vector2(1f, 1f);

                case Pivot.Left:
                    return new Vector2(0f, 0.5f);

                case Pivot.Center:
                    return new Vector2(0.5f, 0.5f);

                case Pivot.Right:
                    return new Vector2(1f, 0.5f);

                case Pivot.BottomLeft:
                    return new Vector2(0f, 0f);

                case Pivot.Bottom:
                    return new Vector2(0.5f, 0f);

                case Pivot.BottomRight:
                    return new Vector2(1f, 0f);
            }
            return new Vector2(0.5f, 0.5f);
        }
    }
}

