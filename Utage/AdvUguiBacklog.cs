namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/UiBacklog"), RequireComponent(typeof(UnityEngine.UI.Button))]
    public class AdvUguiBacklog : MonoBehaviour
    {
        private UnityEngine.UI.Button button;
        public Text characterName;
        private AdvBacklog data;
        public bool isMultiTextInPage;
        public GameObject soundIcon;
        public UguiNovelText text;

        [DebuggerHidden]
        private IEnumerator CoPlayVoice(string voiceFileName, string characterLabel)
        {
            return new <CoPlayVoice>c__Iterator0 { voiceFileName = voiceFileName, characterLabel = characterLabel, $this = this };
        }

        public void Init(AdvBacklog data)
        {
            <Init>c__AnonStorey1 storey = new <Init>c__AnonStorey1 {
                data = data,
                $this = this
            };
            this.data = storey.data;
            if (this.isMultiTextInPage)
            {
                float num = this.text.get_rectTransform().get_rect().get_height();
                this.text.set_text(storey.data.Text);
                float num2 = this.text.get_preferredHeight();
                (this.text.get_transform() as RectTransform).SetSizeWithCurrentAnchors(1, num2);
                float num3 = (base.get_transform() as RectTransform).get_rect().get_height();
                float num4 = this.text.get_transform().get_lossyScale().y / base.get_transform().get_lossyScale().y;
                num3 += (num2 - num) * num4;
                (base.get_transform() as RectTransform).SetSizeWithCurrentAnchors(1, num3);
            }
            else
            {
                this.text.set_text(storey.data.Text);
            }
            this.characterName.set_text(storey.data.MainCharacterNameText);
            int countVoice = storey.data.CountVoice;
            if (countVoice <= 0)
            {
                this.soundIcon.SetActive(false);
                this.Button.set_interactable(false);
            }
            else if ((countVoice >= 2) || this.isMultiTextInPage)
            {
                this.text.get_gameObject().GetComponentCreateIfMissing<UguiNovelTextEventTrigger>().OnClick.AddListener(new UnityAction<UguiNovelTextHitArea>(storey, (IntPtr) this.<>m__0));
            }
            else
            {
                this.Button.get_onClick().AddListener(new UnityAction(storey, (IntPtr) this.<>m__1));
            }
        }

        private void OnClicked(string voiceFileName)
        {
            if (!string.IsNullOrEmpty(voiceFileName))
            {
                base.StartCoroutine(this.CoPlayVoice(voiceFileName, this.Data.FindCharacerLabel(voiceFileName)));
            }
        }

        private void OnClickHitArea(UguiNovelTextHitArea hitGroup, Action<string> OnClicked)
        {
            if (hitGroup.HitEventType == CharData.HitEventType.Sound)
            {
                OnClicked(hitGroup.Arg);
            }
        }

        public UnityEngine.UI.Button Button
        {
            get
            {
                if (this.button == null)
                {
                }
                return (this.button = base.GetComponent<UnityEngine.UI.Button>());
            }
        }

        public AdvBacklog Data
        {
            get
            {
                return this.data;
            }
        }

        [CompilerGenerated]
        private sealed class <CoPlayVoice>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvUguiBacklog $this;
            internal AssetFile <file>__0;
            internal SoundManager <manager>__0;
            internal string characterLabel;
            internal string voiceFileName;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<file>__0 = AssetFileManager.Load(this.voiceFileName, this.$this);
                        if (this.<file>__0 != null)
                        {
                            break;
                        }
                        Debug.LogError("Backlog voiceFile is NULL");
                        goto Label_00CC;

                    case 1:
                        break;

                    default:
                        goto Label_00CC;
                }
                while (!this.<file>__0.IsLoadEnd)
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.<manager>__0 = SoundManager.GetInstance();
                if (this.<manager>__0 != null)
                {
                    this.<manager>__0.PlayVoice(this.characterLabel, this.<file>__0);
                }
                this.<file>__0.Unuse(this.$this);
                this.$PC = -1;
            Label_00CC:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <Init>c__AnonStorey1
        {
            internal AdvUguiBacklog $this;
            internal AdvBacklog data;

            internal void <>m__0(UguiNovelTextHitArea x)
            {
                this.$this.OnClickHitArea(x, new Action<string>(this.$this.OnClicked));
            }

            internal void <>m__1()
            {
                this.$this.OnClicked(this.data.MainVoiceFileName);
            }
        }
    }
}

