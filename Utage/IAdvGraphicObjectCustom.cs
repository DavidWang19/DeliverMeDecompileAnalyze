namespace Utage
{
    using System;

    public interface IAdvGraphicObjectCustom
    {
        void ChangeResourceOnDrawSub(AdvGraphicInfo graphic);
        void OnEffectColorsChange(AdvEffectColor color);
    }
}

