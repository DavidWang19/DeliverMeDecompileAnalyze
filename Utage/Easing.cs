namespace Utage
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class Easing
    {
        public static float EaseInBack(float value)
        {
            value /= 1f;
            float num = 1.70158f;
            return ((value * value) * (((num + 1f) * value) - num));
        }

        public static float EaseInCirc(float value)
        {
            return -(Mathf.Sqrt(1f - (value * value)) - 1f);
        }

        public static float EaseInCubic(float value)
        {
            return ((value * value) * value);
        }

        public static float EaseInExpo(float value)
        {
            return Mathf.Pow(2f, 10f * (value - 1f));
        }

        public static float EaseInOutBack(float value)
        {
            float num = 1.70158f;
            value /= 0.5f;
            if (value < 1f)
            {
                num *= 1.525f;
                return (0.5f * ((value * value) * (((num + 1f) * value) - num)));
            }
            value -= 2f;
            num *= 1.525f;
            return (0.5f * (((value * value) * (((num + 1f) * value) + num)) + 2f));
        }

        public static float EaseInOutCirc(float value)
        {
            value /= 0.5f;
            if (value < 1f)
            {
                return (-0.5f * (Mathf.Sqrt(1f - (value * value)) - 1f));
            }
            value -= 2f;
            return (0.5f * (Mathf.Sqrt(1f - (value * value)) + 1f));
        }

        public static float EaseInOutCubic(float value)
        {
            value /= 0.5f;
            if (value < 1f)
            {
                return (((0.5f * value) * value) * value);
            }
            value -= 2f;
            return (0.5f * (((value * value) * value) + 2f));
        }

        public static float EaseInOutExpo(float value)
        {
            value /= 0.5f;
            if (value < 1f)
            {
                return (0.5f * Mathf.Pow(2f, 10f * (value - 1f)));
            }
            value--;
            return (0.5f * (-Mathf.Pow(2f, -10f * value) + 2f));
        }

        public static float EaseInOutQuad(float value)
        {
            value /= 0.5f;
            if (value < 1f)
            {
                return ((0.5f * value) * value);
            }
            value--;
            return (-0.5f * ((value * (value - 2f)) - 1f));
        }

        public static float EaseInOutQuart(float value)
        {
            value /= 0.5f;
            if (value < 1f)
            {
                return ((((0.5f * value) * value) * value) * value);
            }
            value -= 2f;
            return (-0.5f * ((((value * value) * value) * value) - 2f));
        }

        public static float EaseInOutQuint(float value)
        {
            value /= 0.5f;
            if (value < 1f)
            {
                return (((((0.5f * value) * value) * value) * value) * value);
            }
            value -= 2f;
            return (0.5f * (((((value * value) * value) * value) * value) + 2f));
        }

        public static float EaseInOutSine(float value)
        {
            return (-0.5f * (Mathf.Cos(3.141593f * value) - 1f));
        }

        public static float EaseInQuad(float value)
        {
            return (value * value);
        }

        public static float EaseInQuart(float value)
        {
            return (((value * value) * value) * value);
        }

        public static float EaseInQuint(float value)
        {
            return ((((value * value) * value) * value) * value);
        }

        public static float EaseInSine(float value)
        {
            return (1f - Mathf.Cos(value * 1.570796f));
        }

        public static float EaseOutBack(float value)
        {
            float num = 1.70158f;
            value--;
            return (((value * value) * (((num + 1f) * value) + num)) + 1f);
        }

        public static float EaseOutCirc(float value)
        {
            value--;
            return Mathf.Sqrt(1f - (value * value));
        }

        public static float EaseOutCubic(float value)
        {
            value--;
            return (((value * value) * value) + 1f);
        }

        public static float EaseOutExpo(float value)
        {
            return (-Mathf.Pow(2f, -10f * value) + 1f);
        }

        public static float EaseOutQuad(float value)
        {
            return (-value * (value - 2f));
        }

        public static float EaseOutQuart(float value)
        {
            value--;
            return -((((value * value) * value) * value) - 1f);
        }

        public static float EaseOutQuint(float value)
        {
            value--;
            return (((((value * value) * value) * value) * value) + 1f);
        }

        public static float EaseOutSine(float value)
        {
            return Mathf.Sin(value * 1.570796f);
        }

        public static float GetCurve(float value, EaseType type)
        {
            switch (type)
            {
                case EaseType.Linear:
                    return value;

                case EaseType.Spring:
                    return Spring(value);

                case EaseType.EaseInQuad:
                    return EaseInQuad(value);

                case EaseType.EaseOutQuad:
                    return EaseOutQuad(value);

                case EaseType.EaseInOutQuad:
                    return EaseInOutQuad(value);

                case EaseType.EaseInCubic:
                    return EaseInCubic(value);

                case EaseType.EaseOutCubic:
                    return EaseOutCubic(value);

                case EaseType.EaseInOutCubic:
                    return EaseInOutCubic(value);

                case EaseType.EaseInQuart:
                    return EaseInQuart(value);

                case EaseType.EaseOutQuart:
                    return EaseOutQuart(value);

                case EaseType.EaseInOutQuart:
                    return EaseInOutQuart(value);

                case EaseType.EaseInQuint:
                    return EaseInQuint(value);

                case EaseType.EaseOutQuint:
                    return EaseOutQuint(value);

                case EaseType.EaseInOutQuint:
                    return EaseInOutQuint(value);

                case EaseType.EaseInSine:
                    return EaseInSine(value);

                case EaseType.EaseOutSine:
                    return EaseOutSine(value);

                case EaseType.EaseInOutSine:
                    return EaseInOutSine(value);

                case EaseType.EaseInExpo:
                    return EaseInExpo(value);

                case EaseType.EaseOutExpo:
                    return EaseOutExpo(value);

                case EaseType.EaseInOutExpo:
                    return EaseInOutExpo(value);

                case EaseType.EaseInCirc:
                    return EaseInCirc(value);

                case EaseType.EaseOutCirc:
                    return EaseOutCirc(value);

                case EaseType.EaseInOutCirc:
                    return EaseInOutCirc(value);

                case EaseType.EaseInBack:
                    return EaseInBack(value);

                case EaseType.EaseOutBack:
                    return EaseOutBack(value);

                case EaseType.EaseInOutBack:
                    return EaseInOutBack(value);
            }
            Debug.LogError("UnkonownType");
            return value;
        }

        public static float GetCurve(float start, float end, float value, EaseType type = 0)
        {
            return Linear(start, end, GetCurve(value, type));
        }

        public static Vector2 GetCurve(Vector2 start, Vector2 end, float value, EaseType type = 0)
        {
            float curve = GetCurve(value, type);
            return new Vector2(Linear(start.x, end.x, curve), Linear(start.y, end.y, curve));
        }

        public static Vector3 GetCurve(Vector3 start, Vector3 end, float value, EaseType type = 0)
        {
            float curve = GetCurve(value, type);
            return new Vector3(Linear(start.x, end.x, curve), Linear(start.y, end.y, curve), Linear(start.z, end.z, curve));
        }

        public static Vector4 GetCurve(Vector4 start, Vector4 end, float value, EaseType type = 0)
        {
            float curve = GetCurve(value, type);
            return new Vector4(Linear(start.x, end.x, curve), Linear(start.y, end.y, curve), Linear(start.z, end.z, curve), Linear(start.w, end.w, curve));
        }

        public static float GetCurve01(float value, EaseType type = 0)
        {
            return GetCurve((float) 0f, (float) 1f, value, type);
        }

        public static float Linear(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, value);
        }

        public static float Spring(float value)
        {
            value = Mathf.Clamp01(value);
            return (((Mathf.Sin((value * 3.141593f) * (0.2f + (((2.5f * value) * value) * value))) * Mathf.Pow(1f - value, 2.2f)) + value) * (1f + (1.2f * (1f - value))));
        }
    }
}

