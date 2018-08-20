using DG.Tweening;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Utage;

//自定义管理器，推测是六重养成特殊界面的管理器
public class IMManager : MonoBehaviour, IAdvSaveData, IBinaryIO
{
    public static bool active;
    public Sprite[] avatars;
    private string contact;
    private Sprite contactAvatar;
    public GameObject contactMessage;
    public Text contactText;
    public CanvasGroup iMCanvas;
    [Range(1f, 50f)]
    public int maxMessageCharacter = 0x17;
    [Range(100f, 500f)]
    public float maxMessageWidth = 350f;
    public Transform messageParent;
    public ScrollRect messageScroll;
    public Sprite playerAvatar;
    public GameObject playerMessage;
    public static bool waitIM;

    public void AddMessage(string content, bool byPlayer, AdvEngine engine)
    {
        engine.BacklogManager.AddIMLog(content, !byPlayer ? this.contact : "泠珞");
        engine.BacklogManager.AddPage();
        GameObject obj2 = Object.Instantiate<GameObject>(!byPlayer ? this.contactMessage : this.playerMessage, this.messageParent, false);
        obj2.GetComponentInChildren<Text>().set_text(content);
        RectTransform transform = obj2.get_transform().Find("MessageContent");
        if (content.Length > this.maxMessageCharacter)
        {
            transform.GetComponent<ContentSizeFitter>().set_horizontalFit(0);
            transform.set_sizeDelta(new Vector2(this.maxMessageWidth, transform.get_sizeDelta().y));
        }
        else
        {
            transform.GetComponent<ContentSizeFitter>().set_horizontalFit(2);
        }
        obj2.get_transform().Find("Avatar").GetComponent<Image>().set_sprite(!byPlayer ? this.contactAvatar : this.playerAvatar);
        ShortcutExtensions46.DOFade(obj2.GetComponent<CanvasGroup>(), 1f, 0.5f);
        ContentSizeFitter component = this.messageParent.GetComponent<ContentSizeFitter>();
        this.messageScroll.set_verticalNormalizedPosition(0f);
        component.SetLayoutVertical();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) this.messageParent);
        base.Invoke("SetPosition", 0.05f);
    }

    public void ClearMessage()
    {
        IEnumerator enumerator = this.messageParent.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Object.Destroy(((Transform) enumerator.Current).get_gameObject());
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }

    public void HideIM()
    {
        ShortcutExtensions46.DOFade(this.iMCanvas, 0f, 0.5f);
        active = false;
    }

    public void OnClear()
    {
        waitIM = false;
        this.contact = string.Empty;
        this.contactText.set_text(string.Empty);
        this.contactAvatar = null;
        this.iMCanvas.set_alpha(0f);
        active = false;
        IEnumerator enumerator = this.messageParent.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Object.Destroy(((Transform) enumerator.Current).get_gameObject());
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }

    public void OnRead(BinaryReader reader)
    {
        waitIM = reader.ReadBoolean();
        this.contact = reader.ReadString();
        this.contactText.set_text(this.contact);
        string str = reader.ReadString();
        for (int i = 0; i < this.avatars.Length; i++)
        {
            if (this.avatars[i].get_name() == str)
            {
                this.contactAvatar = this.avatars[i];
                break;
            }
        }
        active = reader.ReadBoolean();
        this.iMCanvas.set_alpha(!active ? 0f : 1f);
        int num2 = reader.ReadInt32();
        for (int j = 0; j < num2; j++)
        {
            string str2 = reader.ReadString();
            bool flag = reader.ReadBoolean();
            GameObject obj2 = Object.Instantiate<GameObject>(!flag ? this.contactMessage : this.playerMessage, this.messageParent, false);
            obj2.GetComponentInChildren<Text>().set_text(str2);
            obj2.get_transform().Find("Avatar").GetComponent<Image>().set_sprite(!flag ? this.contactAvatar : this.playerAvatar);
            ContentSizeFitter component = this.messageParent.GetComponent<ContentSizeFitter>();
            this.messageScroll.set_verticalNormalizedPosition(0f);
            component.SetLayoutVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) this.messageParent);
            base.Invoke("SetPosition", 0.05f);
        }
    }

    public void OnWrite(BinaryWriter writer)
    {
        writer.Write(waitIM);
        writer.Write(this.contact);
        writer.Write((this.contactAvatar != null) ? this.contactAvatar.get_name() : string.Empty);
        writer.Write(active);
        writer.Write(this.messageParent.get_childCount());
        for (int i = 0; i < this.messageParent.get_childCount(); i++)
        {
            writer.Write(this.messageParent.GetChild(i).GetComponentInChildren<Text>().get_text());
            writer.Write(this.messageParent.GetChild(i).get_name().Contains("My"));
        }
    }

    public void SetContact(string contactName, string avatarName)
    {
        this.contact = contactName;
        for (int i = 0; i < this.avatars.Length; i++)
        {
            if (this.avatars[i].get_name() == avatarName)
            {
                this.contactAvatar = this.avatars[i];
                break;
            }
        }
        this.contactText.set_text(this.contact);
    }

    private void SetPosition()
    {
        this.messageScroll.set_verticalNormalizedPosition(0f);
    }

    public void ShowIM()
    {
        ShortcutExtensions46.DOFade(this.iMCanvas, 1f, 0.5f);
        active = true;
    }

    public string SaveKey
    {
        get
        {
            return "IMManager";
        }
    }
}

