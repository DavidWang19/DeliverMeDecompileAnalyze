namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using UnityEngine;
    using UtageExtensions;

    [Serializable]
    public class AdvParamManager : AdvSettingBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvParamManager <DefaultParameter>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <HasChangedSystemParam>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsInit>k__BackingField;
        private IoInerface defaultData;
        public const string DefaultSheetName = "Param";
        private const string KeyPattern = @"(.+)\[(.+)\]\.(.+)";
        private static readonly Regex KeyRegix = new Regex(@"(.+)\[(.+)\]\.(.+)", RegexOptions.IgnorePatternWhitespace);
        private Dictionary<string, AdvParamStructTbl> structTbl = new Dictionary<string, AdvParamStructTbl>();
        private IoInerface systemData;
        private const int Version = 0;

        public object CalcExpression(ExpressionParser exp)
        {
            return exp.CalcExp(new Func<string, object>(this.GetParameter), new Func<string, object, bool>(this.TrySetParameter));
        }

        public bool CalcExpressionBoolean(string exp)
        {
            return this.CalcExpressionBoolean(this.CreateExpressionBoolean(exp));
        }

        public bool CalcExpressionBoolean(ExpressionParser exp)
        {
            bool flag = exp.CalcExpBoolean(new Func<string, object>(this.GetParameter), new Func<string, object, bool>(this.TrySetParameter));
            if (!string.IsNullOrEmpty(exp.ErrorMsg))
            {
                Debug.LogError(exp.ErrorMsg);
            }
            return flag;
        }

        public float CalcExpressionFloat(ExpressionParser exp)
        {
            object obj2 = exp.CalcExp(new Func<string, object>(this.GetParameter), new Func<string, object, bool>(this.TrySetParameter));
            if (obj2.GetType() == typeof(int))
            {
                return (float) ((int) obj2);
            }
            if (obj2.GetType() == typeof(float))
            {
                return (float) obj2;
            }
            Debug.LogError("Float Cast error : " + exp.Exp);
            return 0f;
        }

        public int CalcExpressionInt(ExpressionParser exp)
        {
            object obj2 = exp.CalcExp(new Func<string, object>(this.GetParameter), new Func<string, object, bool>(this.TrySetParameter));
            if (obj2.GetType() == typeof(int))
            {
                return (int) obj2;
            }
            if (obj2.GetType() == typeof(float))
            {
                return (int) ((float) obj2);
            }
            Debug.LogError("Int Cast error : " + exp.Exp);
            return 0;
        }

        public object CalcExpressionNotSetParam(string exp)
        {
            ExpressionParser parser = this.CreateExpression(exp);
            if (!string.IsNullOrEmpty(parser.ErrorMsg))
            {
                throw new Exception(parser.ErrorMsg);
            }
            return parser.CalcExp(new Func<string, object>(this.GetParameter), new Func<string, object, bool>(this.CheckSetParameter));
        }

        public bool CheckSetParameter(string key, object parameter)
        {
            AdvParamData data;
            return this.CheckSetParameterSub(key, parameter, out data);
        }

        private bool CheckSetParameterSub(string key, object parameter, out AdvParamData data)
        {
            if (!this.TryGetParamData(key, out data))
            {
                return false;
            }
            if (data.SaveFileType == AdvParamData.FileType.Const)
            {
                return false;
            }
            if (((data.Type == AdvParamData.ParamType.Bool) || (parameter is bool)) && ((data.Type != AdvParamData.ParamType.Bool) || !(parameter is bool)))
            {
                return false;
            }
            if ((parameter is string) && (data.Type != AdvParamData.ParamType.String))
            {
                return false;
            }
            if ((data.Type == AdvParamData.ParamType.String) && (parameter is bool))
            {
                return false;
            }
            return true;
        }

        public ExpressionParser CreateExpression(string exp)
        {
            return new ExpressionParser(exp, new Func<string, object>(this.GetParameter), new Func<string, object, bool>(this.CheckSetParameter));
        }

        public ExpressionParser CreateExpressionBoolean(string exp)
        {
            return new ExpressionParser(exp, new Func<string, object>(this.GetParameter), new Func<string, object, bool>(this.CheckSetParameter), true);
        }

        public AdvParamStruct GetDefault()
        {
            if (!this.StructTbl.ContainsKey("Param"))
            {
                return null;
            }
            return this.StructTbl["Param"].Tbl[string.Empty];
        }

        public object GetParameter(string key)
        {
            object obj2;
            if (this.TryGetParameter(key, out obj2))
            {
                return obj2;
            }
            return null;
        }

        public T GetParameter<T>(string key)
        {
            return (T) this.GetParameter(key);
        }

        public bool GetParameterBoolean(string key)
        {
            return this.GetParameter<bool>(key);
        }

        public float GetParameterFloat(string key)
        {
            return this.GetParameter<float>(key);
        }

        public int GetParameterInt(string key)
        {
            return this.GetParameter<int>(key);
        }

        public string GetParameterString(string key)
        {
            return this.GetParameter<string>(key);
        }

        internal void InitDefaultAll(AdvParamManager src)
        {
            this.DefaultParameter = src;
            this.StructTbl.Clear();
            foreach (KeyValuePair<string, AdvParamStructTbl> pair in src.StructTbl)
            {
                this.StructTbl.Add(pair.Key, pair.Value.Clone());
            }
            this.IsInit = true;
        }

        internal void InitDefaultNormal(AdvParamManager src)
        {
            foreach (KeyValuePair<string, AdvParamStructTbl> pair in src.StructTbl)
            {
                AdvParamStructTbl tbl;
                if (this.StructTbl.TryGetValue(pair.Key, out tbl))
                {
                    tbl.InitDefaultNormal(pair.Value);
                }
                else
                {
                    Debug.LogError("Param: " + pair.Key + "  is not found in default param");
                }
            }
        }

        protected override void OnParseGrid(StringGrid grid)
        {
            if (base.GridList.Count == 0)
            {
                Debug.LogError("Old Version Reimport Excel Scenario Data");
            }
            else
            {
                AdvParamStructTbl tbl;
                string sheetName = grid.SheetName;
                if (!this.StructTbl.TryGetValue(sheetName, out tbl))
                {
                    tbl = new AdvParamStructTbl();
                    this.StructTbl.Add(sheetName, tbl);
                }
                if (sheetName == "Param")
                {
                    tbl.AddSingle(grid);
                }
                else
                {
                    tbl.AddTbl(grid);
                }
            }
        }

        internal static bool ParseKey(string key, out string structName, out string indexKey, out string valueKey)
        {
            string str;
            valueKey = str = string.Empty;
            structName = indexKey = str;
            if (!key.Contains("["))
            {
                return false;
            }
            Match match = KeyRegix.Match(key);
            if (!match.Success)
            {
                return false;
            }
            structName = match.Groups[1].Value + "{}";
            indexKey = match.Groups[2].Value;
            valueKey = match.Groups[3].Value;
            return true;
        }

        public void Read(BinaryReader reader, AdvParamData.FileType fileType)
        {
            <Read>c__AnonStorey3 storey = new <Read>c__AnonStorey3 {
                fileType = fileType,
                $this = this
            };
            if (storey.fileType == AdvParamData.FileType.Default)
            {
                this.InitDefaultNormal(this.DefaultParameter);
            }
            int num = reader.ReadInt32();
            if ((num < 0) || (num > 0))
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
            else
            {
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    <Read>c__AnonStorey2 storey2 = new <Read>c__AnonStorey2 {
                        <>f__ref$3 = storey,
                        key = reader.ReadString()
                    };
                    if (this.StructTbl.ContainsKey(storey2.key))
                    {
                        reader.ReadBuffer(new Action<BinaryReader>(storey2.<>m__0));
                    }
                    else
                    {
                        reader.SkipBuffer();
                    }
                }
            }
        }

        public void SetParameter(string key, object parameter)
        {
            if (!this.TrySetParameter(key, parameter))
            {
                Debug.LogError(key + " is not parameter name");
            }
        }

        public void SetParameter<T>(string key, T value)
        {
            this.SetParameter(key, value);
        }

        public void SetParameterBoolean(string key, bool value)
        {
            this.SetParameter<bool>(key, value);
        }

        public void SetParameterFloat(string key, float value)
        {
            this.SetParameter<float>(key, value);
        }

        public void SetParameterInt(string key, int value)
        {
            this.SetParameter<int>(key, value);
        }

        public void SetParameterString(string key, string value)
        {
            this.SetParameter<string>(key, value);
        }

        private bool TryGetParamData(string key, out AdvParamData data)
        {
            string str;
            string str2;
            string str3;
            AdvParamStruct struct3;
            data = null;
            if (!ParseKey(key, out str, out str2, out str3))
            {
                AdvParamStruct struct2 = this.GetDefault();
                if (struct2 == null)
                {
                    return false;
                }
                return struct2.Tbl.TryGetValue(key, out data);
            }
            if (!this.TryGetParamTbl(str, str2, out struct3))
            {
                return false;
            }
            return struct3.Tbl.TryGetValue(str3, out data);
        }

        public bool TryGetParameter(string key, out object parameter)
        {
            AdvParamData data;
            if (this.TryGetParamData(key, out data))
            {
                parameter = data.Parameter;
                return true;
            }
            parameter = null;
            return false;
        }

        public bool TryGetParamTbl(string structName, string indexKey, out AdvParamStruct paramStruct)
        {
            paramStruct = null;
            if (!this.StructTbl.ContainsKey(structName))
            {
                return false;
            }
            if (!this.StructTbl[structName].Tbl.ContainsKey(indexKey))
            {
                return false;
            }
            paramStruct = this.StructTbl[structName].Tbl[indexKey];
            return true;
        }

        public bool TrySetParameter(string key, object parameter)
        {
            AdvParamData data;
            if (!this.CheckSetParameterSub(key, parameter, out data))
            {
                return false;
            }
            data.Parameter = parameter;
            if (data.SaveFileType == AdvParamData.FileType.System)
            {
                this.HasChangedSystemParam = true;
            }
            return true;
        }

        public void Write(BinaryWriter writer, AdvParamData.FileType fileType)
        {
            <Write>c__AnonStorey1 storey = new <Write>c__AnonStorey1 {
                fileType = fileType
            };
            writer.Write(0);
            writer.Write(this.StructTbl.Count);
            using (Dictionary<string, AdvParamStructTbl>.Enumerator enumerator = this.StructTbl.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    <Write>c__AnonStorey0 storey2 = new <Write>c__AnonStorey0 {
                        <>f__ref$1 = storey,
                        keyValue = enumerator.Current
                    };
                    writer.Write(storey2.keyValue.Key);
                    writer.WriteBuffer(new Action<BinaryWriter>(storey2.<>m__0));
                }
            }
        }

        public IoInerface DefaultData
        {
            get
            {
                if (this.defaultData == null)
                {
                    this.defaultData = new IoInerface(this, AdvParamData.FileType.Default);
                }
                return this.defaultData;
            }
        }

        public AdvParamManager DefaultParameter { get; set; }

        public bool HasChangedSystemParam { get; set; }

        public bool IsInit { get; protected set; }

        public Dictionary<string, AdvParamStructTbl> StructTbl
        {
            get
            {
                return this.structTbl;
            }
        }

        public IoInerface SystemData
        {
            get
            {
                if (this.systemData == null)
                {
                    this.systemData = new IoInerface(this, AdvParamData.FileType.System);
                }
                return this.systemData;
            }
        }

        [CompilerGenerated]
        private sealed class <Read>c__AnonStorey2
        {
            internal AdvParamManager.<Read>c__AnonStorey3 <>f__ref$3;
            internal string key;

            internal void <>m__0(BinaryReader x)
            {
                this.<>f__ref$3.$this.StructTbl[this.key].Read(x, this.<>f__ref$3.fileType);
            }
        }

        [CompilerGenerated]
        private sealed class <Read>c__AnonStorey3
        {
            internal AdvParamManager $this;
            internal AdvParamData.FileType fileType;
        }

        [CompilerGenerated]
        private sealed class <Write>c__AnonStorey0
        {
            internal AdvParamManager.<Write>c__AnonStorey1 <>f__ref$1;
            internal KeyValuePair<string, AdvParamStructTbl> keyValue;

            internal void <>m__0(BinaryWriter x)
            {
                this.keyValue.Value.Write(x, this.<>f__ref$1.fileType);
            }
        }

        [CompilerGenerated]
        private sealed class <Write>c__AnonStorey1
        {
            internal AdvParamData.FileType fileType;
        }

        public class IoInerface : IBinaryIO
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Utage.AdvParamData.FileType <FileType>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private AdvParamManager <Param>k__BackingField;

            public IoInerface(AdvParamManager param, Utage.AdvParamData.FileType fileType)
            {
                this.Param = param;
                this.FileType = fileType;
            }

            public void OnRead(BinaryReader reader)
            {
                this.Param.Read(reader, this.FileType);
            }

            public void OnWrite(BinaryWriter writer)
            {
                this.Param.Write(writer, this.FileType);
            }

            private Utage.AdvParamData.FileType FileType { get; set; }

            private AdvParamManager Param { get; set; }

            public string SaveKey
            {
                get
                {
                    return "ParamManagerIoInerface";
                }
            }
        }
    }
}

