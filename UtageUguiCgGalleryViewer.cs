using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utage;

[AddComponentMenu("Utage/TemplateUI/CgGalleryViewer")]
public class UtageUguiCgGalleryViewer : UguiView, IPointerClickHandler, IDragHandler, IPointerDownHandler, IEventSystemHandler
{
    private int currentIndex;
    private AdvCgGalleryData data;
    [SerializeField]
    private AdvEngine engine;
    public UtageUguiGallery gallery;
    private bool isEnableClick;
    private bool isLoadEnd;
    [SerializeField]
    private UnityEngine.UI.ScrollRect scrollRect;
    private Vector3 startContentPosition;
    public AdvUguiLoadGraphicFile texture;

    private void Awake()
    {
        this.texture.OnLoadEnd.AddListener(new UnityAction(this, (IntPtr) this.OnLoadEnd));
    }

    private void LoadCurrentTexture()
    {
        this.isLoadEnd = false;
        this.isEnableClick = false;
        this.ScrollRect.set_enabled(false);
        this.ScrollRect.get_content().set_localPosition(this.startContentPosition);
        AdvTextureSettingData dataOpened = this.data.GetDataOpened(this.currentIndex);
        this.texture.LoadFile(this.Engine.DataManager.SettingDataManager.TextureSetting.LabelToGraphic(dataOpened.Key).Main);
    }

    private void OnClose()
    {
        this.ScrollRect.get_content().set_localPosition(this.startContentPosition);
        this.texture.ClearFile();
        this.gallery.WakeUp();
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.isEnableClick = false;
    }

    private void OnLoadEnd()
    {
        this.isLoadEnd = true;
        this.isEnableClick = false;
        this.ScrollRect.set_enabled(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.isEnableClick)
        {
            this.currentIndex++;
            if (this.currentIndex >= this.data.NumOpen)
            {
                this.Back();
            }
            else
            {
                this.LoadCurrentTexture();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.isLoadEnd)
        {
            this.isEnableClick = true;
        }
    }

    public void Open(AdvCgGalleryData data)
    {
        this.gallery.Sleep();
        this.Open();
        this.data = data;
        this.currentIndex = 0;
        this.startContentPosition = this.ScrollRect.get_content().get_localPosition();
        this.LoadCurrentTexture();
    }

    private void Update()
    {
        if (InputUtil.IsMouseRightButtonDown())
        {
            this.Back();
        }
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

    public UnityEngine.UI.ScrollRect ScrollRect
    {
        get
        {
            if (this.scrollRect == null)
            {
                this.scrollRect = base.GetComponent<UnityEngine.UI.ScrollRect>();
                if (this.scrollRect == null)
                {
                    this.scrollRect = base.get_gameObject().AddComponent<UnityEngine.UI.ScrollRect>();
                    this.scrollRect.set_movementType(2);
                }
                if (this.scrollRect.get_content() == null)
                {
                    this.scrollRect.set_content(this.texture.get_transform() as RectTransform);
                }
            }
            return this.scrollRect;
        }
    }
}

