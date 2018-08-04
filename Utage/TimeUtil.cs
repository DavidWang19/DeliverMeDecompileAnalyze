namespace Utage
{
    using System;
    using UnityEngine;

    public static class TimeUtil
    {
        public static float GetDeltaTime(bool unscaled)
        {
            return (!unscaled ? Time.get_deltaTime() : Time.get_unscaledDeltaTime());
        }

        public static float GetTime(bool unscaled)
        {
            return (!unscaled ? Time.get_time() : Time.get_unscaledTime());
        }
    }
}

