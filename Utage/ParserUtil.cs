namespace Utage
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using UnityEngine;

    public static class ParserUtil
    {
        public static Vector2 ParsePivotOptional(string text, Vector2 defaultValue)
        {
            Pivot pivot;
            if (string.IsNullOrEmpty(text))
            {
                return defaultValue;
            }
            Vector2 vector = (Vector2) (Vector2.get_one() * 0.5f);
            if (TryParaseEnum<Pivot>(text, out pivot))
            {
                return PivotUtil.PivotEnumToVector2(pivot);
            }
            if (TryParseVector2Optional(text, vector, out vector))
            {
                return vector;
            }
            object[] args = new object[] { text };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.PivotParse, args));
        }

        public static Vector2 ParseScale2DOptional(string text, Vector2 defaultValue)
        {
            float num;
            if (string.IsNullOrEmpty(text))
            {
                return defaultValue;
            }
            Vector2 vector = defaultValue;
            if (float.TryParse(text, out num))
            {
                return (Vector2) (Vector2.get_one() * num);
            }
            if (!TryParseVector2Optional(text, vector, out vector))
            {
                throw new Exception("Parse Scale2D Error " + text);
            }
            return vector;
        }

        public static Vector3 ParseScale3DOptional(string text, Vector3 defaultValue)
        {
            float num;
            if (string.IsNullOrEmpty(text))
            {
                return defaultValue;
            }
            Vector3 vector = defaultValue;
            if (float.TryParse(text, out num))
            {
                return (Vector3) (Vector3.get_one() * num);
            }
            if (!TryParseVector3Optional(text, vector, out vector))
            {
                throw new Exception("Parse Scale3D Error " + text);
            }
            return vector;
        }

        public static int ParseTag(string text, int start, Func<string, string, bool> callbackParseTag)
        {
            if (text[start] == '<')
            {
                int startIndex = start + 1;
                int index = text.IndexOf('>', startIndex);
                if (index < 0)
                {
                    return start;
                }
                char[] separator = new char[] { '=' };
                string[] strArray = text.Substring(startIndex, index - startIndex).Split(separator, StringSplitOptions.RemoveEmptyEntries);
                if ((strArray.Length < 1) || (strArray.Length > 2))
                {
                    return start;
                }
                string str = strArray[0];
                string str2 = (strArray.Length <= 1) ? string.Empty : strArray[1];
                if (callbackParseTag(str, str2))
                {
                    return index;
                }
            }
            return start;
        }

        public static string ParseTagTextToString(string text, Func<string, string, bool> callbackTagParse)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            int start = 0;
            StringBuilder builder = new StringBuilder();
            while (start < text.Length)
            {
                int num2 = ParseTag(text, start, callbackTagParse);
                if (num2 == start)
                {
                    builder.Append(text[start]);
                    start++;
                }
                else
                {
                    start = num2 + 1;
                }
            }
            return builder.ToString();
        }

        public static int ToMagicID(char id0, char id1, char id2, char id3)
        {
            return ((((id3 << 0x18) + (id2 << 0x10)) + (id1 << 8)) + id0);
        }

        public static bool TryParaseEnum<T>(string str, out T val)
        {
            try
            {
                val = (T) Enum.Parse(typeof(T), str);
                return true;
            }
            catch (Exception)
            {
                val = default(T);
                return false;
            }
        }

        public static bool TryParseVector2Optional(string text, Vector2 defaultValue, out Vector2 vec2)
        {
            vec2 = defaultValue;
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            bool flag = false;
            char[] separator = new char[] { ' ' };
            foreach (string str in text.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                char[] chArray2 = new char[] { '=' };
                string[] strArray3 = str.Split(chArray2, StringSplitOptions.RemoveEmptyEntries);
                if (strArray3.Length != 2)
                {
                    return false;
                }
                string str2 = strArray3[0];
                if (str2 == null)
                {
                    goto Label_00CC;
                }
                if (!(str2 == "x"))
                {
                    if (str2 == "y")
                    {
                        goto Label_00AF;
                    }
                    goto Label_00CC;
                }
                if (!float.TryParse(strArray3[1], out vec2.x))
                {
                    return false;
                }
                flag = true;
                continue;
            Label_00AF:
                if (!float.TryParse(strArray3[1], out vec2.y))
                {
                    return false;
                }
                flag = true;
                continue;
            Label_00CC:
                return false;
            }
            return flag;
        }

        public static bool TryParseVector3Optional(string text, Vector3 defaultValue, out Vector3 vec3)
        {
            vec3 = defaultValue;
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            bool flag = false;
            char[] separator = new char[] { ' ' };
            foreach (string str in text.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                char[] chArray2 = new char[] { '=' };
                string[] strArray3 = str.Split(chArray2, StringSplitOptions.RemoveEmptyEntries);
                if (strArray3.Length != 2)
                {
                    return false;
                }
                string str2 = strArray3[0];
                if (str2 == null)
                {
                    goto Label_00FA;
                }
                if (!(str2 == "x"))
                {
                    if (str2 == "y")
                    {
                        goto Label_00C0;
                    }
                    if (str2 == "z")
                    {
                        goto Label_00DD;
                    }
                    goto Label_00FA;
                }
                if (!float.TryParse(strArray3[1], out vec3.x))
                {
                    return false;
                }
                flag = true;
                continue;
            Label_00C0:
                if (!float.TryParse(strArray3[1], out vec3.y))
                {
                    return false;
                }
                flag = true;
                continue;
            Label_00DD:
                if (!float.TryParse(strArray3[1], out vec3.z))
                {
                    return false;
                }
                flag = true;
                continue;
            Label_00FA:
                return false;
            }
            return flag;
        }
    }
}

