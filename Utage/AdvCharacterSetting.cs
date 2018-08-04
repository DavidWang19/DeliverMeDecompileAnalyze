namespace Utage
{
    using System;
    using UnityEngine;

    public class AdvCharacterSetting : AdvSettingDataDictinoayBase<AdvCharacterSettingData>
    {
        private DictionaryString defaultKey = new DictionaryString();

        public override void BootInit(AdvSettingDataManager dataManager)
        {
            foreach (AdvCharacterSettingData data in base.List)
            {
                data.BootInit(dataManager);
            }
        }

        public bool Contains(string name)
        {
            return this.defaultKey.ContainsKey(name);
        }

        public override void DownloadAll()
        {
            foreach (AdvCharacterSettingData data in base.List)
            {
                data.Graphic.DownloadAll();
            }
        }

        private AdvCharacterSettingData FindData(string key)
        {
            AdvCharacterSettingData data;
            if (!base.Dictionary.TryGetValue(key, out data))
            {
                return null;
            }
            return data;
        }

        internal AdvCharacterSettingData GetCharacterData(string characterLabel, string patternLabel)
        {
            if (string.IsNullOrEmpty(patternLabel))
            {
                return this.FindData(this.defaultKey.Get(characterLabel));
            }
            AdvCharacterSettingData data = this.FindData(ToDataKey(characterLabel, patternLabel));
            if (data == null)
            {
                data = this.FindData(this.defaultKey.Get(characterLabel));
                if ((data != null) && data.Graphic.IsDefaultFileType)
                {
                    return null;
                }
            }
            return data;
        }

        public graphic KeyToGraphicInfo(string key)
        {
            AdvCharacterSettingData data = this.FindData(key);
            if (data == null)
            {
                Debug.LogError("Not contains " + key + " in Character sheet");
                return null;
            }
            return data.Graphic;
        }

        protected override AdvCharacterSettingData ParseFromStringGridRow(AdvCharacterSettingData last, StringGridRow row)
        {
            string name = AdvParser.ParseCellOptional<string>(row, AdvColumnName.CharacterName, string.Empty);
            string pattern = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Pattern, string.Empty);
            string nameText = AdvParser.ParseCellOptional<string>(row, AdvColumnName.NameText, string.Empty);
            if (string.IsNullOrEmpty(name))
            {
                if (last == null)
                {
                    Debug.LogError(row.ToErrorString("Not Found Chacter Name"));
                    return null;
                }
                name = last.Name;
            }
            if (string.IsNullOrEmpty(nameText))
            {
                if ((last != null) && (name == last.Name))
                {
                    nameText = last.NameText;
                }
                else
                {
                    nameText = name;
                }
            }
            AdvCharacterSettingData data = new AdvCharacterSettingData();
            data.Init(name, pattern, nameText, row);
            if (!base.Dictionary.ContainsKey(data.Key))
            {
                base.AddData(data);
                if (!this.defaultKey.ContainsKey(name))
                {
                    this.defaultKey.Add(name, data.Key);
                }
                return data;
            }
            Debug.LogError(string.Empty + row.ToErrorString(ColorUtil.AddColorTag(data.Key, Color.get_red()) + "  is already contains"));
            return null;
        }

        internal static string ToDataKey(string name, string label)
        {
            return string.Format("{0},{1}", name, label);
        }

        protected override bool TryParseContinues(AdvCharacterSettingData last, StringGridRow row)
        {
            if (last != null)
            {
                string str = AdvParser.ParseCellOptional<string>(row, AdvColumnName.CharacterName, string.Empty);
                string str2 = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Pattern, string.Empty);
                if (string.IsNullOrEmpty(str) && string.IsNullOrEmpty(str2))
                {
                    last.AddGraphicInfo(row);
                    return true;
                }
            }
            return false;
        }
    }
}

