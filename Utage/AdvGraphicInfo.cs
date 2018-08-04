namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class AdvGraphicInfo : IAssetFileSettingData
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map4;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvAnimationData <AnimationData>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ConditionalExpression>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <DataType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvEyeBlinkData <EyeBlinkData>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <FileName>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <FileType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Index>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Key>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvLipSynchData <LipSynchData>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector2 <Pivot>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3 <Position>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private StringGridRow <RowData>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3 <Scale>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IAdvSettingData <SettingData>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <SubFileName>k__BackingField;
        public static CreateCustom CallbackCreateCustom;
        public static Func<string, bool> CallbackExpression;
        private AssetFile file;
        public const string FileType2D = "2D";
        public const string FileType2DPrefab = "2DPrefab";
        public const string FileType3D = "3D";
        public const string FileType3DPrefab = "3DPrefab";
        public const string FileTypeAvatar = "Avatar";
        public const string FileTypeCustom = "Custom";
        public const string FileTypeDicing = "Dicing";
        public const string FileTypeParticle = "Particle";
        public const string FileTypeVideo = "Video";
        private AdvRenderTextureSetting renderTextureSetting;
        private const int SaveVersion = 0;
        public const string TypeCapture = "Capture";
        public const string TypeCharacter = "Character";
        public const string TypeParticle = "Particle";
        public const string TypeTexture = "Texture";
        public const string TypeVideo = "Video";

        public AdvGraphicInfo(string dataType, string key, string fileType)
        {
            this.renderTextureSetting = new AdvRenderTextureSetting();
            this.DataType = dataType;
            this.Key = key;
            this.FileType = fileType;
            this.FileName = string.Empty;
            this.Pivot = new Vector2(0.5f, 0.5f);
            this.Scale = Vector3.get_one();
            this.Position = Vector3.get_zero();
            this.ConditionalExpression = string.Empty;
            this.SubFileName = string.Empty;
        }

        public AdvGraphicInfo(string dataType, int index, string key, StringGridRow row, IAdvSettingData advSettindData)
        {
            Vector3 vector;
            this.renderTextureSetting = new AdvRenderTextureSetting();
            this.DataType = dataType;
            this.Index = index;
            this.Key = key;
            this.SettingData = advSettindData;
            this.RowData = row;
            string str = this.DataType;
            if ((str != null) && (str == "Particle"))
            {
                this.FileType = "Particle";
            }
            else
            {
                this.FileType = AdvParser.ParseCellOptional<string>(row, AdvColumnName.FileType, string.Empty);
            }
            this.FileName = AdvParser.ParseCell<string>(row, AdvColumnName.FileName);
            try
            {
                this.Pivot = ParserUtil.ParsePivotOptional(AdvParser.ParseCellOptional<string>(row, AdvColumnName.Pivot, string.Empty), new Vector2(0.5f, 0.5f));
            }
            catch (Exception exception)
            {
                Debug.LogError(row.ToErrorString(exception.Message));
            }
            try
            {
                this.Scale = ParserUtil.ParseScale3DOptional(AdvParser.ParseCellOptional<string>(row, AdvColumnName.Scale, string.Empty), Vector3.get_one());
            }
            catch (Exception exception2)
            {
                Debug.LogError(row.ToErrorString(exception2.Message));
            }
            vector.x = AdvParser.ParseCellOptional<float>(row, AdvColumnName.X, 0f);
            vector.y = AdvParser.ParseCellOptional<float>(row, AdvColumnName.Y, 0f);
            vector.z = AdvParser.ParseCellOptional<float>(row, AdvColumnName.Z, 0f);
            this.Position = vector;
            this.SubFileName = AdvParser.ParseCellOptional<string>(row, AdvColumnName.SubFileName, string.Empty);
            this.ConditionalExpression = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Conditional, string.Empty);
            this.RenderTextureSetting.Parse(row);
        }

        public void BootInit(Func<string, string, string> FileNameToPath, AdvSettingDataManager dataManager)
        {
            this.File = AssetFileManager.GetFileCreateIfMissing(FileNameToPath(this.FileName, this.FileType), this);
            string str = AdvParser.ParseCellOptional<string>(this.RowData, AdvColumnName.Animation, string.Empty);
            if (!string.IsNullOrEmpty(str))
            {
                this.AnimationData = dataManager.AnimationSetting.Find(str);
                if (this.AnimationData == null)
                {
                    Debug.LogError(this.RowData.ToErrorString("Animation [ " + str + " ] is not found"));
                }
            }
            string str2 = AdvParser.ParseCellOptional<string>(this.RowData, AdvColumnName.EyeBlink, string.Empty);
            if (!string.IsNullOrEmpty(str2))
            {
                AdvEyeBlinkData data;
                if (dataManager.EyeBlinkSetting.Dictionary.TryGetValue(str2, out data))
                {
                    this.EyeBlinkData = data;
                }
                else
                {
                    Debug.LogError(this.RowData.ToErrorString("EyeBlinkLabel [ " + str2 + " ] is not found"));
                }
            }
            string str3 = AdvParser.ParseCellOptional<string>(this.RowData, AdvColumnName.LipSynch, string.Empty);
            if (!string.IsNullOrEmpty(str3))
            {
                AdvLipSynchData data2;
                if (dataManager.LipSynchSetting.Dictionary.TryGetValue(str3, out data2))
                {
                    this.LipSynchData = data2;
                }
                else
                {
                    Debug.LogError(this.RowData.ToErrorString("LipSynchLabel [ " + str3 + " ] is not found"));
                }
            }
        }

        internal Type GetComponentType()
        {
            if (CallbackCreateCustom != null)
            {
                Type type = null;
                CallbackCreateCustom(this.FileType, ref type);
                if (type != null)
                {
                    return type;
                }
            }
            string fileType = this.FileType;
            if (fileType != null)
            {
                int num;
                if (<>f__switch$map4 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(9);
                    dictionary.Add("3D", 0);
                    dictionary.Add("3DPrefab", 0);
                    dictionary.Add("Particle", 1);
                    dictionary.Add("2DPrefab", 2);
                    dictionary.Add("Custom", 3);
                    dictionary.Add("Avatar", 4);
                    dictionary.Add("Dicing", 5);
                    dictionary.Add("Video", 6);
                    dictionary.Add("2D", 7);
                    <>f__switch$map4 = dictionary;
                }
                if (<>f__switch$map4.TryGetValue(fileType, out num))
                {
                    switch (num)
                    {
                        case 0:
                            return typeof(AdvGraphicObject3DPrefab);

                        case 1:
                            return typeof(AdvGraphicObjectParticle);

                        case 2:
                            return typeof(AdvGraphicObject2DPrefab);

                        case 3:
                            return typeof(AdvGraphicObjectCustom);

                        case 4:
                            return typeof(AdvGraphicObjectAvatar);

                        case 5:
                            return typeof(AdvGraphicObjectDicing);

                        case 6:
                            return typeof(AdvGraphicObjectVideo);
                    }
                }
            }
            return typeof(AdvGraphicObjectRawImage);
        }

        public void OnWrite(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.DataType);
            writer.Write(this.Key);
            writer.Write(this.Index);
        }

        public static AdvGraphicInfo ReadGraphicInfo(AdvEngine engine, BinaryReader reader)
        {
            graphic graphic;
            int num = reader.ReadInt32();
            if ((num < 0) || (num > 0))
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
                return null;
            }
            string dataType = reader.ReadString();
            string label = reader.ReadString();
            int num2 = reader.ReadInt32();
            if (dataType != null)
            {
                if (!(dataType == "Character"))
                {
                    if (dataType == "Particle")
                    {
                        return engine.DataManager.SettingDataManager.ParticleSetting.LabelToGraphic(label);
                    }
                    if (dataType == "Texture")
                    {
                        graphic = engine.DataManager.SettingDataManager.TextureSetting.LabelToGraphic(label);
                        goto Label_00FB;
                    }
                    if (dataType == "Capture")
                    {
                        Debug.LogError("Caputure image not support on save");
                        return null;
                    }
                }
                else
                {
                    graphic = engine.DataManager.SettingDataManager.CharacterSetting.KeyToGraphicInfo(label);
                    goto Label_00FB;
                }
            }
            return new AdvGraphicInfo(dataType, label, "2D");
        Label_00FB:
            if ((graphic != null) && (num2 < graphic.InfoList.Count))
            {
                return graphic.InfoList[num2];
            }
            return null;
        }

        internal bool TryGetAdvGraphicObjectPrefab(out GameObject prefab)
        {
            prefab = null;
            if (this.File == null)
            {
                return false;
            }
            if (this.File.FileType != AssetFileType.UnityObject)
            {
                return false;
            }
            GameObject unityObject = this.File.UnityObject as GameObject;
            if (unityObject == null)
            {
                return false;
            }
            if (unityObject.GetComponent<AdvGraphicObject>() == null)
            {
                return false;
            }
            prefab = unityObject;
            return true;
        }

        public AdvAnimationData AnimationData { get; private set; }

        public bool CheckConditionalExpression
        {
            get
            {
                if (CallbackExpression == null)
                {
                    Debug.LogError("GraphicInfo CallbackExpression is nul");
                    return false;
                }
                return CallbackExpression(this.ConditionalExpression);
            }
        }

        public string ConditionalExpression { get; private set; }

        public string DataType { get; protected set; }

        public AdvEyeBlinkData EyeBlinkData { get; set; }

        public AssetFile File
        {
            get
            {
                return this.file;
            }
            set
            {
                this.file = value;
            }
        }

        public string FileName { get; protected set; }

        public string FileType { get; protected set; }

        private int Index { get; set; }

        internal bool IsUguiComponentType
        {
            get
            {
                return this.GetComponentType().IsSubclassOf(typeof(AdvGraphicObjectUguiBase));
            }
        }

        public string Key { get; protected set; }

        public AdvLipSynchData LipSynchData { get; private set; }

        public Vector2 Pivot { get; private set; }

        public Vector3 Position { get; private set; }

        public AdvRenderTextureSetting RenderTextureSetting
        {
            get
            {
                return this.renderTextureSetting;
            }
        }

        public StringGridRow RowData { get; protected set; }

        public Vector3 Scale { get; private set; }

        public IAdvSettingData SettingData { get; protected set; }

        public string SubFileName { get; private set; }

        public delegate void CreateCustom(string fileType, ref Type type);
    }
}

