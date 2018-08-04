namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using UnityEngine;

    public class AdvMacroManager
    {
        private Dictionary<string, AdvMacroData> macroDataTbl = new Dictionary<string, AdvMacroData>();
        private const string SheetNamePattern = "Macro[0-9]";
        private static readonly Regex SheetNameRegex = new Regex("Macro[0-9]", RegexOptions.IgnorePatternWhitespace);

        public static bool IsMacroName(string sheetName)
        {
            return ((sheetName == "Macro") || SheetNameRegex.Match(sheetName).Success);
        }

        public bool TryAddMacroData(string name, StringGrid grid)
        {
            if (!IsMacroName(name))
            {
                return false;
            }
            int num = 0;
            while (num < grid.Rows.Count)
            {
                string str;
                StringGridRow row = grid.Rows[num];
                num++;
                if (((row.RowIndex >= grid.DataTopRow) && !row.IsEmptyOrCommantOut) && this.TryParseMacoBegin(row, out str))
                {
                    List<StringGridRow> dataList = new List<StringGridRow>();
                    while (num < grid.Rows.Count)
                    {
                        StringGridRow row2 = grid.Rows[num];
                        num++;
                        if (!row2.IsEmptyOrCommantOut)
                        {
                            if (AdvParser.ParseCellOptional<string>(row2, AdvColumnName.Command, string.Empty) == "EndMacro")
                            {
                                break;
                            }
                            dataList.Add(row2);
                        }
                    }
                    if (this.macroDataTbl.ContainsKey(str))
                    {
                        Debug.LogError(row.ToErrorString(str + " is already contains "));
                    }
                    else
                    {
                        this.macroDataTbl.Add(str, new AdvMacroData(str, row, dataList));
                    }
                }
            }
            return true;
        }

        public bool TryMacroExpansion(StringGridRow row, List<StringGridRow> outputList, string debugMsg)
        {
            AdvMacroData data;
            string key = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Command, string.Empty);
            if (!this.macroDataTbl.TryGetValue(key, out data))
            {
                return false;
            }
            if (string.IsNullOrEmpty(debugMsg))
            {
                debugMsg = row.Grid.Name + ":" + ((row.RowIndex + 1)).ToString();
            }
            debugMsg = debugMsg + " -> MACRO " + data.Header.Grid.Name;
            foreach (StringGridRow row2 in data.MacroExpansion(row, debugMsg))
            {
                if (!this.TryMacroExpansion(row2, outputList, row2.DebugInfo))
                {
                    outputList.Add(row2);
                }
            }
            return true;
        }

        private bool TryParseMacoBegin(StringGridRow row, out string macroName)
        {
            return AdvCommandParser.TryParseScenarioLabel(row, AdvColumnName.Command, out macroName);
        }
    }
}

