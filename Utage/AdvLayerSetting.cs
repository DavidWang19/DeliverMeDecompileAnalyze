namespace Utage
{
    using System;
    using System.Runtime.CompilerServices;

    public class AdvLayerSetting : AdvSettingDataDictinoayBase<AdvLayerSettingData>
    {
        public bool Contains(string layerName)
        {
            return base.Dictionary.ContainsKey(layerName);
        }

        public bool Contains(string layerName, AdvLayerSettingData.LayerType type)
        {
            AdvLayerSettingData data;
            return (base.Dictionary.TryGetValue(layerName, out data) && (data.Type == type));
        }

        public AdvLayerSettingData FindDefaultLayer(AdvLayerSettingData.LayerType type)
        {
            <FindDefaultLayer>c__AnonStorey1 storey = new <FindDefaultLayer>c__AnonStorey1 {
                type = type
            };
            return base.List.Find(new Predicate<AdvLayerSettingData>(storey.<>m__0));
        }

        private void InitDefault(AdvLayerSettingData.LayerType type, int defaultOrder)
        {
            <InitDefault>c__AnonStorey0 storey = new <InitDefault>c__AnonStorey0 {
                type = type
            };
            AdvLayerSettingData data = base.List.Find(new Predicate<AdvLayerSettingData>(storey.<>m__0));
            if (data == null)
            {
                data = new AdvLayerSettingData();
                data.InitDefault(storey.type.ToString() + " Default", storey.type, defaultOrder);
                base.AddData(data);
            }
            data.IsDefault = true;
        }

        public override void ParseGrid(StringGrid grid)
        {
            base.ParseGrid(grid);
            this.InitDefault(AdvLayerSettingData.LayerType.Bg, 0);
            this.InitDefault(AdvLayerSettingData.LayerType.Character, 100);
            this.InitDefault(AdvLayerSettingData.LayerType.Sprite, 200);
        }

        [CompilerGenerated]
        private sealed class <FindDefaultLayer>c__AnonStorey1
        {
            internal AdvLayerSettingData.LayerType type;

            internal bool <>m__0(AdvLayerSettingData item)
            {
                return ((item.Type == this.type) && item.IsDefault);
            }
        }

        [CompilerGenerated]
        private sealed class <InitDefault>c__AnonStorey0
        {
            internal AdvLayerSettingData.LayerType type;

            internal bool <>m__0(AdvLayerSettingData item)
            {
                return (item.Type == this.type);
            }
        }
    }
}

