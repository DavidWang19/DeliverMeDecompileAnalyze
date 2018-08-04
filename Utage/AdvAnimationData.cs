namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    public class AdvAnimationData : IAdvSettingData
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AnimationClip <Clip>k__BackingField;

        public AdvAnimationData(StringGrid grid, ref int index, bool legacy)
        {
            this.Clip = new AnimationClip();
            this.Clip.set_legacy(legacy);
            this.ParseHeader(grid.Rows[index++]);
            List<float> timeTbl = this.ParseTimeTbl(grid.Rows[index++]);
            if (!this.Clip.get_legacy())
            {
                this.AddDummyCurve(timeTbl);
            }
            while (index < grid.Rows.Count)
            {
                StringGridRow row = grid.Rows[index];
                try
                {
                    if (row.IsEmptyOrCommantOut)
                    {
                        index++;
                    }
                    else
                    {
                        PropertyType type;
                        if (this.IsHeader(row))
                        {
                            break;
                        }
                        if (!row.TryParseCellTypeOptional<PropertyType>(0, PropertyType.Custom, out type))
                        {
                            string str2;
                            string str3;
                            row.ParseCell<string>(0).Separate('.', false, out str2, out str3);
                            Type type2 = Type.GetType(str2);
                            if (type2 == null)
                            {
                                Debug.LogError(str2 + "is not class name");
                            }
                            this.Clip.SetCurve(string.Empty, type2, str3, this.ParseCurve(timeTbl, row));
                        }
                        else if (this.IsEvent(type))
                        {
                            this.AddEvent(type, timeTbl, row);
                        }
                        else
                        {
                            this.AddCurve(type, this.ParseCurve(timeTbl, row));
                        }
                        index++;
                    }
                    continue;
                }
                catch (Exception exception)
                {
                    Debug.LogError(row.ToErrorString(exception.Message));
                    continue;
                }
            }
        }

        private void AddCurve(PropertyType type, AnimationCurve curve)
        {
            if (curve.get_keys().Length > 0)
            {
                switch (type)
                {
                    case PropertyType.X:
                        this.Clip.SetCurve(string.Empty, typeof(Transform), "localPosition.x", curve);
                        break;

                    case PropertyType.Y:
                        this.Clip.SetCurve(string.Empty, typeof(Transform), "localPosition.y", curve);
                        break;

                    case PropertyType.Z:
                        this.Clip.SetCurve(string.Empty, typeof(Transform), "localPosition.z", curve);
                        break;

                    case PropertyType.Scale:
                        this.Clip.SetCurve(string.Empty, typeof(Transform), "localScale.x", curve);
                        this.Clip.SetCurve(string.Empty, typeof(Transform), "localScale.y", curve);
                        this.Clip.SetCurve(string.Empty, typeof(Transform), "localScale.z", curve);
                        break;

                    case PropertyType.ScaleX:
                        this.Clip.SetCurve(string.Empty, typeof(Transform), "localScale.x", curve);
                        break;

                    case PropertyType.ScaleY:
                        this.Clip.SetCurve(string.Empty, typeof(Transform), "localScale.y", curve);
                        break;

                    case PropertyType.ScaleZ:
                        this.Clip.SetCurve(string.Empty, typeof(Transform), "localScale.z", curve);
                        break;

                    case PropertyType.Angle:
                    case PropertyType.AngleZ:
                        this.Clip.SetCurve(string.Empty, typeof(Transform), "localEulerAngles.z", curve);
                        break;

                    case PropertyType.AngleX:
                        this.Clip.SetCurve(string.Empty, typeof(Transform), "localEulerAngles.x", curve);
                        break;

                    case PropertyType.AngleY:
                        this.Clip.SetCurve(string.Empty, typeof(Transform), "localEulerAngles.y", curve);
                        break;

                    case PropertyType.Alpha:
                        this.Clip.SetCurve(string.Empty, typeof(AdvEffectColor), "animationColor.a", curve);
                        break;

                    default:
                        Debug.LogError("UnknownType");
                        break;
                }
            }
        }

        private void AddDummyCurve(List<float> timeTbl)
        {
            AnimationCurve curve = AnimationCurve.Linear(timeTbl[0], 0f, timeTbl[timeTbl.Count - 1], 1f);
            this.Clip.SetCurve(string.Empty, typeof(Object), string.Empty, curve);
        }

        private void AddEvent(PropertyType propertyType, List<float> timeTbl, StringGridRow row)
        {
            for (int i = 0; i < row.Strings.Length; i++)
            {
                if ((i != 0) && !row.IsEmptyCell(i))
                {
                    AnimationEvent event2 = new AnimationEvent();
                    if (propertyType == PropertyType.Texture)
                    {
                        string str;
                        if (!row.TryParseCell<string>(i, out str))
                        {
                            continue;
                        }
                        event2.set_functionName("ChangePattern");
                        event2.set_stringParameter(str);
                        event2.set_time(timeTbl[i - 1]);
                    }
                    if (Application.get_isPlaying())
                    {
                        this.Clip.AddEvent(event2);
                    }
                }
            }
        }

        private bool IsCustomProperty(PropertyType type)
        {
            if (type != PropertyType.Custom)
            {
                return false;
            }
            return true;
        }

        private bool IsEvent(PropertyType type)
        {
            if (type != PropertyType.Texture)
            {
                return false;
            }
            return true;
        }

        private bool IsHeader(StringGridRow row)
        {
            return (row.ParseCell<string>(0)[0] == '*');
        }

        private AnimationCurve ParseCurve(List<float> timeTbl, StringGridRow row)
        {
            AnimationCurve curve = new AnimationCurve();
            for (int i = 0; i < row.Strings.Length; i++)
            {
                float num2;
                if (((i != 0) && !row.IsEmptyCell(i)) && row.TryParseCell<float>(i, out num2))
                {
                    curve.AddKey(new Keyframe(timeTbl[i - 1], num2));
                }
            }
            if (curve.get_keys().Length <= 1)
            {
            }
            return curve;
        }

        private void ParseHeader(StringGridRow row)
        {
            this.Clip.set_name(row.ParseCell<string>(0).Substring(1));
            this.Clip.set_wrapMode(row.ParseCellOptional<WrapMode>(1, 0));
        }

        private List<float> ParseTimeTbl(StringGridRow row)
        {
            List<float> list = new List<float>();
            for (int i = 1; i < row.Strings.Length; i++)
            {
                float num2;
                if (!row.TryParseCell<float>(i, out num2))
                {
                    Debug.LogError(row.ToErrorString("TimeTbl pase error"));
                }
                list.Add(num2);
            }
            return list;
        }

        public AnimationClip Clip { get; set; }

        private enum PropertyType
        {
            Custom,
            X,
            Y,
            Z,
            Scale,
            ScaleX,
            ScaleY,
            ScaleZ,
            Angle,
            AngleX,
            AngleY,
            AngleZ,
            Alpha,
            Texture
        }
    }
}

