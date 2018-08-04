namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/Internal/AnimationPlayer")]
    public class AdvAnimationPlayer : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <AutoDestory>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AnimationClip <Clip>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <EnableSave>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Speed>k__BackingField;
        private Animator animator;
        private Animation lecayAnimation;
        public const WrapMode NoneOverrideWrapMode = -1;
        private Action onComplete;
        private const int Version = 0;

        internal void Cancel()
        {
            if (this.lecayAnimation != null)
            {
                this.lecayAnimation.Stop();
            }
            this.OnComplete();
        }

        private float GetTime()
        {
            if (this.lecayAnimation != null)
            {
                return this.lecayAnimation.get_Item(this.Clip.get_name()).get_time();
            }
            if (this.animator != null)
            {
                Debug.Log("Not Support");
                return 0f;
            }
            return 0f;
        }

        private void OnComplete()
        {
            if (this.onComplete != null)
            {
                this.onComplete();
            }
            if (this.AutoDestory)
            {
                Object.Destroy(this);
            }
        }

        private void OnDestroy()
        {
            if (this.lecayAnimation != null)
            {
                Object.Destroy(this.lecayAnimation);
            }
            if (this.animator != null)
            {
                Object.Destroy(this.animator);
            }
        }

        internal void Play(AnimationClip clip, float speed, Action onComplete = null)
        {
            this.Clip = clip;
            this.Speed = speed;
            this.onComplete = onComplete;
            if (clip.get_legacy())
            {
                this.PlayAnimatinLegacy(clip, speed);
            }
            else
            {
                Debug.LogError("Not Support");
            }
        }

        private void PlayAnimatinLegacy(AnimationClip clip, float speed)
        {
            if (this.lecayAnimation == null)
            {
                this.lecayAnimation = base.get_gameObject().GetComponentCreateIfMissing<Animation>();
            }
            this.lecayAnimation.AddClip(clip, clip.get_name());
            this.lecayAnimation.get_Item(clip.get_name()).set_speed(speed);
            this.lecayAnimation.Play(clip.get_name());
        }

        public void Read(BinaryReader reader, AdvEngine engine)
        {
            int num = reader.ReadInt32();
            if ((num < 0) || (num > 0))
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
            else
            {
                string name = reader.ReadString();
                float speed = reader.ReadSingle();
                float time = reader.ReadSingle();
                AdvAnimationData data = engine.DataManager.SettingDataManager.AnimationSetting.Find(name);
                if (data == null)
                {
                    Debug.LogError(name + " is not found in Animation sheet");
                    Object.Destroy(this);
                }
                else
                {
                    this.EnableSave = true;
                    this.AutoDestory = true;
                    this.Play(data.Clip, speed, null);
                    this.SetTime(time);
                }
            }
        }

        internal static void ReadSaveData(BinaryReader reader, GameObject go, AdvEngine engine)
        {
            int num = reader.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                go.AddComponent<AdvAnimationPlayer>().Read(reader, engine);
            }
        }

        private void SetTime(float time)
        {
            if (this.lecayAnimation != null)
            {
                this.lecayAnimation.get_Item(this.Clip.get_name()).set_time(time);
            }
            else if (this.animator != null)
            {
                Debug.Log("Not Support");
            }
        }

        private void Update()
        {
            if (this.lecayAnimation != null)
            {
                if (!this.lecayAnimation.get_isPlaying())
                {
                    this.OnComplete();
                }
            }
            else if (this.animator != null)
            {
                Debug.LogError("Not Support");
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.Clip.get_name());
            writer.Write(this.Speed);
            writer.Write(this.GetTime());
        }

        internal static void WriteSaveData(BinaryWriter writer, GameObject go)
        {
            AdvAnimationPlayer[] components = go.GetComponents<AdvAnimationPlayer>();
            int num = 0;
            foreach (AdvAnimationPlayer player in components)
            {
                if (player.EnableSave)
                {
                    num++;
                }
            }
            writer.Write(num);
            foreach (AdvAnimationPlayer player2 in components)
            {
                if (player2.EnableSave)
                {
                    player2.Write(writer);
                }
            }
        }

        public bool AutoDestory { get; set; }

        private AnimationClip Clip { get; set; }

        public bool EnableSave { get; set; }

        private float Speed { get; set; }
    }
}

