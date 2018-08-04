namespace Utage
{
    using System;
    using UnityEngine;

    [Serializable]
    public class LinearValue
    {
        private float time;
        private float timeCurrent;
        private float valueBegin;
        private float valueCurrent;
        private float valueEnd;

        public void Clear()
        {
            this.time = 0f;
            this.timeCurrent = 0f;
            this.valueBegin = 0f;
            this.valueEnd = 0f;
            this.valueCurrent = 0f;
        }

        public float GetValue()
        {
            return this.valueCurrent;
        }

        public void IncTime()
        {
            if (!this.IsEnd())
            {
                this.timeCurrent = Mathf.Min(this.timeCurrent + Time.get_deltaTime(), this.time);
                this.valueCurrent = Mathf.Lerp(this.valueBegin, this.valueEnd, this.timeCurrent / this.time);
            }
        }

        public void Init(float time, float valueBegin, float valueEnd)
        {
            this.time = time;
            this.timeCurrent = 0f;
            this.valueBegin = valueBegin;
            this.valueEnd = valueEnd;
            this.valueCurrent = valueBegin;
        }

        public bool IsEnd()
        {
            return (this.timeCurrent >= this.time);
        }
    }
}

