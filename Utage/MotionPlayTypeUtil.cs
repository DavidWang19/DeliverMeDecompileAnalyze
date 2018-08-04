namespace Utage
{
    using System;

    public static class MotionPlayTypeUtil
    {
        public static bool CheckReplayMotion(string currentMotion, string nextMotion, MotionPlayType playType)
        {
            if ((currentMotion == nextMotion) && ((playType == MotionPlayType.Loop) || (playType == MotionPlayType.NoReplay)))
            {
                return false;
            }
            return true;
        }
    }
}

