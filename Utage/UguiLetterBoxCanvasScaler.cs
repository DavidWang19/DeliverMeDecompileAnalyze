namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode, RequireComponent(typeof(UnityEngine.Canvas)), AddComponentMenu("Utage/Lib/UI/LetterBoxCanvasScaler")]
    public class UguiLetterBoxCanvasScaler : UguiLayoutControllerBase, ILayoutSelfController, ILayoutController
    {
        private UnityEngine.Canvas canvas;
        private Utage.LetterBoxCamera letterBoxCamera;

        private bool IsPrefabAsset()
        {
            return false;
        }

        public void SetLayoutHorizontal()
        {
            this.tracker.Clear();
            if (this.Canvas.get_renderMode() != 2)
            {
                if (!this.IsPrefabAsset())
                {
                    Debug.LogError("LetterBoxCanvas is not RenderMode.World");
                }
            }
            else if (this.LetterBoxCamera == null)
            {
                if (!this.IsPrefabAsset())
                {
                    Debug.LogError("LetterBoxCamera is null");
                }
            }
            else
            {
                this.tracker.Add(this, base.CachedRectTransform, 0x3fe0);
                Vector2 vector = new Vector2(0.5f, 0.5f);
                base.CachedRectTransform.set_anchorMax(vector);
                base.CachedRectTransform.set_anchorMin(vector);
                base.CachedRectTransform.set_sizeDelta(this.LetterBoxCamera.CurrentSize);
                float num = 1f / ((float) this.LetterBoxCamera.PixelsToUnits);
                base.CachedRectTransform.set_localScale((Vector3) (Vector3.get_one() * num));
            }
        }

        public void SetLayoutVertical()
        {
        }

        protected override void Update()
        {
            Vector2 currentSize = this.LetterBoxCamera.CurrentSize;
            if (!Mathf.Approximately(currentSize.x, base.CachedRectTransform.get_sizeDelta().x) || !Mathf.Approximately(currentSize.y, base.CachedRectTransform.get_sizeDelta().y))
            {
                base.SetDirty();
            }
            else
            {
                float num = 1f / ((float) this.LetterBoxCamera.PixelsToUnits);
                if ((!Mathf.Approximately(num, base.CachedRectTransform.get_localScale().x) || !Mathf.Approximately(num, base.CachedRectTransform.get_localScale().y)) || !Mathf.Approximately(num, base.CachedRectTransform.get_localScale().z))
                {
                    base.SetDirty();
                }
            }
        }

        public UnityEngine.Canvas Canvas
        {
            get
            {
                if (this.canvas == null)
                {
                    this.canvas = base.GetComponent<UnityEngine.Canvas>();
                }
                return this.canvas;
            }
        }

        public Utage.LetterBoxCamera LetterBoxCamera
        {
            get
            {
                if (this.letterBoxCamera == null)
                {
                    if (this.Canvas.get_worldCamera() == null)
                    {
                        if (!this.IsPrefabAsset())
                        {
                            Debug.LogError("Canvas worldCamera is null");
                        }
                    }
                    else
                    {
                        this.letterBoxCamera = this.Canvas.get_worldCamera().GetComponent<Utage.LetterBoxCamera>();
                    }
                }
                return this.letterBoxCamera;
            }
        }
    }
}

