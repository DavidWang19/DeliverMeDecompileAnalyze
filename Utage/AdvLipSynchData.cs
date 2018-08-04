namespace Utage
{
    using System;
    using UnityEngine;

    public class AdvLipSynchData : AdvSettingDictinoayItemBase
    {
        [SerializeField]
        private MiniAnimationData animationData = new MiniAnimationData();
        [SerializeField]
        private float interval = 0.2f;
        [SerializeField]
        private float scaleVoiceVolume = 1f;
        [SerializeField]
        private string tag = "eye";
        [SerializeField]
        private LipSynchType type = LipSynchType.TextAndVoice;

        public override bool InitFromStringGridRow(StringGridRow row)
        {
            int num;
            string key = AdvCommandParser.ParseScenarioLabel(row, AdvColumnName.Label);
            base.InitKey(key);
            this.Type = AdvParser.ParseCellOptional<LipSynchType>(row, AdvColumnName.Type, LipSynchType.TextAndVoice);
            this.Interval = AdvParser.ParseCellOptional<float>(row, AdvColumnName.Interval, 0.2f);
            this.ScaleVoiceVolume = (float) AdvParser.ParseCellOptional<int>(row, AdvColumnName.ScaleVoiceVolume, 1);
            this.Tag = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Tag, "lip");
            if (row.Grid.TryGetColumnIndex(AdvColumnName.Name0.QuickToString(), out num))
            {
                this.animationData.TryParse(row, num);
            }
            return true;
        }

        public MiniAnimationData AnimationData
        {
            get
            {
                return this.animationData;
            }
        }

        public float Interval
        {
            get
            {
                return this.interval;
            }
            set
            {
                this.interval = value;
            }
        }

        public float ScaleVoiceVolume
        {
            get
            {
                return this.scaleVoiceVolume;
            }
            set
            {
                this.scaleVoiceVolume = value;
            }
        }

        public string Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                this.tag = value;
            }
        }

        public LipSynchType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }
}

