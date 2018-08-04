namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class ColorUtil
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$mapD;
        public static readonly Color Aqua = new Color32(0, 0xff, 0xff, 0xff);
        public static readonly Color Black = new Color32(0, 0, 0, 0xff);
        public static readonly Color Blue = new Color32(0, 0, 0xff, 0xff);
        public static readonly Color Brown = new Color32(0xa5, 0x2a, 0x2a, 0xff);
        public static readonly Color Cyan = new Color32(0, 0xff, 0xff, 0xff);
        public static readonly Color Darkblue = new Color32(0, 0, 160, 0xff);
        public static readonly Color Fuchsia = new Color32(0xff, 0, 0xff, 0xff);
        public static readonly Color Green = new Color32(0, 0x80, 0, 0xff);
        public static readonly Color Grey = new Color32(0x80, 0x80, 0x80, 0xff);
        public static readonly Color Lightblue = new Color32(0xad, 0xd8, 230, 0xff);
        public static readonly Color Lime = new Color32(0, 0xff, 0, 0xff);
        public static readonly Color Magenta = new Color32(0xff, 0, 0xff, 0xff);
        public static readonly Color Maroon = new Color32(0x80, 0, 0, 0xff);
        public static readonly Color Navy = new Color32(0, 0, 0x80, 0xff);
        public static readonly Color Olive = new Color32(0x80, 0x80, 0, 0xff);
        public static readonly Color Orange = new Color32(0xff, 0xa5, 0, 0xff);
        public static readonly Color Purple = new Color32(0x80, 0, 0x80, 0xff);
        public static readonly Color Red = new Color32(0xff, 0, 0, 0xff);
        public static readonly Color Silver = new Color32(0xc0, 0xc0, 0xc0, 0xff);
        public static readonly Color Teal = new Color32(0, 0x80, 0x80, 0xff);
        public static readonly Color White = new Color32(0xff, 0xff, 0xff, 0xff);
        public static readonly Color Yellow = new Color32(0xff, 0xff, 0, 0xff);

        public static string AddColorTag(string str, string colorKey)
        {
            string format = "<color={1}>{0}</color>";
            return string.Format(format, str, colorKey);
        }

        public static string AddColorTag(string str, Color color)
        {
            return AddColorTag(str, "#" + ToColorString(color));
        }

        public static Color ParseColor(string text)
        {
            Color color = Color.get_white();
            if (TryParseColor(text, ref color))
            {
                return color;
            }
            object[] args = new object[] { text };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ColorParseError, args));
        }

        public static string ToColorString(Color color)
        {
            int num = (int) (255f * color.r);
            int num2 = (int) (255f * color.g);
            int num3 = (int) (255f * color.b);
            int num4 = (int) (255f * color.a);
            int num5 = (((num << 0x18) + (num2 << 0x10)) + (num3 << 8)) + num4;
            return num5.ToString("X8").ToLower();
        }

        public static string ToNguiColorString(Color color)
        {
            int num = (int) (255f * color.r);
            int num2 = (int) (255f * color.g);
            int num3 = (int) (255f * color.b);
            int num4 = ((num << 0x10) + (num2 << 8)) + num3;
            return num4.ToString("X6").ToLower();
        }

        public static bool TryParseColor(string text, ref Color color)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (text[0] == '#')
                {
                    return TryParseColorDetail(text.Substring(1), ref color);
                }
                if (text != null)
                {
                    int num;
                    if (<>f__switch$mapD == null)
                    {
                        Dictionary<string, int> dictionary = new Dictionary<string, int>(0x16);
                        dictionary.Add("aqua", 0);
                        dictionary.Add("black", 1);
                        dictionary.Add("blue", 2);
                        dictionary.Add("brown", 3);
                        dictionary.Add("cyan", 4);
                        dictionary.Add("darkblue", 5);
                        dictionary.Add("fuchsia", 6);
                        dictionary.Add("green", 7);
                        dictionary.Add("grey", 8);
                        dictionary.Add("lightblue", 9);
                        dictionary.Add("lime", 10);
                        dictionary.Add("magenta", 11);
                        dictionary.Add("maroon", 12);
                        dictionary.Add("navy", 13);
                        dictionary.Add("olive", 14);
                        dictionary.Add("orange", 15);
                        dictionary.Add("purple", 0x10);
                        dictionary.Add("red", 0x11);
                        dictionary.Add("silver", 0x12);
                        dictionary.Add("teal", 0x13);
                        dictionary.Add("white", 20);
                        dictionary.Add("yellow", 0x15);
                        <>f__switch$mapD = dictionary;
                    }
                    if (<>f__switch$mapD.TryGetValue(text, out num))
                    {
                        switch (num)
                        {
                            case 0:
                                color = Cyan;
                                goto Label_0337;

                            case 1:
                                color = Black;
                                goto Label_0337;

                            case 2:
                                color = Blue;
                                goto Label_0337;

                            case 3:
                                color = Brown;
                                goto Label_0337;

                            case 4:
                                color = Cyan;
                                goto Label_0337;

                            case 5:
                                color = Darkblue;
                                goto Label_0337;

                            case 6:
                                color = Magenta;
                                goto Label_0337;

                            case 7:
                                color = Green;
                                goto Label_0337;

                            case 8:
                                color = Grey;
                                goto Label_0337;

                            case 9:
                                color = Lightblue;
                                goto Label_0337;

                            case 10:
                                color = Lime;
                                goto Label_0337;

                            case 11:
                                color = Magenta;
                                goto Label_0337;

                            case 12:
                                color = Maroon;
                                goto Label_0337;

                            case 13:
                                color = Navy;
                                goto Label_0337;

                            case 14:
                                color = Olive;
                                goto Label_0337;

                            case 15:
                                color = Orange;
                                goto Label_0337;

                            case 0x10:
                                color = Purple;
                                goto Label_0337;

                            case 0x11:
                                color = Red;
                                goto Label_0337;

                            case 0x12:
                                color = Silver;
                                goto Label_0337;

                            case 0x13:
                                color = Teal;
                                goto Label_0337;

                            case 20:
                                color = White;
                                goto Label_0337;

                            case 0x15:
                                color = Yellow;
                                goto Label_0337;
                        }
                    }
                }
            }
            return false;
        Label_0337:
            return true;
        }

        private static bool TryParseColorDetail(string text, ref Color color)
        {
            try
            {
                if (text.Length == 6)
                {
                    int num = Convert.ToInt32(text, 0x10);
                    float num2 = ((float) ((num & 0xff0000) >> 0x10)) / 255f;
                    float num3 = ((float) ((num & 0xff00) >> 8)) / 255f;
                    float num4 = ((float) (num & 0xff)) / 255f;
                    color = new Color(num2, num3, num4);
                    return true;
                }
                if (text.Length == 8)
                {
                    int num5 = Convert.ToInt32(text, 0x10);
                    float num6 = ((float) ((num5 & 0xff000000L) >> 0x18)) / 255f;
                    float num7 = ((float) ((num5 & 0xff0000) >> 0x10)) / 255f;
                    float num8 = ((float) ((num5 & 0xff00) >> 8)) / 255f;
                    float num9 = ((float) (num5 & 0xff)) / 255f;
                    color = new Color(num6, num7, num8, num9);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

