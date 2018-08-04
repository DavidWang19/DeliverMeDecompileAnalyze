namespace Utage
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/UI/CrossFadeRawImage")]
    public class UguiCrossFadeRawImage : MonoBehaviour, IMeshModifier, IMaterialModifier
    {
        private UnityEngine.Material corssFadeMaterial;
        [SerializeField]
        private Texture fadeTexture;
        private UnityEngine.Material lastMaterial;
        [SerializeField, Range(0f, 1f)]
        private float strengh = 1f;
        protected Graphic target;
        private Utage.Timer timer;

        private void Awake()
        {
            this.lastMaterial = this.Target.get_material();
            this.corssFadeMaterial = new UnityEngine.Material(ShaderManager.CrossFade);
            this.Material = this.corssFadeMaterial;
        }

        internal void CrossFade(Texture fadeTexture, float time, Action onComplete)
        {
            <CrossFade>c__AnonStorey0 storey = new <CrossFade>c__AnonStorey0 {
                onComplete = onComplete,
                $this = this
            };
            this.FadeTexture = fadeTexture;
            this.Target.get_material().EnableKeyword("CROSS_FADE");
            this.Timer.StartTimer(time, new Action<Utage.Timer>(storey.<>m__0), new Action<Utage.Timer>(storey.<>m__1), 0f);
        }

        public UnityEngine.Material GetModifiedMaterial(UnityEngine.Material baseMaterial)
        {
            baseMaterial.SetFloat("_Strength", this.Strengh);
            baseMaterial.SetTexture("_FadeTex", this.FadeTexture);
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
            if (this.Target.get_mainTexture() != null)
            {
                this.RebuildVertex(vh);
            }
        }

        private void OnDestroy()
        {
            this.Material = this.lastMaterial;
            Object.Destroy(this.corssFadeMaterial);
            Object.Destroy(this.timer);
        }

        public virtual void RebuildVertex(VertexHelper vh)
        {
            vh.Clear();
            Rect pixelAdjustedRect = this.Target.GetPixelAdjustedRect();
            float introduced4 = pixelAdjustedRect.get_x();
            float introduced5 = pixelAdjustedRect.get_y();
            float introduced6 = pixelAdjustedRect.get_x();
            float introduced7 = pixelAdjustedRect.get_y();
            Vector4 vector = new Vector4(introduced4, introduced5, introduced6 + pixelAdjustedRect.get_width(), introduced7 + pixelAdjustedRect.get_height());
            Color color = this.Target.get_color();
            Rect rect2 = (this.Target as RawImage).get_uvRect();
            float introduced8 = rect2.get_xMin();
            float introduced9 = rect2.get_xMin();
            vh.AddVert(new Vector3(vector.x, vector.y), color, new Vector2(introduced8, rect2.get_yMin()), new Vector2(introduced9, rect2.get_yMin()), Vector3.get_zero(), Vector4.get_zero());
            float introduced10 = rect2.get_xMin();
            float introduced11 = rect2.get_xMin();
            vh.AddVert(new Vector3(vector.x, vector.w), color, new Vector2(introduced10, rect2.get_yMax()), new Vector2(introduced11, rect2.get_yMax()), Vector3.get_zero(), Vector4.get_zero());
            float introduced12 = rect2.get_xMax();
            float introduced13 = rect2.get_xMax();
            vh.AddVert(new Vector3(vector.z, vector.w), color, new Vector2(introduced12, rect2.get_yMax()), new Vector2(introduced13, rect2.get_yMax()), Vector3.get_zero(), Vector4.get_zero());
            float introduced14 = rect2.get_xMax();
            float introduced15 = rect2.get_xMax();
            vh.AddVert(new Vector3(vector.z, vector.y), color, new Vector2(introduced14, rect2.get_yMin()), new Vector2(introduced15, rect2.get_yMin()), Vector3.get_zero(), Vector4.get_zero());
            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }

        public Texture FadeTexture
        {
            get
            {
                return this.fadeTexture;
            }
            set
            {
                if (this.fadeTexture != value)
                {
                    this.fadeTexture = value;
                    this.Target.SetVerticesDirty();
                    this.Target.SetMaterialDirty();
                }
            }
        }

        public UnityEngine.Material Material
        {
            get
            {
                return this.Target.get_material();
            }
            set
            {
                this.Target.set_material(value);
            }
        }

        private float Strengh
        {
            get
            {
                return this.strengh;
            }
            set
            {
                this.strengh = value;
                this.Target.SetMaterialDirty();
            }
        }

        public virtual Graphic Target
        {
            get
            {
                if (this.target == null)
                {
                }
                return (this.target = base.GetComponent<RawImage>());
            }
        }

        private Utage.Timer Timer
        {
            get
            {
                if (this.timer == null)
                {
                    this.timer = base.get_gameObject().AddComponent<Utage.Timer>();
                }
                return this.timer;
            }
        }

        [CompilerGenerated]
        private sealed class <CrossFade>c__AnonStorey0
        {
            internal UguiCrossFadeRawImage $this;
            internal Action onComplete;

            internal void <>m__0(Timer x)
            {
                this.$this.Strengh = x.Time01Inverse;
            }

            internal void <>m__1(Timer x)
            {
                this.$this.Target.get_material().DisableKeyword("CROSS_FADE");
                this.onComplete();
            }
        }
    }
}

