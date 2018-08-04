namespace Utage
{
    using System;

    [Flags]
    public enum AssetBundleTargetFlags
    {
        Android = 1,
        iOS = 2,
        OSX = 0x10,
        WebGL = 4,
        Windows = 8
    }
}

