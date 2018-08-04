namespace Utage
{
    using System;
    using UnityEngine;

    [Serializable]
    public class MinMaxInt : MinMax<int>
    {
        public int Clamp(int value)
        {
            return Mathf.Clamp(value, base.Min, base.Max);
        }

        public float RandomRange()
        {
            return (float) Random.Range(base.Min, base.Max + 1);
        }
    }
}

