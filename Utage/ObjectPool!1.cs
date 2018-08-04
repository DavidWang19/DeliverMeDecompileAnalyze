namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;

    internal class ObjectPool<T> where T: new()
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <countAll>k__BackingField;
        private readonly UnityAction<T> m_ActionOnGet;
        private readonly UnityAction<T> m_ActionOnRelease;
        private readonly Stack<T> m_Stack;

        public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
        {
            this.m_Stack = new Stack<T>();
            this.m_ActionOnGet = actionOnGet;
            this.m_ActionOnRelease = actionOnRelease;
        }

        public T Get()
        {
            T local;
            if (this.m_Stack.Count == 0)
            {
                local = Activator.CreateInstance<T>();
                this.countAll++;
            }
            else
            {
                local = this.m_Stack.Pop();
            }
            if (this.m_ActionOnGet != null)
            {
                this.m_ActionOnGet.Invoke(local);
            }
            return local;
        }

        public void Release(T element)
        {
            if ((this.m_Stack.Count > 0) && object.ReferenceEquals(this.m_Stack.Peek(), element))
            {
                Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
            }
            if (this.m_ActionOnRelease != null)
            {
                this.m_ActionOnRelease.Invoke(element);
            }
            this.m_Stack.Push(element);
        }

        public int countActive
        {
            get
            {
                return (this.countAll - this.countInactive);
            }
        }

        public int countAll { get; private set; }

        public int countInactive
        {
            get
            {
                return this.m_Stack.Count;
            }
        }
    }
}

