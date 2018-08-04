namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class iTweenData
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$mapA;
        public const string A = "a";
        public const string Alpha = "alpha";
        private static readonly string[][] ArgTbl;
        public const string B = "b";
        public static Func<string, object> CallbackGetValue;
        public const string Color = "color";
        public const string Delay = "delay";
        public const string EaseType = "easeType";
        private string errorMsg;
        public const string G = "g";
        private Dictionary<string, object> hashObjects;
        private bool isDynamic;
        public const string Islocal = "islocal";
        private int loopCount;
        private iTween.LoopType loopType;
        public const string LoopType = "loopType";
        public const string R = "r";
        public const string Speed = "speed";
        private string strArg;
        private string strEaseType;
        private string strLoopType;
        private string strType;
        public const string Time = "time";
        private iTweenType type;
        public const string X = "x";
        public const string Y = "y";
        public const string Z = "z";

        static iTweenData()
        {
            string[][] textArrayArray1 = new string[0x15][];
            textArrayArray1[0] = new string[] { "time", "delay", "color", "alpha", "r", "g", "b", "a" };
            textArrayArray1[1] = new string[] { "time", "delay", "color", "alpha", "r", "g", "b", "a" };
            textArrayArray1[2] = new string[] { "time", "delay", "x", "y", "z", "speed" };
            textArrayArray1[3] = new string[] { "time", "delay", "x", "y", "z", "speed" };
            textArrayArray1[4] = new string[] { "time", "delay", "x", "y", "z", "speed", "islocal" };
            textArrayArray1[5] = new string[] { "time", "delay", "x", "y", "z", "speed", "islocal" };
            textArrayArray1[6] = new string[] { "time", "delay", "x", "y", "z" };
            textArrayArray1[7] = new string[] { "time", "delay", "x", "y", "z" };
            textArrayArray1[8] = new string[] { "time", "delay", "x", "y", "z" };
            textArrayArray1[9] = new string[] { "time", "delay", "x", "y", "z", "speed" };
            textArrayArray1[10] = new string[] { "time", "delay", "x", "y", "z", "speed" };
            textArrayArray1[11] = new string[] { "time", "delay", "x", "y", "z", "speed", "islocal" };
            textArrayArray1[12] = new string[] { "time", "delay", "x", "y", "z", "speed", "islocal" };
            textArrayArray1[13] = new string[] { "time", "delay", "x", "y", "z", "speed" };
            textArrayArray1[14] = new string[] { "time", "delay", "x", "y", "z", "speed" };
            textArrayArray1[15] = new string[] { "time", "delay", "x", "y", "z", "speed" };
            textArrayArray1[0x10] = new string[] { "time", "delay", "x", "y", "z", "speed" };
            textArrayArray1[0x11] = new string[] { "time", "delay", "x", "y", "z", "islocal" };
            textArrayArray1[0x12] = new string[] { "time", "delay", "x", "y", "z" };
            textArrayArray1[0x13] = new string[] { "time", "delay", "x", "y", "z" };
            textArrayArray1[20] = new string[0];
            ArgTbl = textArrayArray1;
        }

        public iTweenData(BinaryReader reader)
        {
            this.hashObjects = new Dictionary<string, object>();
            this.errorMsg = string.Empty;
            string type = reader.ReadString();
            string arg = reader.ReadString();
            string easeType = reader.ReadString();
            string loopType = reader.ReadString();
            this.Init(type, arg, easeType, loopType);
        }

        public iTweenData(iTweenType type, string arg)
        {
            this.hashObjects = new Dictionary<string, object>();
            this.errorMsg = string.Empty;
            this.Init(type.ToString(), arg, string.Empty, string.Empty);
        }

        public iTweenData(string type, string arg, string easeType, string loopType)
        {
            this.hashObjects = new Dictionary<string, object>();
            this.errorMsg = string.Empty;
            this.Init(type, arg, easeType, loopType);
        }

        private void AddErrorMsg(string msg)
        {
            if (!string.IsNullOrEmpty(this.errorMsg))
            {
                this.errorMsg = this.errorMsg + "\n";
            }
            this.errorMsg = this.errorMsg + msg;
        }

        private static bool CheckArg(iTweenType type, string name)
        {
            return (Array.IndexOf<string>(ArgTbl[(int) type], name) >= 0);
        }

        private void Init(string type, string arg, string easeType, string loopType)
        {
            this.strType = type;
            this.strArg = arg;
            this.strEaseType = easeType;
            this.strLoopType = loopType;
            this.ParseParameters(type, arg);
            if (!string.IsNullOrEmpty(easeType))
            {
                this.HashObjects.Add("easeType", easeType);
            }
            if (!string.IsNullOrEmpty(loopType))
            {
                try
                {
                    this.ParseLoopType(loopType);
                    this.HashObjects.Add("loopType", this.loopType);
                }
                catch (Exception exception)
                {
                    this.AddErrorMsg(loopType + "は、LoopTypeとして解析できません。");
                    this.AddErrorMsg(exception.Message);
                }
            }
        }

        public static bool IsPostionType(iTweenType type)
        {
            switch (type)
            {
                case iTweenType.MoveAdd:
                case iTweenType.MoveBy:
                case iTweenType.MoveFrom:
                case iTweenType.MoveTo:
                case iTweenType.PunchPosition:
                case iTweenType.ShakePosition:
                    return true;
            }
            return false;
        }

        public object[] MakeHashArray()
        {
            List<object> list = new List<object>();
            foreach (KeyValuePair<string, object> pair in this.HashObjects)
            {
                list.Add(pair.Key);
                list.Add(pair.Value);
            }
            return list.ToArray();
        }

        private void ParseLoopType(string loopTypeStr)
        {
            this.loopType = iTween.LoopType.none;
            this.loopCount = 0;
            char[] separator = new char[] { ' ', '=' };
            string[] strArray = loopTypeStr.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length != 2)
            {
                throw new Exception();
            }
            this.loopType = (iTween.LoopType) Enum.Parse(typeof(iTween.LoopType), strArray[0]);
            this.loopCount = int.Parse(strArray[1]);
        }

        private void ParseParameters(string type, string arg)
        {
            try
            {
                this.type = (iTweenType) Enum.Parse(typeof(iTweenType), type);
                if (this.type != iTweenType.Stop)
                {
                    char[] separator = new char[] { ' ', '=' };
                    string[] strArray = arg.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (((strArray.Length % 2) != 0) || (strArray.Length <= 0))
                    {
                        this.AddErrorMsg(arg + "内が、「パラメーター名=値」 の形式で書かれていません。");
                    }
                    else
                    {
                        for (int i = 0; i < (strArray.Length / 2); i++)
                        {
                            string key = strArray[i * 2];
                            this.HashObjects.Add(key, ParseValue(this.type, key, strArray[(i * 2) + 1], ref this.isDynamic));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.AddErrorMsg(arg + "内が、「パラメーター名=値」 の形式で書かれていません。");
                this.AddErrorMsg(exception.Message);
            }
        }

        private static object ParseValue(iTweenType type, string name, string valueString, ref bool isDynamic)
        {
            object obj2 = null;
            if (CallbackGetValue != null)
            {
                obj2 = CallbackGetValue(valueString);
                isDynamic = true;
            }
            if (CheckArg(type, name) && (name != null))
            {
                int num;
                if (<>f__switch$mapA == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(13);
                    dictionary.Add("time", 0);
                    dictionary.Add("delay", 0);
                    dictionary.Add("speed", 0);
                    dictionary.Add("alpha", 0);
                    dictionary.Add("r", 0);
                    dictionary.Add("g", 0);
                    dictionary.Add("b", 0);
                    dictionary.Add("a", 0);
                    dictionary.Add("x", 0);
                    dictionary.Add("y", 0);
                    dictionary.Add("z", 0);
                    dictionary.Add("islocal", 1);
                    dictionary.Add("color", 2);
                    <>f__switch$mapA = dictionary;
                }
                if (<>f__switch$mapA.TryGetValue(name, out num))
                {
                    switch (num)
                    {
                        case 0:
                            if (obj2 == null)
                            {
                                return float.Parse(valueString);
                            }
                            return (float) obj2;

                        case 1:
                            if (obj2 == null)
                            {
                                return bool.Parse(valueString);
                            }
                            return (bool) obj2;

                        case 2:
                            return ColorUtil.ParseColor(valueString);
                    }
                }
            }
            return null;
        }

        public void ReInit()
        {
            if (this.isDynamic)
            {
                this.HashObjects.Clear();
                this.Init(this.strType, this.strArg, this.strEaseType, this.strLoopType);
            }
        }

        public void Write(BinaryWriter writer)
        {
            if (!this.IsEndlessLoop)
            {
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(Utage.ErrorMsg.TweenWrite, new object[0]));
            }
            writer.Write(this.strType);
            writer.Write(this.strArg);
            writer.Write(this.strEaseType);
            writer.Write(this.strLoopType);
        }

        public string ErrorMsg
        {
            get
            {
                return this.errorMsg;
            }
        }

        public Dictionary<string, object> HashObjects
        {
            get
            {
                return this.hashObjects;
            }
        }

        public bool IsDynamic
        {
            get
            {
                return this.isDynamic;
            }
        }

        public bool IsEndlessLoop
        {
            get
            {
                return ((this.loopType != iTween.LoopType.none) && (this.loopCount <= 0));
            }
        }

        public bool IsLocal
        {
            get
            {
                if (this.HashObjects.ContainsKey("islocal"))
                {
                    return (bool) this.HashObjects["islocal"];
                }
                if (!this.IsSupportLocal)
                {
                    Debug.LogError("Not Support Local type");
                }
                return false;
            }
            set
            {
                this.HashObjects["islocal"] = value;
            }
        }

        public bool IsSupportLocal
        {
            get
            {
                string[] strArray = ArgTbl[(int) this.Type];
                foreach (string str in strArray)
                {
                    if (str == "islocal")
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public iTween.LoopType Loop
        {
            get
            {
                return this.loopType;
            }
        }

        public int LoopCount
        {
            get
            {
                return this.loopCount;
            }
        }

        public iTweenType Type
        {
            get
            {
                return this.type;
            }
        }
    }
}

