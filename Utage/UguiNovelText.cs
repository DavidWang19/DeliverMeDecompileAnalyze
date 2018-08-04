namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [RequireComponent(typeof(UguiNovelTextGenerator)), AddComponentMenu("Utage/Lib/UI/NovelText")]
    public class UguiNovelText : Text
    {
        private bool isDirtyVerts;
        private readonly UIVertex[] m_TempVerts = new UIVertex[4];
        private UguiNovelTextGenerator textGenerator;

        protected override void Awake()
        {
            base.Awake();
            UnityAction b = new UnityAction(this, (IntPtr) this.OnDirtyVerts);
            base.m_OnDirtyVertsCallback = (UnityAction) Delegate.Combine(base.m_OnDirtyVertsCallback, b);
        }

        public int GetTotalLineHeight(int fontSize)
        {
            return Mathf.CeilToInt(fontSize * (base.get_lineSpacing() + 0.2f));
        }

        private void OnDirtyVerts()
        {
            this.TextGenerator.ChangeAll();
            this.isDirtyVerts = true;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if ((base.get_font() != null) && !this.TextGenerator.IsRequestingCharactersInTexture)
            {
                if (!this.isDirtyVerts)
                {
                    this.TextGenerator.IsRebuidFont = true;
                }
                IList<UIVertex> list = this.TextGenerator.CreateVertex();
                vh.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    int index = i & 3;
                    this.m_TempVerts[index] = list[i];
                    if (index == 3)
                    {
                        vh.AddUIVertexQuad(this.m_TempVerts);
                    }
                }
                this.isDirtyVerts = false;
            }
        }

        public override void SetAllDirty()
        {
            this.TextGenerator.ChangeAll();
            base.SetAllDirty();
        }

        internal void SetVerticesOnlyDirty()
        {
            this.TextGenerator.SetVerticesOnlyDirty();
        }

        public Vector3 CurrentEndPosition
        {
            get
            {
                this.TextGenerator.RefreshEndPosition();
                return this.TextGenerator.EndPosition;
            }
        }

        public Vector3 EndPosition
        {
            get
            {
                return this.TextGenerator.EndPosition;
            }
        }

        public int LengthOfView
        {
            get
            {
                return this.TextGenerator.LengthOfView;
            }
            set
            {
                this.TextGenerator.LengthOfView = value;
            }
        }

        public override float preferredHeight
        {
            get
            {
                return this.TextGenerator.PreferredHeight;
            }
        }

        public UguiNovelTextGenerator TextGenerator
        {
            get
            {
                if (this.textGenerator == null)
                {
                }
                return (this.textGenerator = base.GetComponent<UguiNovelTextGenerator>());
            }
        }
    }
}

