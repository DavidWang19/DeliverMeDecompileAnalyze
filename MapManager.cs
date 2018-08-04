using DG.Tweening;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utage;

public class MapManager : MonoBehaviour, IAdvSaveData, IBinaryIO
{
    public Button[] areaButtons;
    public Sprite[] avatars;
    public RawImage cityImage;
    public GameObject cityMap;
    public Texture[] cityTextures;
    private string currentMapData = string.Empty;
    private AdvEngine engine;
    public Button goCityButton;
    public Button goSchoolButton;
    public CanvasGroup mapCanvas;
    public RawImage schoolImage;
    public GameObject schoolMap;
    public Texture[] schoolTextures;
    private bool shown;
    [HideInInspector]
    public bool wait;

    private void Awake()
    {
        this.goSchoolButton.get_onClick().RemoveAllListeners();
        this.goSchoolButton.get_onClick().AddListener(new UnityAction(this, (IntPtr) this.SwitchToSchool));
        this.goCityButton.get_onClick().RemoveAllListeners();
        this.goCityButton.get_onClick().AddListener(new UnityAction(this, (IntPtr) this.SwitchToCity));
    }

    private void ClickArea(string clickJumpValue)
    {
        this.mapCanvas.set_blocksRaycasts(false);
        this.engine.Param.TrySetParameter("mapControl", clickJumpValue);
        this.wait = false;
    }

    private Sprite GetAvatarByName(string avatarName)
    {
        for (int i = 0; i < this.avatars.Length; i++)
        {
            if (this.avatars[i].get_name() == avatarName)
            {
                return this.avatars[i];
            }
        }
        Debug.Log("Cannot find avatar: " + avatarName);
        return null;
    }

    private string GetRandomMapData(string mapDatas)
    {
        char[] separator = new char[] { '|' };
        string[] strArray = mapDatas.Split(separator);
        Random.InitState(this.engine.Param.GetParameterInt("seed"));
        int parameterInt = this.engine.Param.GetParameterInt("day");
        int index = 0;
        for (int i = 0; i < parameterInt; i++)
        {
            index = Random.Range(0, strArray.Length);
        }
        return strArray[index];
    }

    public void HideMap()
    {
        ShortcutExtensions46.DOFade(this.mapCanvas, 0f, 0.5f);
        this.mapCanvas.set_blocksRaycasts(false);
        this.shown = false;
    }

    public void OnClear()
    {
        this.wait = false;
        this.mapCanvas.set_alpha(0f);
        this.mapCanvas.set_blocksRaycasts(false);
        this.shown = false;
        foreach (Button button in this.areaButtons)
        {
            button.get_gameObject().SetActive(false);
        }
    }

    public void OnRead(BinaryReader reader)
    {
        this.wait = reader.ReadBoolean();
        this.shown = reader.ReadBoolean();
        this.currentMapData = reader.ReadString();
        this.engine = GameObject.Find("AdvEngine").GetComponent<AdvEngine>();
        if (this.shown)
        {
            this.ShowMap(this.engine, this.currentMapData, true);
        }
        this.mapCanvas.set_blocksRaycasts(reader.ReadBoolean());
    }

    public void OnWrite(BinaryWriter writer)
    {
        writer.Write(this.wait);
        writer.Write(this.shown);
        writer.Write(this.currentMapData);
        writer.Write(this.mapCanvas.get_blocksRaycasts());
    }

    public void ShowMap(AdvEngine advEngine, string mapDataName, bool onRead = false)
    {
        ShortcutExtensions46.DOFade(this.mapCanvas, 1f, !onRead ? 0.5f : 0f);
        this.shown = true;
        this.mapCanvas.set_blocksRaycasts(true);
        this.engine = advEngine;
        if (mapDataName.Contains("|"))
        {
            mapDataName = this.GetRandomMapData(mapDataName);
        }
        this.currentMapData = mapDataName;
        if (mapDataName.Substring(0, 2).Contains("重"))
        {
            mapDataName = mapDataName.Substring(0, 2) + "/" + mapDataName;
        }
        string[][] arrayCollection = MyTool.ParseTSV(MyTool.LoadText("MapData/" + mapDataName));
        List<string> list = new List<string>(MyTool.GetArrayByFirstElement(arrayCollection, "Area"));
        foreach (Button button in this.areaButtons)
        {
            bool flag = false;
            string[] arrayByFirstElement = new string[0];
            if (list.Contains(button.get_gameObject().get_name()))
            {
                flag = true;
                arrayByFirstElement = MyTool.GetArrayByFirstElement(arrayCollection, button.get_gameObject().get_name());
            }
            button.get_gameObject().SetActive(flag);
            if (flag)
            {
                <ShowMap>c__AnonStorey0 storey = new <ShowMap>c__AnonStorey0 {
                    $this = this,
                    clickLable = arrayByFirstElement[1]
                };
                button.get_onClick().RemoveAllListeners();
                button.get_onClick().AddListener(new UnityAction(storey, (IntPtr) this.<>m__0));
                Image[] componentsInChildren = button.GetComponentsInChildren<Image>();
                componentsInChildren[1].set_enabled(false);
                componentsInChildren[2].set_enabled(false);
                if (arrayByFirstElement.Length > 2)
                {
                    componentsInChildren[1].set_sprite(this.GetAvatarByName(arrayByFirstElement[2]));
                    componentsInChildren[1].set_enabled(true);
                }
                if (arrayByFirstElement.Length > 3)
                {
                    componentsInChildren[2].set_sprite(this.GetAvatarByName(arrayByFirstElement[3]));
                    componentsInChildren[2].set_enabled(true);
                }
            }
        }
        bool flag2 = MyTool.GetArrayByFirstElement(arrayCollection, "Mode")[1] == "school";
        int index = MyTool.ParseInt(MyTool.GetArrayByFirstElement(arrayCollection, "Mode")[2], 1) - 1;
        index = Mathf.Clamp(index, 0, this.schoolTextures.Length - 1);
        this.schoolImage.set_texture(this.schoolTextures[index]);
        this.cityImage.set_texture(this.cityTextures[index]);
        if (flag2)
        {
            this.SwitchToSchool();
        }
        else
        {
            this.SwitchToCity();
        }
    }

    private void SwitchToCity()
    {
        this.schoolMap.SetActive(false);
        this.cityMap.SetActive(true);
    }

    private void SwitchToSchool()
    {
        this.schoolMap.SetActive(true);
        this.cityMap.SetActive(false);
    }

    public string SaveKey
    {
        get
        {
            return "MapManager";
        }
    }

    [CompilerGenerated]
    private sealed class <ShowMap>c__AnonStorey0
    {
        internal MapManager $this;
        internal string clickLable;

        internal void <>m__0()
        {
            this.$this.ClickArea(this.clickLable);
        }
    }
}

