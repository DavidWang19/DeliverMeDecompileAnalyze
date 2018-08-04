using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Utage;

[AddComponentMenu("Utage/TemplateUI/Title")]
public class UtageUguiBoot : UguiView
{
    [SerializeField]
    private AdvEngine engine;
    public UguiFadeTextureStream fadeTextureStream;
    public bool isWaitBoot;
    public bool isWaitDownLoad;
    public bool isWaitSplashScreen = true;
    public UtageUguiLoadWait loadWait;
    public UtageUguiTitle title;

    [DebuggerHidden]
    private IEnumerator CoUpdate()
    {
        return new <CoUpdate>c__Iterator0 { $this = this };
    }

    public void Start()
    {
        this.title.get_gameObject().SetActive(false);
        base.StartCoroutine(this.CoUpdate());
    }

    public AdvEngine Engine
    {
        get
        {
            if (this.engine == null)
            {
            }
            return (this.engine = Object.FindObjectOfType<AdvEngine>());
        }
    }

    [CompilerGenerated]
    private sealed class <CoUpdate>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal bool $disposing;
        internal int $PC;
        internal UtageUguiBoot $this;

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
                    if (!this.$this.isWaitSplashScreen)
                    {
                        goto Label_0063;
                    }
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_00C9;

                case 3:
                    goto Label_010E;

                default:
                    goto Label_0180;
            }
            if (!WrapperUnityVersion.IsFinishedSplashScreen())
            {
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                goto Label_0182;
            }
        Label_0063:
            this.$this.Open();
            if (this.$this.fadeTextureStream == null)
            {
                goto Label_00DE;
            }
            this.$this.fadeTextureStream.get_gameObject().SetActive(true);
            this.$this.fadeTextureStream.Play();
        Label_00C9:
            while (this.$this.fadeTextureStream.IsPlaying)
            {
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_0182;
            }
        Label_00DE:
            if (!this.$this.isWaitBoot)
            {
                goto Label_0123;
            }
        Label_010E:
            while (this.$this.Engine.IsWaitBootLoading)
            {
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 3;
                }
                goto Label_0182;
            }
        Label_0123:
            this.$this.Close();
            if (this.$this.isWaitDownLoad && (this.$this.loadWait != null))
            {
                this.$this.loadWait.OpenOnBoot();
            }
            else
            {
                this.$this.title.Open();
            }
            this.$PC = -1;
        Label_0180:
            return false;
        Label_0182:
            return true;
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
}

