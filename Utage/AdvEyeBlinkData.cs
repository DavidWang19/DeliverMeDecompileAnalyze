namespace Utage
{
    using System;
    using UnityEngine;

    public class AdvEyeBlinkData : AdvSettingDictinoayItemBase
    {
        [SerializeField]
        private MiniAnimationData animationData = new MiniAnimationData();
        [SerializeField]
        private float intervalMax = 6f;
        [SerializeField]
        private float intervalMin = 2f;
        [SerializeField]
        private float randomDoubleEyeBlink = 0.2f;
        [SerializeField]
        private string tag = "eye";

        public override bool InitFromStringGridRow(StringGridRow row)
        {
            int num;
            string key = AdvCommandParser.ParseScenarioLabel(row, AdvColumnName.Label);
            base.InitKey(key);
            this.IntervalMin = AdvParser.ParseCellOptional<float>(row, AdvColumnName.IntervalMin, 2f);
            this.IntervalMax = AdvParser.ParseCellOptional<float>(row, AdvColumnName.IntervalMax, 6f);
            this.RandomDoubleEyeBlink = AdvParser.ParseCellOptional<float>(row, AdvColumnName.RandomDouble, 0.2f);
            this.Tag = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Tag, "eye");
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

        public float IntervalMax
        {
            get
            {
                return this.intervalMax;
            }
            set
            {
                this.intervalMax = value;
            }
        }

        public float IntervalMin
        {
            get
            {
                return this.intervalMin;
            }
            set
            {
                this.intervalMin = value;
            }
        }

        public float RandomDoubleEyeBlink
        {
            get
            {
                return this.randomDoubleEyeBlink;
            }
            set
            {
                this.randomDoubleEyeBlink = value;
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
    }
}

