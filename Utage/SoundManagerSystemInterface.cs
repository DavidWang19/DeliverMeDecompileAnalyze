namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public interface SoundManagerSystemInterface
    {
        AudioSource GetAudioSource(string groupName, string label);
        float GetGroupVolume(string groupName);
        float GetMasterVolume(string groupName);
        float GetSamplesVolume(string groupName, string label);
        void Init(SoundManager soundManager, List<string> saveStreamNameList);
        bool IsMultiPlay(string groupName);
        bool IsPlaying(string groupName, string label);
        void Play(string groupName, string label, SoundData soundData, float fadeInTime, float fadeOutTime);
        void ReadSaveDataBuffer(BinaryReader reader);
        void SetGroupVolume(string groupName, float volume);
        void SetMasterVolume(string groupName, float volume);
        void SetMultiPlay(string groupName, bool multiPlay);
        void Stop(string groupName, string label, float fadeTime);
        void StopAll(float fadeTime);
        void StopGroup(string groupName, float fadeTime);
        void StopGroupIgnoreLoop(string groupName, float fadeTime);
        void WriteSaveData(BinaryWriter writer);

        bool IsLoading { get; }
    }
}

