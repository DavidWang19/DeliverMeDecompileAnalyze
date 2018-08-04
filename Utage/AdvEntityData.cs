namespace Utage
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using UnityEngine;

    [Serializable]
    public class AdvEntityData
    {
        [SerializeField]
        private string[] originalStrings;

        public AdvEntityData(string[] originalStrings)
        {
            this.originalStrings = originalStrings;
        }

        private static bool CheckEntitySeparator(char c)
        {
            switch (c)
            {
                case '[':
                case ']':
                    break;

                default:
                    if (c != '.')
                    {
                        return ExpressionToken.CheckSeparator(c);
                    }
                    break;
            }
            return true;
        }

        public static bool ContainsEntitySimple(StringGridRow row)
        {
            for (int i = 0; i < row.Strings.Length; i++)
            {
                int index = row.Strings[i].IndexOf('&');
                if (index >= 0)
                {
                    string str = row.Strings[i];
                    if (((index + 1) >= str.Length) || (str[index + 1] != '&'))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public string[] CreateCommandStrings(Func<string, object> GetParameter)
        {
            string[] strArray = new string[this.originalStrings.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                string str = strArray[i] = this.originalStrings[i];
                if ((str.Length > 1) && (str.IndexOf('&') >= 0))
                {
                    StringBuilder builder = new StringBuilder();
                    int num2 = 0;
                    while (num2 < str.Length)
                    {
                        if (str[num2] == '&')
                        {
                            bool flag = false;
                            for (int j = num2 + 1; j < str.Length; j++)
                            {
                                if ((j == (str.Length - 1)) || CheckEntitySeparator(str[j + 1]))
                                {
                                    string arg = str.Substring(num2 + 1, j - num2);
                                    object obj2 = GetParameter(arg);
                                    if (obj2 != null)
                                    {
                                        builder.Append(obj2.ToString());
                                        num2 = j + 1;
                                        flag = true;
                                    }
                                    break;
                                }
                            }
                            if (flag)
                            {
                                continue;
                            }
                        }
                        builder.Append(str[num2]);
                        num2++;
                    }
                    strArray[i] = builder.ToString();
                }
            }
            return strArray;
        }

        public static AdvCommand CreateEntityCommand(AdvCommand original, AdvEngine engine, AdvScenarioPageData pageData)
        {
            StringGridRow row = new StringGridRow(original.RowData.Grid, original.RowData.RowIndex) {
                DebugIndex = original.RowData.DebugIndex
            };
            string[] strings = original.EntityData.CreateCommandStrings(new Func<string, object>(engine.Param.GetParameter));
            row.InitFromStringArray(strings);
            AdvCommand command = AdvCommandParser.CreateCommand(original.Id, row, engine.DataManager.SettingDataManager);
            if (command is AdvCommandText)
            {
                (command as AdvCommandText).InitOnCreateEntity(original as AdvCommandText);
            }
            return command;
        }

        public static bool TryCreateEntityStrings(StringGridRow original, Func<string, object> GetParameter, out string[] strings)
        {
            bool flag = false;
            strings = new string[original.Strings.Length];
            for (int i = 0; i < original.Strings.Length; i++)
            {
                string str = strings[i] = original.Strings[i];
                if ((str.Length > 1) && (str.IndexOf('&') >= 0))
                {
                    int num2;
                    int num3;
                    if (original.Grid.TryGetColumnIndex(AdvColumnName.WindowType.QuickToString(), out num2) && (i == num2))
                    {
                        Debug.LogError(" Can not use entity in " + AdvColumnName.WindowType.QuickToString());
                        return false;
                    }
                    if (original.Grid.TryGetColumnIndex(AdvColumnName.PageCtrl.QuickToString(), out num3) && (i == num3))
                    {
                        Debug.LogError(" Can not use entity in " + AdvColumnName.PageCtrl.QuickToString());
                        return false;
                    }
                    StringBuilder builder = new StringBuilder();
                    int num4 = 0;
                    while (num4 < str.Length)
                    {
                        if (str[num4] == '&')
                        {
                            bool flag2 = false;
                            for (int j = num4 + 1; j < str.Length; j++)
                            {
                                if ((j == (str.Length - 1)) || CheckEntitySeparator(str[j + 1]))
                                {
                                    string arg = str.Substring(num4 + 1, j - num4);
                                    object obj2 = GetParameter(arg);
                                    if (obj2 != null)
                                    {
                                        builder.Append(obj2.ToString());
                                        num4 = j + 1;
                                        flag2 = true;
                                    }
                                    break;
                                }
                            }
                            if (flag2)
                            {
                                flag = true;
                                continue;
                            }
                        }
                        builder.Append(str[num4]);
                        num4++;
                    }
                    strings[i] = builder.ToString();
                }
            }
            return flag;
        }
    }
}

