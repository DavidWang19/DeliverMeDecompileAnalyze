namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/UI/Transition")]
    public class UguiTransition : MonoBehaviour, IMaterialModifier, IMeshModifier
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Material <DefaultMaterial>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsPremultipliedAlpha>k__BackingField;
        [SerializeField]
        private Texture ruleTexture;
        [SerializeField, Range(0f, 1f)]
        private float strengh;
        private Graphic target;
        [SerializeField, Range(0.001f, 1f)]
        private float vague = 0.2f;

        private void Awake()
        {
            this.Target.SetAllDirty();
        }

        public Material GetModifiedMaterial(Material baseMaterial)
        {
            baseMaterial.SetFloat("_Strength", this.Strengh);
            baseMaterial.SetFloat("_Vague", this.Vague);
            baseMaterial.SetTexture("_RuleTex", this.RuleTexture);
            if (this.IsPremultipliedAlpha)
            {
                baseMaterial.SetInt("_SrcBlend", 1);
                baseMaterial.SetInt("_DstBlend", 5);
                baseMaterial.EnableKeyword("PREMULTIPLIED_ALPHA");
                return baseMaterial;
            }
            baseMaterial.SetInt("_SrcBlend", 5);
            baseMaterial.SetInt("_DstBlend", 10);
            baseMaterial.DisableKeyword("PREMULTIPLIED_ALPHA");
            return baseMaterial;
        }

        public void ModifyMesh(Mesh mesh)
        {
            using (VertexHelper helper = new VertexHelper(mesh))
            {
                this.ModifyMesh(helper);
                helper.FillMesh(mesh);
            }
        }

        public void ModifyMesh(VertexHelper vh)
        {
            if (base.get_enabled())
            {
                Rect rect = this.Target.get_rectTransform().get_rect();
                Vector2 vector = this.Target.get_rectTransform().get_pivot();
                float num = rect.get_width();
                float num2 = rect.get_height();
                UIVertex vertex = new UIVertex();
                for (int i = 0; i < vh.get_currentVertCount(); i++)
                {
                    vh.PopulateUIVertex(ref vertex, i);
                    vertex.uv1 = new Vector2((vertex.position.x / num) + vector.x, (vertex.position.y / num2) + vector.y);
                    vh.SetUIVertex(vertex, i);
                }
            }
        }

        public void RuleFadeIn(float time, Action onComplete)
        {
            <RuleFadeIn>c__AnonStorey0 storey = new <RuleFadeIn>c__AnonStorey0 {
                onComplete = onComplete,
                $this = this
            };
            this.Target.set_material(new Material(ShaderManager.RuleFade));
            storey.timer = base.get_gameObject().AddComponent<Timer>();
            storey.timer.StartTimer(time, new Action<Timer>(storey.<>m__0), new Action<Timer>(storey.<>m__1), 0f);
        }

        public void RuleFadeIn(Texture texture, float vague, bool isPremultipliedAlpha, float time, Action onComplete)
        {
            this.RuleTexture = texture;
            this.Vague = vague;
            this.IsPremultipliedAlpha = isPremultipliedAlpha;
            this.RuleFadeIn(time, onComplete);
        }

        public void RuleFadeOut(float time, Action onComplete)
        {
            <RuleFadeOut>c__AnonStorey1 storey = new <RuleFadeOut>c__AnonStorey1 {
                onComplete = onComplete,
                $this = this
            };
            this.Target.set_material(new Material(ShaderManager.RuleFade));
            storey.timer = base.get_gameObject().AddComponent<Timer>();
            storey.timer.StartTimer(time, new Action<Timer>(storey.<>m__0), new Action<Timer>(storey.<>m__1), 0f);
        }

        public void RuleFadeOut(Texture texture, float vague, bool isPremultipliedAlpha, float time, Action onComplete)
        {
            this.RuleTexture = texture;
            this.Vague = vague;
            this.IsPremultipliedAlpha = isPremultipliedAlpha;
            this.RuleFadeOut(time, onComplete);
        }

        private Material DefaultMaterial { get; set; }

        public bool IsPremultipliedAlpha { get; set; }

        public Texture RuleTexture
        {
            get
            {
                return this.ruleTexture;
            }
            set
            {
                if (this.ruleTexture != value)
                {
                    this.ruleTexture = value;
                    this.Target.SetMaterialDirty();
                }
            }
        }

        public float Strengh
        {
            get
            {
                return this.strengh;
            }
            set
            {
                if (!Mathf.Approximately(this.strengh, value))
                {
                    this.strengh = value;
                    this.Target.SetMaterialDirty();
                }
            }
        }

        public Graphic Target
        {
            get
            {
                if (this.target == null)
                {
                    this.Target = base.GetComponent<Graphic>();
                }
                return this.target;
            }
            set
            {
                this.target = value;
                this.DefaultMaterial = this.target.get_material();
                this.Target.SetMaterialDirty();
            }
        }

        public float Vague
        {
            get
            {
                return this.vague;
            }
            set
            {
                if (!Mathf.Approximately(this.vague, value))
                {
                    this.vague = value;
                    this.Target.SetMaterialDirty();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <RuleFadeIn>c__AnonStorey0
        {
            internal UguiTransition $this;
            internal Action onComplete;
            internal Timer timer;

            internal void <>m__0(Timer x)
            {
                this.$this.Strengh = x.Time01Inverse;
            }

            internal void <>m__1(Timer x)
            {
                this.$this.Target.set_material(this.$this.DefaultMaterial);
                Object.Destroy(this.timer);
                this.onComplete();
            }
        }

        [CompilerGenerated]
        private sealed class <RuleFadeOut>c__AnonStorey1
        {
            internal UguiTransition $this;
            internal Action onComplete;
            internal Timer timer;

            internal void <>m__0(Timer x)
            {
                this.$this.Strengh = x.Time01;
            }

            internal void <>m__1(Timer x)
            {
                this.$this.Target.set_material(this.$this.DefaultMaterial);
                Object.Destroy(this.timer);
                this.onComplete();
            }
        }
    }
}

