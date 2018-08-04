namespace Utage
{
    using System;
    using System.IO;
    using UnityEngine;
    using UtageExtensions;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/Camera/LetterBoxCamera")]
    public class LetterBoxCamera : MonoBehaviour
    {
        [SerializeField]
        private AnchorType anchor = AnchorType.MiddleCenter;
        private Camera cachedCamera;
        private Vector2 currentSize;
        private bool hasChanged = true;
        [SerializeField]
        private int height = 600;
        [SerializeField]
        private bool isFlexible;
        [SerializeField]
        private int maxHeight = 600;
        [SerializeField]
        private int maxWidth = 800;
        public LetterBoxCameraEvent OnGameScreenSizeChange = new LetterBoxCameraEvent();
        private Vector2 padding;
        [SerializeField]
        private int pixelsToUnits = 100;
        private float screenAspectRatio;
        private const int Version = 0;
        [SerializeField]
        private int width = 800;
        [SerializeField]
        public float zoom2D = 1f;
        [SerializeField]
        public Vector2 zoom2DCenter;

        internal void OnClear()
        {
            this.Zoom2D = 1f;
            this.Zoom2DCenter = Vector2.get_zero();
        }

        private void OnValidate()
        {
            this.hasChanged = true;
        }

        public void Read(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if ((num < 0) || (num > 0))
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
            else
            {
                this.Zoom2D = reader.ReadSingle();
                this.Zoom2DCenter = reader.ReadVector2();
            }
        }

        public void Refresh()
        {
            this.hasChanged = false;
            this.RefreshCurrentSize();
            this.RefreshCamera();
        }

        private void RefreshCamera()
        {
            float x = this.padding.x;
            float num2 = 1f - (this.padding.x * 2f);
            float y = this.padding.y;
            float num4 = 1f - (this.padding.y * 2f);
            switch (this.anchor)
            {
                case AnchorType.UpperLeft:
                    x = 0f;
                    y = this.padding.y * 2f;
                    break;

                case AnchorType.UpperCenter:
                    y = this.padding.y * 2f;
                    break;

                case AnchorType.UpperRight:
                    x = this.padding.x * 2f;
                    y = this.padding.y * 2f;
                    break;

                case AnchorType.MiddleLeft:
                    x = 0f;
                    break;

                case AnchorType.MiddleRight:
                    x = this.padding.x * 2f;
                    break;

                case AnchorType.LowerLeft:
                    x = 0f;
                    y = 0f;
                    break;

                case AnchorType.LowerCenter:
                    y = 0f;
                    break;

                case AnchorType.LowerRight:
                    x = this.padding.x * 2f;
                    y = 0f;
                    break;
            }
            Rect rect = new Rect(x, y, num2, num4);
            this.CachedCamera.set_orthographicSize((this.CurrentSize.y / ((float) (2 * this.pixelsToUnits))) / this.Zoom2D);
            this.CachedCamera.set_rect(rect);
            Vector2 vector2 = (Vector2) ((((-1f / this.Zoom2D) + 1f) * this.Zoom2DCenter) / ((float) this.pixelsToUnits));
            this.CachedCamera.get_transform().set_localPosition(vector2);
        }

        private void RefreshCurrentSize()
        {
            if (this.TryRefreshCurrentSize())
            {
                this.OnGameScreenSizeChange.Invoke(this);
            }
        }

        internal void SetZoom2D(float zoom, Vector2 center)
        {
            this.Zoom2D = zoom;
            this.Zoom2DCenter = center;
        }

        private void Start()
        {
            this.hasChanged = true;
        }

        private bool TryRefreshCurrentSize()
        {
            int flexibleMaxWidth;
            int flexibleMinHeight;
            this.screenAspectRatio = (1f * Screen.get_width()) / ((float) Screen.get_height());
            float num = ((float) this.Width) / ((float) this.Height);
            float num2 = ((float) this.FlexibleMaxWidth) / ((float) this.FlexibleMinHeight);
            float num3 = ((float) this.FlexibleMinWidth) / ((float) this.FlexibleMaxHeight);
            if (this.screenAspectRatio > num2)
            {
                this.padding.x = (1f - (num2 / this.screenAspectRatio)) / 2f;
                this.padding.y = 0f;
                flexibleMaxWidth = this.FlexibleMaxWidth;
                flexibleMinHeight = this.FlexibleMinHeight;
            }
            else if (this.screenAspectRatio < num3)
            {
                this.padding.x = 0f;
                this.padding.y = (1f - (this.screenAspectRatio / num3)) / 2f;
                flexibleMaxWidth = this.FlexibleMinWidth;
                flexibleMinHeight = this.FlexibleMaxHeight;
            }
            else
            {
                this.padding.x = 0f;
                this.padding.y = 0f;
                if (Mathf.Approximately(this.screenAspectRatio, num))
                {
                    flexibleMaxWidth = this.Width;
                    flexibleMinHeight = this.Height;
                }
                else
                {
                    flexibleMinHeight = this.FlexibleMinHeight;
                    flexibleMaxWidth = Mathf.FloorToInt(this.screenAspectRatio * flexibleMinHeight);
                    if (flexibleMaxWidth < this.FlexibleMinWidth)
                    {
                        flexibleMaxWidth = this.FlexibleMinWidth;
                        flexibleMinHeight = Mathf.FloorToInt(((float) flexibleMaxWidth) / this.screenAspectRatio);
                    }
                }
            }
            bool flag = (this.currentSize.x != flexibleMaxWidth) || (this.currentSize.y != flexibleMinHeight);
            this.currentSize = new Vector2((float) flexibleMaxWidth, (float) flexibleMinHeight);
            return flag;
        }

        private void Update()
        {
            if (this.hasChanged || !Mathf.Approximately(this.screenAspectRatio, (1f * Screen.get_width()) / ((float) Screen.get_height())))
            {
                this.Refresh();
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.Zoom2D);
            writer.Write(this.Zoom2DCenter);
        }

        public Camera CachedCamera
        {
            get
            {
                if (this.cachedCamera == null)
                {
                    this.cachedCamera = base.GetComponent<Camera>();
                }
                return this.cachedCamera;
            }
        }

        public Vector2 CurrentSize
        {
            get
            {
                if (this.hasChanged)
                {
                    this.RefreshCurrentSize();
                }
                return this.currentSize;
            }
        }

        public int FlexibleMaxHeight
        {
            get
            {
                return (!this.IsFlexible ? this.Height : Mathf.Max(new int[] { this.Height, this.Height, this.MaxHeight }));
            }
        }

        public int FlexibleMaxWidth
        {
            get
            {
                return (!this.IsFlexible ? this.Width : Mathf.Max(new int[] { this.Width, this.Width, this.MaxWidth }));
            }
        }

        public int FlexibleMinHeight
        {
            get
            {
                return (!this.IsFlexible ? this.Height : Mathf.Min(new int[] { this.Height, this.Height, this.MaxHeight }));
            }
        }

        public int FlexibleMinWidth
        {
            get
            {
                return (!this.IsFlexible ? this.Width : Mathf.Min(new int[] { this.Width, this.Width, this.MaxWidth }));
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.hasChanged = true;
                this.height = value;
            }
        }

        public bool IsFlexible
        {
            get
            {
                return this.isFlexible;
            }
            set
            {
                this.hasChanged = true;
                this.isFlexible = value;
            }
        }

        public int MaxHeight
        {
            get
            {
                return this.maxHeight;
            }
            set
            {
                this.hasChanged = true;
                this.maxHeight = value;
            }
        }

        public int MaxWidth
        {
            get
            {
                return this.maxWidth;
            }
            set
            {
                this.hasChanged = true;
                this.maxWidth = value;
            }
        }

        public int PixelsToUnits
        {
            get
            {
                return this.pixelsToUnits;
            }
            set
            {
                this.hasChanged = true;
                this.pixelsToUnits = value;
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.hasChanged = true;
                this.width = value;
            }
        }

        public float Zoom2D
        {
            get
            {
                return this.zoom2D;
            }
            set
            {
                this.zoom2D = value;
                this.hasChanged = true;
            }
        }

        public Vector2 Zoom2DCenter
        {
            get
            {
                return this.zoom2DCenter;
            }
            set
            {
                this.zoom2DCenter = value;
                this.hasChanged = true;
            }
        }

        public enum AnchorType
        {
            UpperLeft,
            UpperCenter,
            UpperRight,
            MiddleLeft,
            MiddleCenter,
            MiddleRight,
            LowerLeft,
            LowerCenter,
            LowerRight
        }
    }
}

