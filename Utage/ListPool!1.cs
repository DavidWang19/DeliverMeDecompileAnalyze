namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine.Events;

    internal static class ListPool<T>
    {
        private static readonly ObjectPool<List<T>> s_ListPool;

        static ListPool()
        {
            ListPool<T>.s_ListPool = new ObjectPool<List<T>>(null, new UnityAction<List<T>>(null, (IntPtr) ListPool<T>.<s_ListPool>m__0));
        }

        [CompilerGenerated]
        private static void <s_ListPool>m__0(List<T> l)
        {
            l.Clear();
        }

        public static List<T> Get()
        {
            return ListPool<T>.s_ListPool.Get();
        }

        public static void Release(List<T> toRelease)
        {
            ListPool<T>.s_ListPool.Release(toRelease);
        }
    }
}

