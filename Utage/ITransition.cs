namespace Utage
{
    using System;

    public interface ITransition
    {
        void CancelClosing();
        void Close();
        void Open();

        bool IsPlaying { get; }
    }
}

