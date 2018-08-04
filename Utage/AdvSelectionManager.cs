namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;

    [AddComponentMenu("Utage/ADV/Internal/SelectionManager")]
    public class AdvSelectionManager : MonoBehaviour, IAdvSaveData, IBinaryIO
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsShowing>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsWaitInput>k__BackingField;
        private AdvEngine engine;
        [SerializeField]
        private SelectionEvent onBeginShow = new SelectionEvent();
        [SerializeField]
        private SelectionEvent onBeginWaitInput = new SelectionEvent();
        [SerializeField]
        private SelectionEvent onClear = new SelectionEvent();
        [SerializeField]
        private SelectionEvent onSelected = new SelectionEvent();
        [SerializeField]
        private SelectionEvent onUpdateWaitInput = new SelectionEvent();
        private AdvSelection selected;
        private List<AdvSelection> selections = new List<AdvSelection>();
        private List<AdvSelection> spriteSelections = new List<AdvSelection>();
        private const int VERSION = 1;
        private const int VERSION_0 = 0;

        public void AddSelection(string label, string text, ExpressionParser exp, string prefabName, float? x, float? y, StringGridRow row)
        {
            this.selections.Add(new AdvSelection(label, text, exp, prefabName, x, y, row));
        }

        public void AddSelectionClick(string label, string name, bool isPolygon, ExpressionParser exp, StringGridRow row)
        {
            AdvSelection item = new AdvSelection(label, name, isPolygon, exp, row);
            this.spriteSelections.Add(item);
            this.AddSpriteClickEvent(item);
        }

        private void AddSpriteClickEvent(AdvSelection select)
        {
            <AddSpriteClickEvent>c__AnonStorey0 storey = new <AddSpriteClickEvent>c__AnonStorey0 {
                select = select,
                $this = this
            };
            this.Engine.GraphicManager.AddClickEvent(storey.select.SpriteName, storey.select.IsPolygon, storey.select.RowData, new UnityAction<BaseEventData>(storey, (IntPtr) this.<>m__0));
        }

        public void Clear()
        {
            this.ClearSub();
            this.OnClear.Invoke(this);
        }

        private void ClearSub()
        {
            this.selected = null;
            this.selections.Clear();
            foreach (AdvSelection selection in this.spriteSelections)
            {
                this.Engine.GraphicManager.RemoveClickEvent(selection.SpriteName);
            }
            this.spriteSelections.Clear();
            this.IsShowing = false;
            this.IsWaitInput = false;
        }

        public void OnRead(BinaryReader reader)
        {
            this.ClearSub();
            int num = reader.ReadInt32();
            switch (num)
            {
                case 1:
                {
                    int num2 = reader.ReadInt32();
                    for (int i = 0; i < num2; i++)
                    {
                        AdvSelection item = new AdvSelection(reader, this.engine);
                        this.selections.Add(item);
                    }
                    num2 = reader.ReadInt32();
                    for (int j = 0; j < num2; j++)
                    {
                        AdvSelection selection2 = new AdvSelection(reader, this.engine);
                        this.spriteSelections.Add(selection2);
                        this.AddSpriteClickEvent(selection2);
                    }
                    break;
                }
                case 0:
                {
                    int num5 = reader.ReadInt32();
                    for (int k = 0; k < num5; k++)
                    {
                        AdvSelection selection3 = new AdvSelection(reader, this.engine);
                        this.selections.Add(selection3);
                    }
                    break;
                }
                default:
                {
                    object[] args = new object[] { num };
                    Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
                    break;
                }
            }
        }

        private void OnSpriteClick(BaseEventData eventData, AdvSelection select)
        {
            this.Select(select);
        }

        public void OnWrite(BinaryWriter writer)
        {
            writer.Write(1);
            writer.Write(this.Selections.Count);
            foreach (AdvSelection selection in this.Selections)
            {
                selection.Write(writer);
            }
            writer.Write(this.SpriteSelections.Count);
            foreach (AdvSelection selection2 in this.SpriteSelections)
            {
                selection2.Write(writer);
            }
        }

        public void Select(int index)
        {
            this.Select(this.selections[index]);
        }

        public void Select(AdvSelection selected)
        {
            this.selected = selected;
            string jumpLabel = selected.JumpLabel;
            if (selected.Exp != null)
            {
                this.Engine.Param.CalcExpression(selected.Exp);
            }
            this.OnSelected.Invoke(this);
            this.Engine.SystemSaveData.SelectionData.AddData(selected);
            this.Clear();
            this.Engine.ScenarioPlayer.MainThread.JumpManager.RegistoreLabel(jumpLabel);
        }

        internal void SelectWithTotalIndex(int index)
        {
            if (index < this.Selections.Count)
            {
                this.Select(index);
            }
            else
            {
                index -= this.Selections.Count;
                this.Select(this.SpriteSelections[index]);
            }
        }

        public void Show()
        {
            this.selected = null;
            this.IsShowing = true;
            this.OnBeginShow.Invoke(this);
        }

        internal bool TryStartWaitInputIfShowing()
        {
            if ((this.selections.Count <= 0) && (this.spriteSelections.Count <= 0))
            {
                return false;
            }
            this.IsWaitInput = true;
            this.OnBeginWaitInput.Invoke(this);
            return true;
        }

        private void Update()
        {
            if (this.IsWaitInput)
            {
                this.OnUpdateWaitInput.Invoke(this);
            }
        }

        void IAdvSaveData.OnClear()
        {
            this.Clear();
        }

        public AdvEngine Engine
        {
            get
            {
                if (this.engine == null)
                {
                }
                return (this.engine = base.GetComponent<AdvEngine>());
            }
        }

        public bool IsSelected
        {
            get
            {
                return (this.selected != null);
            }
        }

        public bool IsShowing { get; set; }

        public bool IsWaitInput { get; set; }

        public SelectionEvent OnBeginShow
        {
            get
            {
                return this.onBeginShow;
            }
        }

        public SelectionEvent OnBeginWaitInput
        {
            get
            {
                return this.onBeginWaitInput;
            }
        }

        public SelectionEvent OnClear
        {
            get
            {
                return this.onClear;
            }
        }

        public SelectionEvent OnSelected
        {
            get
            {
                return this.onSelected;
            }
        }

        public SelectionEvent OnUpdateWaitInput
        {
            get
            {
                return this.onUpdateWaitInput;
            }
        }

        public string SaveKey
        {
            get
            {
                return "AdvSelectionManager";
            }
        }

        public AdvSelection Selected
        {
            get
            {
                return this.selected;
            }
        }

        public List<AdvSelection> Selections
        {
            get
            {
                return this.selections;
            }
        }

        public List<AdvSelection> SpriteSelections
        {
            get
            {
                return this.spriteSelections;
            }
        }

        public int TotalCount
        {
            get
            {
                return (this.Selections.Count + this.SpriteSelections.Count);
            }
        }

        [CompilerGenerated]
        private sealed class <AddSpriteClickEvent>c__AnonStorey0
        {
            internal AdvSelectionManager $this;
            internal AdvSelection select;

            internal void <>m__0(BaseEventData eventData)
            {
                this.$this.OnSpriteClick(eventData, this.select);
            }
        }
    }
}

