namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;
    using UtageExtensions;

    [AddComponentMenu("Utage/Lib/System UI/DebugMenu")]
    public class SystemUiDebugMenu : MonoBehaviour
    {
        [SerializeField]
        private bool autoUpdateLogText = true;
        [SerializeField]
        private GameObject buttonRoot;
        [SerializeField]
        private UguiLocalize buttonText;
        [SerializeField]
        private GameObject buttonViewRoot;
        private Mode currentMode;
        [SerializeField]
        private GameObject debugInfo;
        [SerializeField]
        private Text debugInfoText;
        [SerializeField]
        private GameObject debugLog;
        [SerializeField]
        private Text debugLogText;
        [SerializeField]
        private bool enabeReleaseBuild;
        [SerializeField]
        private GameObject rootDebugMenu;
        [SerializeField]
        private GameObject targetDeleteAllSaveData;

        private void ChangeMode(Mode mode)
        {
            if (this.currentMode != mode)
            {
                if (mode >= Mode.Max)
                {
                    mode = Mode.Hide;
                }
                this.currentMode = mode;
                this.ClearAll();
                base.StopAllCoroutines();
                switch (this.currentMode)
                {
                    case Mode.Info:
                        base.StartCoroutine(this.CoUpdateInfo());
                        break;

                    case Mode.Log:
                        base.StartCoroutine(this.CoUpdateLog());
                        break;

                    case Mode.Memu:
                        base.StartCoroutine(this.CoUpdateMenu());
                        break;
                }
            }
        }

        private void ClearAll()
        {
            this.buttonViewRoot.SetActive(false);
            this.debugInfo.SetActive(false);
            this.debugLog.SetActive(false);
            this.rootDebugMenu.SetActive(false);
        }

        [DebuggerHidden]
        private IEnumerator CoUpdateInfo()
        {
            return new <CoUpdateInfo>c__Iterator0 { $this = this };
        }

        [DebuggerHidden]
        private IEnumerator CoUpdateLog()
        {
            return new <CoUpdateLog>c__Iterator1 { $this = this };
        }

        [DebuggerHidden]
        private IEnumerator CoUpdateMenu()
        {
            return new <CoUpdateMenu>c__Iterator2 { $this = this };
        }

        public void OnClickChangeLanguage()
        {
            LanguageManagerBase instance = LanguageManagerBase.Instance;
            if ((instance != null) && (instance.Languages.Count >= 1))
            {
                int index = instance.Languages.IndexOf(instance.CurrentLanguage);
                instance.CurrentLanguage = instance.Languages[(index + 1) % instance.Languages.Count];
            }
        }

        public void OnClickDeleteAllCacheFiles()
        {
            AssetFileManager.GetInstance().AssetBundleInfoManager.DeleteAllCache();
        }

        public void OnClickDeleteAllSaveDataAndQuit()
        {
            this.targetDeleteAllSaveData.SafeSendMessage("OnDeleteAllSaveDataAndQuit", null, false);
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }

        public void OnClickSwitchButton()
        {
            if (!this.Ignore)
            {
                this.ChangeMode(this.currentMode + 1);
            }
        }

        private void Start()
        {
            if (this.Ignore)
            {
                this.buttonRoot.SetActive(false);
            }
            this.ClearAll();
            this.ChangeMode(this.currentMode);
        }

        private bool Ignore
        {
            get
            {
                return (!this.enabeReleaseBuild && !Debug.get_isDebugBuild());
            }
        }

        [CompilerGenerated]
        private sealed class <CoUpdateInfo>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal SystemUiDebugMenu $this;

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
                        this.$this.buttonViewRoot.SetActive(true);
                        this.$this.buttonText.Key = SystemText.DebugInfo.ToString();
                        this.$this.debugInfo.SetActive(true);
                        break;

                    case 1:
                        break;
                        this.$PC = -1;
                        goto Label_009F;

                    default:
                        goto Label_009F;
                }
                this.$this.debugInfoText.set_text(DebugPrint.GetDebugString());
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            Label_009F:
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
        private sealed class <CoUpdateLog>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal SystemUiDebugMenu $this;

            [DebuggerHidden]
            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    this.$this.buttonViewRoot.SetActive(true);
                    this.$this.buttonText.Key = SystemText.DebugLog.ToString();
                    this.$this.debugLog.SetActive(true);
                    if (this.$this.autoUpdateLogText)
                    {
                        this.$this.debugLogText.set_text(this.$this.debugLogText.get_text() + DebugPrint.GetLogString());
                    }
                }
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
        private sealed class <CoUpdateMenu>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal SystemUiDebugMenu $this;

            [DebuggerHidden]
            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    this.$this.buttonViewRoot.SetActive(true);
                    this.$this.buttonText.Key = SystemText.DebugMenu.ToString();
                    this.$this.rootDebugMenu.SetActive(true);
                }
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

        private enum Mode
        {
            Hide,
            Info,
            Log,
            Memu,
            Max
        }
    }
}

