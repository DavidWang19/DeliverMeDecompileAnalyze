namespace Utage
{
    using System;
    using UnityEngine;

    public static class InputUtil
    {
        private static float wheelSensitive = 0.1f;

        public static bool IsInputControl()
        {
            return (UtageToolKit.IsPlatformStandAloneOrEditor() && (Input.GetKey(0x132) || Input.GetKey(0x131)));
        }

        public static bool IsInputKeyboadReturnDown()
        {
            return (UtageToolKit.IsPlatformStandAloneOrEditor() && Input.GetKeyDown(13));
        }

        public static bool IsInputScrollWheelDown()
        {
            return (Input.GetAxis("Mouse ScrollWheel") <= -wheelSensitive);
        }

        public static bool IsInputScrollWheelUp()
        {
            return (Input.GetAxis("Mouse ScrollWheel") >= wheelSensitive);
        }

        [Obsolete("Use IsMouseRightButtonDown instead")]
        public static bool IsMousceRightButtonDown()
        {
            return IsMouseRightButtonDown();
        }

        public static bool IsMouseRightButtonDown()
        {
            return (UtageToolKit.IsPlatformStandAloneOrEditor() && Input.GetMouseButtonDown(1));
        }
    }
}

