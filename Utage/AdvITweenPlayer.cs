namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    [AddComponentMenu("Utage/ADV/Internal/TweenPlayer")]
    internal class AdvITweenPlayer : MonoBehaviour
    {
        private Action<AdvITweenPlayer> callbackComplete;
        private int count;
        private iTweenData data;
        private Hashtable hashTbl;
        private bool isColorSprite;
        private bool isPlaying;
        private List<AdvITweenPlayer> oldTweenPlayers = new List<AdvITweenPlayer>();
        private AdvEffectColor target;
        private string tweenName;

        public void Cancel()
        {
            iTween.StopByName(base.get_gameObject(), this.tweenName);
            this.isPlaying = false;
            Object.Destroy(this);
        }

        public void Init(iTweenData data, bool isUnder2DSpace, float pixelsToUnits, float skipSpeed, Action<AdvITweenPlayer> callbackComplete)
        {
            this.data = data;
            if (data.Type != iTweenType.Stop)
            {
                this.callbackComplete = callbackComplete;
                data.ReInit();
                this.hashTbl = iTween.Hash(data.MakeHashArray());
                if (iTweenData.IsPostionType(data.Type) && ((!isUnder2DSpace || !this.hashTbl.ContainsKey("islocal")) || !((bool) this.hashTbl["islocal"])))
                {
                    if (this.hashTbl.ContainsKey("x"))
                    {
                        this.hashTbl["x"] = ((float) this.hashTbl["x"]) / pixelsToUnits;
                    }
                    if (this.hashTbl.ContainsKey("y"))
                    {
                        this.hashTbl["y"] = ((float) this.hashTbl["y"]) / pixelsToUnits;
                    }
                    if (this.hashTbl.ContainsKey("z"))
                    {
                        this.hashTbl["z"] = ((float) this.hashTbl["z"]) / pixelsToUnits;
                    }
                }
                if (skipSpeed > 0f)
                {
                    bool flag = this.hashTbl.ContainsKey("speed");
                    if (flag)
                    {
                        this.hashTbl["speed"] = ((float) this.hashTbl["speed"]) * skipSpeed;
                    }
                    if (this.hashTbl.ContainsKey("time"))
                    {
                        this.hashTbl["time"] = ((float) this.hashTbl["time"]) / skipSpeed;
                    }
                    else if (!flag)
                    {
                        this.hashTbl["time"] = 1f / skipSpeed;
                    }
                }
                if ((data.Type == iTweenType.ColorTo) || (data.Type == iTweenType.ColorFrom))
                {
                    this.target = base.get_gameObject().GetComponent<AdvEffectColor>();
                    if (this.target != null)
                    {
                        Color tweenColor = this.target.TweenColor;
                        if (data.Type == iTweenType.ColorTo)
                        {
                            this.hashTbl["from"] = tweenColor;
                            this.hashTbl["to"] = this.ParaseTargetColor(this.hashTbl, tweenColor);
                        }
                        else if (data.Type == iTweenType.ColorFrom)
                        {
                            this.hashTbl["from"] = this.ParaseTargetColor(this.hashTbl, tweenColor);
                            this.hashTbl["to"] = tweenColor;
                        }
                        this.hashTbl["onupdate"] = "OnColorUpdate";
                        this.isColorSprite = true;
                    }
                }
                this.hashTbl["oncomplete"] = "OnCompleteTween";
                this.hashTbl["oncompletetarget"] = base.get_gameObject();
                this.hashTbl["oncompleteparams"] = this;
                this.tweenName = this.GetHashCode().ToString();
                this.hashTbl["name"] = this.tweenName;
            }
        }

        private void OnColorUpdate(Color color)
        {
            if (this.target != null)
            {
                this.target.TweenColor = color;
            }
        }

        private void OnCompleteTween(AdvITweenPlayer arg)
        {
            if (arg == this)
            {
                this.count++;
                if ((this.count >= this.data.LoopCount) && !this.IsEndlessLoop)
                {
                    this.Cancel();
                }
            }
        }

        private void OnDestroy()
        {
            foreach (AdvITweenPlayer player in this.oldTweenPlayers)
            {
                if (player != null)
                {
                    Object.Destroy(player);
                }
            }
            if (this.callbackComplete != null)
            {
                this.callbackComplete(this);
            }
            this.callbackComplete = null;
        }

        private Color ParaseTargetColor(Hashtable hashTbl, Color color)
        {
            if (hashTbl.Contains("color"))
            {
                color = (Color) hashTbl["color"];
            }
            else
            {
                if (hashTbl.Contains("r"))
                {
                    color.r = (float) hashTbl["r"];
                }
                if (hashTbl.Contains("g"))
                {
                    color.g = (float) hashTbl["g"];
                }
                if (hashTbl.Contains("b"))
                {
                    color.b = (float) hashTbl["b"];
                }
                if (hashTbl.Contains("a"))
                {
                    color.a = (float) hashTbl["a"];
                }
            }
            if (hashTbl.Contains("alpha"))
            {
                color.a = (float) hashTbl["alpha"];
            }
            return color;
        }

        public void Play()
        {
            if (this.TryStoreOldTween())
            {
            }
            this.isPlaying = true;
            if (this.data.Type == iTweenType.Stop)
            {
                iTween.Stop(base.get_gameObject());
            }
            else if (this.isColorSprite)
            {
                iTween.ValueTo(base.get_gameObject(), this.hashTbl);
            }
            else
            {
                switch (this.data.Type)
                {
                    case iTweenType.ColorFrom:
                        iTween.ColorFrom(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.ColorTo:
                        iTween.ColorTo(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.MoveAdd:
                        iTween.MoveAdd(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.MoveBy:
                        iTween.MoveBy(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.MoveFrom:
                        iTween.MoveFrom(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.MoveTo:
                        iTween.MoveTo(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.PunchPosition:
                        iTween.PunchPosition(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.PunchRotation:
                        iTween.PunchRotation(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.PunchScale:
                        iTween.PunchScale(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.RotateAdd:
                        iTween.RotateAdd(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.RotateBy:
                        iTween.RotateBy(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.RotateFrom:
                        iTween.RotateFrom(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.RotateTo:
                        iTween.RotateTo(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.ScaleAdd:
                        iTween.ScaleAdd(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.ScaleBy:
                        iTween.ScaleBy(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.ScaleFrom:
                        iTween.ScaleFrom(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.ScaleTo:
                        iTween.ScaleTo(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.ShakePosition:
                        iTween.ShakePosition(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.ShakeRotation:
                        iTween.ShakeRotation(base.get_gameObject(), this.hashTbl);
                        break;

                    case iTweenType.ShakeScale:
                        iTween.ShakeScale(base.get_gameObject(), this.hashTbl);
                        break;

                    default:
                    {
                        this.isPlaying = false;
                        object[] args = new object[] { this.data.Type.ToString() };
                        Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownType, args));
                        break;
                    }
                }
            }
        }

        public void Read(BinaryReader reader, bool isUnder2DSpace, float pixelsToUnits)
        {
            iTweenData data = new iTweenData(reader);
            this.Init(data, isUnder2DSpace, pixelsToUnits, 1f, null);
        }

        internal static void ReadSaveData(BinaryReader reader, GameObject go, bool isUnder2DSpace, float pixelsToUnits)
        {
            int num = reader.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                go.AddComponent<AdvITweenPlayer>().Read(reader, isUnder2DSpace, pixelsToUnits);
            }
        }

        private bool TryStoreOldTween()
        {
            bool flag = false;
            foreach (AdvITweenPlayer player in base.GetComponents<AdvITweenPlayer>())
            {
                if ((player != this) && player.isPlaying)
                {
                    flag = true;
                    this.oldTweenPlayers.Add(player);
                    this.oldTweenPlayers.AddRange(player.oldTweenPlayers);
                }
            }
            return flag;
        }

        public void Write(BinaryWriter writer)
        {
            this.data.Write(writer);
        }

        internal static void WriteSaveData(BinaryWriter writer, GameObject go)
        {
            AdvITweenPlayer[] components = go.GetComponents<AdvITweenPlayer>();
            int num = 0;
            foreach (AdvITweenPlayer player in components)
            {
                if (player.IsEndlessLoop)
                {
                    num++;
                }
            }
            writer.Write(num);
            foreach (AdvITweenPlayer player2 in components)
            {
                if (player2.IsEndlessLoop)
                {
                    player2.Write(writer);
                }
            }
        }

        public bool IsAddType
        {
            get
            {
                iTweenType type = this.data.Type;
                if (((type != iTweenType.MoveAdd) && (type != iTweenType.RotateAdd)) && (type != iTweenType.ScaleAdd))
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsEndlessLoop
        {
            get
            {
                return this.data.IsEndlessLoop;
            }
        }

        public bool IsPlaying
        {
            get
            {
                return this.isPlaying;
            }
        }
    }
}

