namespace Utage
{
    using System;

    public static class FlagsUtil
    {
        public static T Add<T>(T value, T flags) where T: struct
        {
            T local;
            try
            {
                local = (T) (((int) value) | ((int) flags));
            }
            catch (Exception exception)
            {
                throw new ArgumentException(string.Format("Could not add flags type '{0}'.", typeof(T).Name), exception);
            }
            return local;
        }

        public static bool Has<T>(T value, T flags) where T: struct
        {
            try
            {
                return ((((int) value) & ((int) flags)) == ((int) flags));
            }
            catch
            {
                return false;
            }
        }

        public static bool HasAny<T>(T value, T flags) where T: struct
        {
            try
            {
                return ((((int) value) & ((int) flags)) != 0);
            }
            catch
            {
                return false;
            }
        }

        public static bool Is<T>(T value, T flags) where T: struct
        {
            try
            {
                return (((int) value) == ((int) flags));
            }
            catch
            {
                return false;
            }
        }

        public static T Remove<T>(T value, T flags) where T: struct
        {
            T local;
            try
            {
                local = (T) (((int) value) & ~((int) flags));
            }
            catch (Exception exception)
            {
                throw new ArgumentException(string.Format("Could not remove flags type '{0}'.", typeof(T).Name), exception);
            }
            return local;
        }

        public static T SetEnable<T>(T value, T flags, bool isEnable) where T: struct
        {
            T local;
            try
            {
                if (isEnable)
                {
                    return Add<T>(value, flags);
                }
                local = Remove<T>(value, flags);
            }
            catch (Exception exception)
            {
                throw new ArgumentException(string.Format("Could not SetEnable flags type '{0}'.", typeof(T).Name), exception);
            }
            return local;
        }
    }
}

