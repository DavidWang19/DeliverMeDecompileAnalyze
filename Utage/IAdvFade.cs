namespace Utage
{
    using System;

    public interface IAdvFade
    {
        void FadeIn(float time, Action onComplete);
        void FadeOut(float time, Action onComplete);
        void RuleFadeIn(AdvEngine engine, AdvTransitionArgs data, Action onComplete);
        void RuleFadeOut(AdvEngine engine, AdvTransitionArgs data, Action onComplete);
    }
}

