namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class AdvScenarioDataExported : ScriptableObject
    {
        [SerializeField]
        private StringGridDictionary dictionary;

        public void Clear()
        {
            this.dictionary.Clear();
        }

        public void ParseFromExcel(string sheetName, StringGrid grid)
        {
            this.dictionary.Add(sheetName, grid);
        }

        public List<StringGridDictionaryKeyValue> List
        {
            get
            {
                return this.dictionary.List;
            }
        }
    }
}

