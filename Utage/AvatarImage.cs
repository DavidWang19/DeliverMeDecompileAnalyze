namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;
    using UtageExtensions;

    [AddComponentMenu("Utage/Lib/UI/AvatarImage"), ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
    public class AvatarImage : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <HasChanged>k__BackingField;
        [SerializeField]
        private Utage.AvatarData avatarData;
        [SerializeField, NovelAvatarPattern("AvatarData")]
        private Utage.AvatarPattern avatarPattern = new Utage.AvatarPattern();
        private RectTransform cachedRectTransform;
        [SerializeField]
        private bool flipX;
        [SerializeField]
        private bool flipY;
        [SerializeField]
        private UnityEngine.Material material;
        public UnityEvent OnPostRefresh = new UnityEvent();
        private RectTransform rootChildren;

        public void ChangePattern(string tag, string patternName)
        {
            this.avatarPattern.SetPatternName(tag, patternName);
            this.HasChanged = true;
        }

        public void Flip(bool flipX, bool flipY)
        {
            this.FlipX = flipX;
            this.FlipY = flipY;
        }

        private void MakeImageFromAvartorData(Utage.AvatarData data)
        {
            if (this.AvatarData != null)
            {
                foreach (Sprite sprite in data.MakeSortedSprites(this.avatarPattern))
                {
                    if (sprite != null)
                    {
                        RectTransform transform = this.RootChildren.AddChildGameObjectComponent<RectTransform>(sprite.get_name());
                        transform.get_gameObject().set_hideFlags(0x34);
                        Image image = transform.get_gameObject().AddComponent<Image>();
                        image.set_material(this.Material);
                        image.set_sprite(sprite);
                        image.SetNativeSize();
                        UguiFlip flip = image.get_gameObject().AddComponent<UguiFlip>();
                        flip.FlipX = this.flipX;
                        flip.FlipY = this.FlipY;
                    }
                }
            }
        }

        private void OnEnable()
        {
            this.HasChanged = true;
        }

        private void Refresh()
        {
            this.RootChildren.DestroyChildrenInEditorOrPlayer();
            this.avatarPattern.Rebuild(this.AvatarData);
            this.MakeImageFromAvartorData(this.AvatarData);
            this.OnPostRefresh.Invoke();
        }

        private void Update()
        {
            if (this.HasChanged)
            {
                this.Refresh();
                this.HasChanged = false;
            }
        }

        public Utage.AvatarData AvatarData
        {
            get
            {
                return this.avatarData;
            }
            set
            {
                this.avatarData = value;
                this.avatarPattern.Rebuild(this.AvatarData);
                this.HasChanged = true;
            }
        }

        public Utage.AvatarPattern AvatarPattern
        {
            get
            {
                return this.avatarPattern;
            }
            set
            {
                this.avatarPattern = value;
                this.HasChanged = true;
            }
        }

        public RectTransform CachedRectTransform
        {
            get
            {
                if (this.cachedRectTransform == null)
                {
                    this.cachedRectTransform = base.GetComponent<RectTransform>();
                }
                return this.cachedRectTransform;
            }
        }

        public bool FlipX
        {
            get
            {
                return this.flipX;
            }
            set
            {
                this.flipX = value;
                this.HasChanged = true;
            }
        }

        public bool FlipY
        {
            get
            {
                return this.flipY;
            }
            set
            {
                this.flipY = value;
                this.HasChanged = true;
            }
        }

        private bool HasChanged { get; set; }

        public UnityEngine.Material Material
        {
            get
            {
                return this.material;
            }
            set
            {
                this.material = value;
                this.HasChanged = true;
            }
        }

        private RectTransform RootChildren
        {
            get
            {
                if (this.rootChildren == null)
                {
                    this.rootChildren = base.get_transform().AddChildGameObjectComponent<RectTransform>("childRoot");
                    this.rootChildren.get_gameObject().set_hideFlags(0x34);
                }
                return this.rootChildren;
            }
        }
    }
}

