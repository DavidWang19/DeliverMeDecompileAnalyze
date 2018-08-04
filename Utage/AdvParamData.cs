namespace Utage
{
    using System;
    using UnityEngine;

    public class AdvParamData
    {
        private FileType fileType;
        private string key;
        private object parameter;
        private string parameterString;
        private ParamType type;

        public void Copy(AdvParamData src)
        {
            this.key = src.Key;
            this.type = src.type;
            this.parameterString = src.parameterString;
            this.ParseParameterString();
            this.fileType = src.fileType;
        }

        public void CopySaveData(AdvParamData src)
        {
            if (this.key != src.Key)
            {
                Debug.LogError(src.key + "is diffent name of Saved param");
            }
            if (this.type != src.type)
            {
                Debug.LogError(src.type + "is diffent type of Saved param");
            }
            if (this.fileType != src.fileType)
            {
                Debug.LogError(src.fileType + "is diffent fileType of Saved param");
            }
            this.parameterString = src.parameterString;
            this.ParseParameterString();
        }

        private void ParseParameterString()
        {
            switch (this.type)
            {
                case ParamType.Bool:
                    this.parameter = bool.Parse(this.parameterString);
                    break;

                case ParamType.Float:
                    this.parameter = float.Parse(this.parameterString);
                    break;

                case ParamType.Int:
                    this.parameter = int.Parse(this.parameterString);
                    break;

                case ParamType.String:
                    this.parameter = this.parameterString;
                    break;
            }
        }

        public void Read(string paramString)
        {
            this.parameterString = paramString;
            this.ParseParameterString();
        }

        public bool TryParse(StringGridRow row)
        {
            string str = AdvParser.ParseCell<string>(row, AdvColumnName.Label);
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            this.key = str;
            this.type = AdvParser.ParseCell<ParamType>(row, AdvColumnName.Type);
            this.parameterString = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Value, string.Empty);
            this.fileType = AdvParser.ParseCellOptional<FileType>(row, AdvColumnName.FileType, FileType.Default);
            try
            {
                this.ParseParameterString();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryParse(AdvParamData src, string value)
        {
            this.key = src.Key;
            this.type = src.Type;
            this.fileType = src.SaveFileType;
            this.parameterString = value;
            try
            {
                this.ParseParameterString();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryParse(string name, string type, string fileType)
        {
            this.key = name;
            if (!ParserUtil.TryParaseEnum<ParamType>(type, out this.type))
            {
                Debug.LogError(type + " is not ParamType");
                return false;
            }
            if (string.IsNullOrEmpty(fileType))
            {
                this.fileType = FileType.Default;
            }
            else if (!ParserUtil.TryParaseEnum<FileType>(fileType, out this.fileType))
            {
                Debug.LogError(fileType + " is not FileType");
                return false;
            }
            return true;
        }

        public string Key
        {
            get
            {
                return this.key;
            }
        }

        public object Parameter
        {
            get
            {
                if (this.parameter == null)
                {
                    this.ParseParameterString();
                }
                return this.parameter;
            }
            set
            {
                switch (this.type)
                {
                    case ParamType.Bool:
                        this.parameter = (bool) value;
                        break;

                    case ParamType.Float:
                        this.parameter = ExpressionCast.ToFloat(value);
                        break;

                    case ParamType.Int:
                        this.parameter = ExpressionCast.ToInt(value);
                        break;

                    case ParamType.String:
                        this.parameter = (string) value;
                        break;
                }
                this.parameterString = this.parameter.ToString();
            }
        }

        public string ParameterString
        {
            get
            {
                return this.parameterString;
            }
        }

        public FileType SaveFileType
        {
            get
            {
                return this.fileType;
            }
        }

        public ParamType Type
        {
            get
            {
                return this.type;
            }
        }

        public enum FileType
        {
            Default,
            System,
            Const
        }

        public enum ParamType
        {
            Bool,
            Float,
            Int,
            String
        }
    }
}

