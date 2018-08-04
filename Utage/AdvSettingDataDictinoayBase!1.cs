namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class AdvSettingDataDictinoayBase<T> : AdvSettingBase where T: AdvSettingDictinoayItemBase, new()
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, T> <Dictionary>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<T> <List>k__BackingField;

        public AdvSettingDataDictinoayBase()
        {
            this.Dictionary = new Dictionary<string, T>();
            this.List = new List<T>();
        }

        protected void AddData(T data)
        {
            this.List.Add(data);
            this.Dictionary.Add(data.Key, data);
        }

        protected override void OnParseGrid(StringGrid grid)
        {
            T last = null;
            foreach (StringGridRow row in grid.Rows)
            {
                if (((row.RowIndex >= grid.DataTopRow) && !row.IsEmptyOrCommantOut) && !this.TryParseContinues(last, row))
                {
                    T local2 = this.ParseFromStringGridRow(last, row);
                    if (local2 != null)
                    {
                        last = local2;
                    }
                }
            }
        }

        protected virtual T ParseFromStringGridRow(T last, StringGridRow row)
        {
            T data = Activator.CreateInstance<T>();
            if (data.InitFromStringGridRowMain(row))
            {
                if (!this.Dictionary.ContainsKey(data.Key))
                {
                    this.AddData(data);
                    return data;
                }
                Debug.LogError(string.Empty + row.ToErrorString(ColorUtil.AddColorTag(data.Key, Color.get_red()) + "  is already contains"));
            }
            return null;
        }

        protected virtual bool TryParseContinues(T last, StringGridRow row)
        {
            return ((last == null) && false);
        }

        public Dictionary<string, T> Dictionary { get; private set; }

        public List<T> List { get; private set; }
    }
}

