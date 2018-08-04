namespace Utage
{
    using System;

    public class ExpressionCast
    {
        public static float ToFloat(object value)
        {
            if (value.GetType() == typeof(float))
            {
                return (float) value;
            }
            if (value.GetType() != typeof(int))
            {
                throw new Exception("Cant cast :" + value.GetType() + " ToFloat");
            }
            return (float) ((int) value);
        }

        public static int ToInt(object value)
        {
            if (value.GetType() == typeof(int))
            {
                return (int) value;
            }
            if (value.GetType() != typeof(float))
            {
                throw new Exception("Cant cast :" + value.GetType() + " ToInt");
            }
            return (int) ((float) value);
        }
    }
}

