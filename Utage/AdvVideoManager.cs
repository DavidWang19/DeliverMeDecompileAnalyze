namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Video;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/VideoManager")]
    public class AdvVideoManager : MonoBehaviour
    {
        private AdvEngine engine;
        private Dictionary<string, VideoInfo> videos = new Dictionary<string, VideoInfo>();

        internal void Cancel(string label)
        {
            if (this.Videos[label].Cancel)
            {
                this.Videos[label].Canceled = true;
                this.Videos[label].Player.Stop();
            }
        }

        internal void Complete(string label)
        {
            VideoInfo info = this.Videos[label];
            info.Player.set_targetCamera(null);
            Object.Destroy(info.Player.get_gameObject());
            this.Videos.Remove(label);
        }

        internal bool IsEndPlay(string label)
        {
            if (!this.Videos.ContainsKey(label))
            {
                return true;
            }
            if (this.Videos[label].Canceled)
            {
                return true;
            }
            if (!this.Videos[label].Started)
            {
                return false;
            }
            return ((this.Videos[label].Player.get_time() > 0.0) && !this.Videos[label].Player.get_isPlaying());
        }

        private void OnStarted(VideoInfo info)
        {
            info.Started = true;
        }

        internal void Play(string label, string cameraName, VideoClip clip, bool loop, bool cancel)
        {
            <Play>c__AnonStorey0 storey = new <Play>c__AnonStorey0 {
                $this = this
            };
            VideoInfo info = new VideoInfo {
                Cancel = cancel
            };
            storey.info = info;
            this.Videos.Add(label, storey.info);
            VideoPlayer player = base.get_transform().AddChildGameObject(label).AddComponent<VideoPlayer>();
            player.set_isLooping(loop);
            player.set_clip(clip);
            player.set_targetCamera(this.Engine.EffectManager.FindTarget(AdvEffectManager.TargetType.Camera, cameraName).GetComponentInChildren<Camera>());
            player.set_renderMode(1);
            player.set_aspectRatio(3);
            player.Play();
            player.add_started(new VideoPlayer.EventHandler(storey, (IntPtr) this.<>m__0));
            storey.info.Player = player;
        }

        internal void Play(string label, string cameraName, AssetFile file, bool loop, bool cancel)
        {
            this.Play(label, cameraName, file.UnityObject as VideoClip, loop, cancel);
        }

        public AdvEngine Engine
        {
            get
            {
                if (this.engine == null)
                {
                }
                return (this.engine = base.GetComponentInParent<AdvEngine>());
            }
        }

        private Dictionary<string, VideoInfo> Videos
        {
            get
            {
                return this.videos;
            }
        }

        [CompilerGenerated]
        private sealed class <Play>c__AnonStorey0
        {
            internal AdvVideoManager $this;
            internal AdvVideoManager.VideoInfo info;

            internal void <>m__0(VideoPlayer x)
            {
                this.$this.OnStarted(this.info);
            }
        }

        private class VideoInfo
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <Cancel>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <Canceled>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private VideoPlayer <Player>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <Started>k__BackingField;

            public bool Cancel { get; set; }

            public bool Canceled { get; set; }

            public VideoPlayer Player { get; set; }

            public bool Started { get; set; }
        }
    }
}

