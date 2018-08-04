namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AdvSelection
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Label>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <PrefabName>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float? <X>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float? <Y>k__BackingField;
        private ExpressionParser exp;
        private bool isPolygon;
        private string jumpLabel;
        private StringGridRow row;
        private string spriteName;
        private string text;
        private const int VERSION = 2;
        private const int VERSION_0 = 0;
        private const int VERSION_1 = 1;

        public AdvSelection(BinaryReader reader, AdvEngine engine)
        {
            this.spriteName = string.Empty;
            this.Read(reader, engine);
        }

        public AdvSelection(string jumpLabel, string spriteName, bool isPolygon, ExpressionParser exp, StringGridRow row)
        {
            this.spriteName = string.Empty;
            this.Label = string.Empty;
            this.jumpLabel = jumpLabel;
            this.text = string.Empty;
            this.exp = exp;
            this.spriteName = spriteName;
            this.isPolygon = isPolygon;
            this.row = row;
        }

        public AdvSelection(string jumpLabel, string text, ExpressionParser exp, string prefabName, float? x, float? y, StringGridRow row)
        {
            this.spriteName = string.Empty;
            this.Label = string.Empty;
            this.jumpLabel = jumpLabel;
            this.text = text;
            this.exp = exp;
            this.PrefabName = prefabName;
            this.X = x;
            this.Y = y;
            this.row = row;
        }

        private void Read(BinaryReader reader, AdvEngine engine)
        {
            int num = reader.ReadInt32();
            if (num == 2)
            {
                this.jumpLabel = reader.ReadString();
                this.text = reader.ReadString();
                string str = reader.ReadString();
                if (!string.IsNullOrEmpty(str))
                {
                    this.exp = engine.DataManager.SettingDataManager.DefaultParam.CreateExpression(str);
                }
                else
                {
                    this.exp = null;
                }
                this.spriteName = reader.ReadString();
                this.isPolygon = reader.ReadBoolean();
            }
            else if (num == 1)
            {
                this.jumpLabel = reader.ReadString();
                this.text = reader.ReadString();
                string str2 = reader.ReadString();
                if (!string.IsNullOrEmpty(str2))
                {
                    this.exp = engine.DataManager.SettingDataManager.DefaultParam.CreateExpression(str2);
                }
                else
                {
                    this.exp = null;
                }
            }
            else if (num == 0)
            {
                this.jumpLabel = reader.ReadString();
                this.text = reader.ReadString();
            }
            else
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(2);
            writer.Write(this.jumpLabel);
            writer.Write(this.text);
            if (this.Exp != null)
            {
                writer.Write(this.Exp.Exp);
            }
            else
            {
                writer.Write(string.Empty);
            }
            writer.Write(this.spriteName);
            writer.Write(this.isPolygon);
        }

        public ExpressionParser Exp
        {
            get
            {
                return this.exp;
            }
        }

        public bool IsPolygon
        {
            get
            {
                return this.isPolygon;
            }
        }

        public string JumpLabel
        {
            get
            {
                return this.jumpLabel;
            }
        }

        public string Label { get; private set; }

        public string PrefabName { get; protected set; }

        public StringGridRow RowData
        {
            get
            {
                return this.row;
            }
        }

        public string SpriteName
        {
            get
            {
                return this.spriteName;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
        }

        public float? X { get; protected set; }

        public float? Y { get; protected set; }
    }
}

