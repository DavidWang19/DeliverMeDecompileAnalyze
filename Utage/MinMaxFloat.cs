namespace Utage
{
    using System;
    using UnityEngine;

    [Serializable]
    public class MinMaxFloat : MinMax<float>
    {
        public float Clamp(float value)
        {
            return Mathf.Clamp(value, base.Min, base.Max);
        }

        public float RandomRange()
        {
            return Random.Range(base.Min, base.Max);
        }
    }
}

