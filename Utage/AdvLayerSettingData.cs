namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class AdvLayerSettingData : AdvSettingDictinoayItemBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Utage.Alignment <Alignment>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <FlipX>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <FlipY>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RectSetting <Horizontal>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <LayerMask>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Order>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector2 <Pivot>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3 <Scale>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private LayerType <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RectSetting <Vertical>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Z>k__BackingField;
        private bool isDefault;

        public void InitDefault(string name, LayerType type, int order)
        {
            base.InitKey(name);
            this.Type = type;
            this.Horizontal = new RectSetting();
            this.Vertical = new RectSetting();
            this.Pivot = (Vector2) (Vector2.get_one() * 0.5f);
            this.Order = order;
            this.Scale = Vector2.get_one();
            this.Z = -0.01f * order;
            this.LayerMask = string.Empty;
            this.Alignment = Utage.Alignment.None;
            this.FlipX = false;
            this.FlipY = false;
        }

        public override bool InitFromStringGridRow(StringGridRow row)
        {
            Vector2 vector;
            Vector3 vector2;
            base.RowData = row;
            string str = AdvParser.ParseCell<string>(row, AdvColumnName.LayerName);
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            base.InitKey(str);
            this.Type = AdvParser.ParseCell<LayerType>(row, AdvColumnName.Type);
            this.Order = AdvParser.ParseCell<int>(row, AdvColumnName.Order);
            this.LayerMask = AdvParser.ParseCellOptional<string>(row, AdvColumnName.LayerMask, string.Empty);
            this.Horizontal = new RectSetting();
            bool flag = !AdvParser.IsEmptyCell(row, AdvColumnName.BorderLeft);
            bool flag2 = !AdvParser.IsEmptyCell(row, AdvColumnName.BorderRight);
            if (flag)
            {
                this.Horizontal.type = !flag2 ? BorderType.BorderMin : BorderType.Streach;
            }
            else
            {
                this.Horizontal.type = !flag2 ? BorderType.None : BorderType.BorderMax;
            }
            this.Horizontal.position = AdvParser.ParseCellOptional<float>(row, AdvColumnName.X, 0f);
            this.Horizontal.size = AdvParser.ParseCellOptional<float>(row, AdvColumnName.Width, 0f);
            this.Horizontal.borderMin = AdvParser.ParseCellOptional<float>(row, AdvColumnName.BorderLeft, 0f);
            this.Horizontal.borderMax = AdvParser.ParseCellOptional<float>(row, AdvColumnName.BorderRight, 0f);
            this.Vertical = new RectSetting();
            bool flag3 = !AdvParser.IsEmptyCell(row, AdvColumnName.BorderTop);
            bool flag4 = !AdvParser.IsEmptyCell(row, AdvColumnName.BorderBottom);
            if (flag3)
            {
                this.Vertical.type = !flag4 ? BorderType.BorderMax : BorderType.Streach;
            }
            else
            {
                this.Vertical.type = !flag4 ? BorderType.None : BorderType.BorderMin;
            }
            this.Vertical.position = AdvParser.ParseCellOptional<float>(row, AdvColumnName.Y, 0f);
            this.Vertical.size = AdvParser.ParseCellOptional<float>(row, AdvColumnName.Height, 0f);
            this.Vertical.borderMin = AdvParser.ParseCellOptional<float>(row, AdvColumnName.BorderBottom, 0f);
            this.Vertical.borderMax = AdvParser.ParseCellOptional<float>(row, AdvColumnName.BorderTop, 0f);
            vector.x = AdvParser.ParseCellOptional<float>(row, AdvColumnName.PivotX, 0.5f);
            vector.y = AdvParser.ParseCellOptional<float>(row, AdvColumnName.PivotY, 0.5f);
            this.Pivot = vector;
            vector2.x = AdvParser.ParseCellOptional<float>(row, AdvColumnName.ScaleX, 1f);
            vector2.y = AdvParser.ParseCellOptional<float>(row, AdvColumnName.ScaleY, 1f);
            vector2.z = AdvParser.ParseCellOptional<float>(row, AdvColumnName.ScaleZ, 1f);
            this.Scale = vector2;
            this.Z = AdvParser.ParseCellOptional<float>(row, AdvColumnName.Z, -0.01f * this.Order);
            this.Alignment = AdvParser.ParseCellOptional<Utage.Alignment>(row, AdvColumnName.Align, Utage.Alignment.None);
            this.FlipX = AdvParser.ParseCellOptional<bool>(row, AdvColumnName.FlipX, false);
            this.FlipY = AdvParser.ParseCellOptional<bool>(row, AdvColumnName.FlipY, false);
            return true;
        }

        public Utage.Alignment Alignment { get; private set; }

        public bool FlipX { get; private set; }

        public bool FlipY { get; private set; }

        internal RectSetting Horizontal { get; private set; }

        public bool IsDefault
        {
            get
            {
                return this.isDefault;
            }
            set
            {
                this.isDefault = value;
            }
        }

        public string LayerMask { get; private set; }

        public string Name
        {
            get
            {
                return base.Key;
            }
        }

        public int Order { get; private set; }

        public Vector2 Pivot { get; private set; }

        public Vector3 Scale { get; private set; }

        public LayerType Type { get; private set; }

        internal RectSetting Vertical { get; private set; }

        public float Z { get; private set; }

        internal enum BorderType
        {
            None,
            Streach,
            BorderMin,
            BorderMax
        }

        public enum LayerType
        {
            Bg,
            Character,
            Sprite,
            Max
        }

        internal class RectSetting
        {
            public float borderMax;
            public float borderMin;
            public float position;
            public float size;
            public AdvLayerSettingData.BorderType type;

            internal void GetBorderdPositionAndSize(float defaultSize, out float position, out float size)
            {
                switch (this.type)
                {
                    case AdvLayerSettingData.BorderType.Streach:
                        size = defaultSize;
                        size -= this.borderMin + this.borderMax;
                        position = this.borderMin - this.borderMax;
                        return;

                    case AdvLayerSettingData.BorderType.BorderMin:
                        size = (this.size != 0f) ? this.size : defaultSize;
                        position = ((-defaultSize / 2f) + this.borderMin) + (size / 2f);
                        return;

                    case AdvLayerSettingData.BorderType.BorderMax:
                        size = (this.size != 0f) ? this.size : defaultSize;
                        position = ((defaultSize / 2f) - this.borderMax) - (size / 2f);
                        return;
                }
                size = (this.size != 0f) ? this.size : defaultSize;
                position = this.position;
            }
        }
    }
}

