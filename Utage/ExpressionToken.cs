namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class ExpressionToken
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map5;
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map6;
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map7;
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map8;
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map9;
        private const string And = "&&";
        private const string Comma = ",";
        public static readonly ExpressionToken CommaToken = new ExpressionToken(",", false, TokenType.Comma, 0);
        private const string Div = "/";
        private const string DivEq = "/=";
        private const string Eq = "=";
        private const string EqEq = "==";
        private const string FuncCeil = "Ceil";
        private const string FuncCeilToInt = "CeilToInt";
        private const string FuncFloor = "Floor";
        private const string FuncFloorToInt = "FloorToInt";
        private const string FuncRandom = "Random";
        private const string FuncRandomF = "RandomF";
        private const string Greater = ">";
        private const string GreaterEq = ">=";
        private bool isAlphabet;
        private const string Less = "<";
        private const string LessEq = "<=";
        private const string Lpa = "(";
        public static readonly ExpressionToken LpaToken = new ExpressionToken("(", false, TokenType.Lpa, 0);
        public const string Minus = "-";
        private const string MinusEq = "-=";
        private const string Mod = "%";
        private const string ModEq = "%=";
        private string name;
        private const string Not = "!";
        private const string NotEq = "!=";
        private int numFunctionArg;
        private static readonly ExpressionToken[] OperatorArray = new ExpressionToken[] { 
            LpaToken, RpaToken, CommaToken, new ExpressionToken(">=", false, TokenType.Binary, 4), new ExpressionToken("<=", false, TokenType.Binary, 4), new ExpressionToken(">", false, TokenType.Binary, 4), new ExpressionToken("<", false, TokenType.Binary, 4), new ExpressionToken("==", false, TokenType.Binary, 5), new ExpressionToken("!=", false, TokenType.Binary, 5), new ExpressionToken("&&", false, TokenType.Binary, 6), new ExpressionToken("||", false, TokenType.Binary, 7), new ExpressionToken("=", false, TokenType.Substitution, 8), new ExpressionToken("+=", false, TokenType.Substitution, 8), new ExpressionToken("-=", false, TokenType.Substitution, 8), new ExpressionToken("*=", false, TokenType.Substitution, 8), new ExpressionToken("/=", false, TokenType.Substitution, 8), 
            new ExpressionToken("%=", false, TokenType.Substitution, 8), new ExpressionToken("!", false, TokenType.Unary, 1), new ExpressionToken("*", false, TokenType.Binary, 2), new ExpressionToken("/", false, TokenType.Binary, 2), new ExpressionToken("%", false, TokenType.Binary, 2), new ExpressionToken("+", false, TokenType.Binary, 3), new ExpressionToken("-", false, TokenType.Binary, 3)
         };
        private const string Or = "||";
        public const string Plus = "+";
        private const string PlusEq = "+=";
        private int priority;
        private const string Prod = "*";
        private const string ProdEq = "*=";
        private const string Rpa = ")";
        public static readonly ExpressionToken RpaToken = new ExpressionToken(")", false, TokenType.Rpa, 0);
        private TokenType type;
        public static readonly ExpressionToken UniMinus = new ExpressionToken("-", false, TokenType.Unary, 1);
        public static readonly ExpressionToken UniPlus = new ExpressionToken("+", false, TokenType.Unary, 1);
        private object variable;

        public ExpressionToken(string name, bool isAlphabet, TokenType type, int priority)
        {
            this.Create(name, isAlphabet, type, priority, null);
        }

        public ExpressionToken(string name, bool isAlphabet, TokenType type, int priority, object variable)
        {
            this.Create(name, isAlphabet, type, priority, variable);
        }

        private static object CalcBinary(object value1, ExpressionToken token, object value2)
        {
            string name = token.name;
            if (name != null)
            {
                int num;
                if (<>f__switch$map5 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(13);
                    dictionary.Add("*", 0);
                    dictionary.Add("/", 0);
                    dictionary.Add("%", 0);
                    dictionary.Add("+", 0);
                    dictionary.Add("-", 0);
                    dictionary.Add(">", 0);
                    dictionary.Add("<", 0);
                    dictionary.Add(">=", 0);
                    dictionary.Add("<=", 0);
                    dictionary.Add("==", 1);
                    dictionary.Add("!=", 1);
                    dictionary.Add("&&", 2);
                    dictionary.Add("||", 2);
                    <>f__switch$map5 = dictionary;
                }
                if (<>f__switch$map5.TryGetValue(name, out num))
                {
                    switch (num)
                    {
                        case 0:
                            return CalcBinaryNumber(value1, token, value2);

                        case 1:
                            return CalcBinaryEq(value1, token, value2);

                        case 2:
                            return CalcBinaryAndOr(value1, token, value2);
                    }
                }
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryAndOr(object value1, ExpressionToken token, object value2)
        {
            if ((value1 is bool) && (value2 is bool))
            {
                return CalcBinaryAndOrSub((bool) value1, token, (bool) value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryAndOrSub(bool value1, ExpressionToken token, bool value2)
        {
            switch (token.name)
            {
                case "&&":
                    return (!value1 ? ((object) 0) : ((object) value2));

                case "||":
                    return (value1 ? ((object) 1) : ((object) value2));
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryEq(object value1, ExpressionToken token, object value2)
        {
            if (value1 is int)
            {
                if (value2 is int)
                {
                    return CalcBinaryEqSub((int) value1, token, (int) value2);
                }
                if (value2 is float)
                {
                    return CalcBinaryEqSub((int) value1, token, (float) value2);
                }
            }
            else if (value1 is float)
            {
                if (value2 is int)
                {
                    return CalcBinaryEqSub((float) value1, token, (int) value2);
                }
                if (value2 is float)
                {
                    return CalcBinaryEqSub((float) value1, token, (float) value2);
                }
            }
            else if (value1 is bool)
            {
                if (value2 is bool)
                {
                    return CalcBinaryEqSub((bool) value1, token, (bool) value2);
                }
            }
            else if ((value1 is string) && (value2 is string))
            {
                return CalcBinaryEqSub((string) value1, token, (string) value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryEqSub(bool value1, ExpressionToken token, bool value2)
        {
            switch (token.name)
            {
                case "==":
                    return (value1 == value2);

                case "!=":
                    return (value1 != value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryEqSub(int value1, ExpressionToken token, int value2)
        {
            switch (token.name)
            {
                case "==":
                    return (value1 == value2);

                case "!=":
                    return (value1 != value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryEqSub(int value1, ExpressionToken token, float value2)
        {
            switch (token.name)
            {
                case "==":
                    return (value1 == value2);

                case "!=":
                    return (value1 != value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryEqSub(float value1, ExpressionToken token, int value2)
        {
            switch (token.name)
            {
                case "==":
                    return (value1 == value2);

                case "!=":
                    return (value1 != value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryEqSub(float value1, ExpressionToken token, float value2)
        {
            switch (token.name)
            {
                case "==":
                    return (value1 == value2);

                case "!=":
                    return (value1 != value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryEqSub(string value1, ExpressionToken token, string value2)
        {
            switch (token.name)
            {
                case "==":
                    return (value1 == value2);

                case "!=":
                    return (value1 != value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryNumber(object value1, ExpressionToken token, object value2)
        {
            if (value1 is int)
            {
                if (value2 is int)
                {
                    return CalcBinaryNumberSub((int) value1, token, (int) value2);
                }
                if (value2 is float)
                {
                    return CalcBinaryNumberSub((int) value1, token, (float) value2);
                }
                if (value2 is string)
                {
                    return CalcBinaryNumberSub((int) value1, token, (string) value2);
                }
            }
            else if (value1 is float)
            {
                if (value2 is int)
                {
                    return CalcBinaryNumberSub((float) value1, token, (int) value2);
                }
                if (value2 is float)
                {
                    return CalcBinaryNumberSub((float) value1, token, (float) value2);
                }
                if (value2 is string)
                {
                    return CalcBinaryNumberSub((float) value1, token, (string) value2);
                }
            }
            else if (value1 is string)
            {
                if (value2 is int)
                {
                    return CalcBinaryNumberSub((string) value1, token, (int) value2);
                }
                if (value2 is float)
                {
                    return CalcBinaryNumberSub((string) value1, token, (float) value2);
                }
                if (value2 is string)
                {
                    return CalcBinaryNumberSub((string) value1, token, (string) value2);
                }
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryNumberSub(bool value1, ExpressionToken token, string value2)
        {
            string name = token.name;
            if ((name != null) && (name == "+"))
            {
                return (value1 + value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryNumberSub(int value1, ExpressionToken token, int value2)
        {
            string name = token.name;
            if (name != null)
            {
                int num;
                if (<>f__switch$map6 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(9);
                    dictionary.Add("*", 0);
                    dictionary.Add("/", 1);
                    dictionary.Add("%", 2);
                    dictionary.Add("+", 3);
                    dictionary.Add("-", 4);
                    dictionary.Add(">", 5);
                    dictionary.Add("<", 6);
                    dictionary.Add(">=", 7);
                    dictionary.Add("<=", 8);
                    <>f__switch$map6 = dictionary;
                }
                if (<>f__switch$map6.TryGetValue(name, out num))
                {
                    switch (num)
                    {
                        case 0:
                            return (value1 * value2);

                        case 1:
                            return (value1 / value2);

                        case 2:
                            return (value1 % value2);

                        case 3:
                            return (value1 + value2);

                        case 4:
                            return (value1 - value2);

                        case 5:
                            return (value1 > value2);

                        case 6:
                            return (value1 < value2);

                        case 7:
                            return (value1 >= value2);

                        case 8:
                            return (value1 <= value2);
                    }
                }
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryNumberSub(int value1, ExpressionToken token, float value2)
        {
            string name = token.name;
            if (name != null)
            {
                int num;
                if (<>f__switch$map7 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(9);
                    dictionary.Add("*", 0);
                    dictionary.Add("/", 1);
                    dictionary.Add("%", 2);
                    dictionary.Add("+", 3);
                    dictionary.Add("-", 4);
                    dictionary.Add(">", 5);
                    dictionary.Add("<", 6);
                    dictionary.Add(">=", 7);
                    dictionary.Add("<=", 8);
                    <>f__switch$map7 = dictionary;
                }
                if (<>f__switch$map7.TryGetValue(name, out num))
                {
                    switch (num)
                    {
                        case 0:
                            return (value1 * value2);

                        case 1:
                            return (((float) value1) / value2);

                        case 2:
                            return (((float) value1) % value2);

                        case 3:
                            return (value1 + value2);

                        case 4:
                            return (value1 - value2);

                        case 5:
                            return (value1 > value2);

                        case 6:
                            return (value1 < value2);

                        case 7:
                            return (value1 >= value2);

                        case 8:
                            return (value1 <= value2);
                    }
                }
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryNumberSub(int value1, ExpressionToken token, string value2)
        {
            string name = token.name;
            if ((name != null) && (name == "+"))
            {
                return (value1 + value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryNumberSub(float value1, ExpressionToken token, int value2)
        {
            string name = token.name;
            if (name != null)
            {
                int num;
                if (<>f__switch$map8 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(9);
                    dictionary.Add("*", 0);
                    dictionary.Add("/", 1);
                    dictionary.Add("%", 2);
                    dictionary.Add("+", 3);
                    dictionary.Add("-", 4);
                    dictionary.Add(">", 5);
                    dictionary.Add("<", 6);
                    dictionary.Add(">=", 7);
                    dictionary.Add("<=", 8);
                    <>f__switch$map8 = dictionary;
                }
                if (<>f__switch$map8.TryGetValue(name, out num))
                {
                    switch (num)
                    {
                        case 0:
                            return (value1 * value2);

                        case 1:
                            return (value1 / ((float) value2));

                        case 2:
                            return (value1 % ((float) value2));

                        case 3:
                            return (value1 + value2);

                        case 4:
                            return (value1 - value2);

                        case 5:
                            return (value1 > value2);

                        case 6:
                            return (value1 < value2);

                        case 7:
                            return (value1 >= value2);

                        case 8:
                            return (value1 <= value2);
                    }
                }
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryNumberSub(float value1, ExpressionToken token, float value2)
        {
            string name = token.name;
            if (name != null)
            {
                int num;
                if (<>f__switch$map9 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(9);
                    dictionary.Add("*", 0);
                    dictionary.Add("/", 1);
                    dictionary.Add("%", 2);
                    dictionary.Add("+", 3);
                    dictionary.Add("-", 4);
                    dictionary.Add(">", 5);
                    dictionary.Add("<", 6);
                    dictionary.Add(">=", 7);
                    dictionary.Add("<=", 8);
                    <>f__switch$map9 = dictionary;
                }
                if (<>f__switch$map9.TryGetValue(name, out num))
                {
                    switch (num)
                    {
                        case 0:
                            return (value1 * value2);

                        case 1:
                            return (value1 / value2);

                        case 2:
                            return (value1 % value2);

                        case 3:
                            return (value1 + value2);

                        case 4:
                            return (value1 - value2);

                        case 5:
                            return (value1 > value2);

                        case 6:
                            return (value1 < value2);

                        case 7:
                            return (value1 >= value2);

                        case 8:
                            return (value1 <= value2);
                    }
                }
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryNumberSub(float value1, ExpressionToken token, string value2)
        {
            string name = token.name;
            if ((name != null) && (name == "+"))
            {
                return (value1 + value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryNumberSub(string value1, ExpressionToken token, bool value2)
        {
            string name = token.name;
            if ((name != null) && (name == "+"))
            {
                return (value1 + value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryNumberSub(string value1, ExpressionToken token, int value2)
        {
            string name = token.name;
            if ((name != null) && (name == "+"))
            {
                return (value1 + value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryNumberSub(string value1, ExpressionToken token, float value2)
        {
            string name = token.name;
            if ((name != null) && (name == "+"))
            {
                return (value1 + value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcBinaryNumberSub(string value1, ExpressionToken token, string value2)
        {
            string name = token.name;
            if ((name != null) && (name == "+"))
            {
                return (value1 + value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcSubstition(object value1, ExpressionToken token, object value2)
        {
            if (token.name == "=")
            {
                return value2;
            }
            if (value1 is int)
            {
                if (value2 is int)
                {
                    return CalcSubstitionSub((int) value1, token, (int) value2);
                }
                if (value2 is float)
                {
                    return CalcSubstitionSub((int) value1, token, (float) value2);
                }
                if (value2 is string)
                {
                    return CalcSubstitionSub((int) value1, token, (string) value2);
                }
            }
            else if (value1 is float)
            {
                if (value2 is int)
                {
                    return CalcSubstitionSub((float) value1, token, (int) value2);
                }
                if (value2 is float)
                {
                    return CalcSubstitionSub((float) value1, token, (float) value2);
                }
                if (value2 is string)
                {
                    return CalcSubstitionSub((float) value1, token, (string) value2);
                }
            }
            else if (value1 is string)
            {
                if (value2 is int)
                {
                    return CalcSubstitionSub((string) value1, token, (int) value2);
                }
                if (value2 is float)
                {
                    return CalcSubstitionSub((string) value1, token, (float) value2);
                }
                if (value2 is string)
                {
                    return CalcSubstitionSub((string) value1, token, (string) value2);
                }
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcSubstitionSub(int value1, ExpressionToken token, int value2)
        {
            switch (token.name)
            {
                case "+=":
                    return (value1 + value2);

                case "-=":
                    return (value1 - value2);

                case "*=":
                    return (value1 * value2);

                case "/=":
                    return (value1 / value2);

                case "%=":
                    return (value1 % value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcSubstitionSub(int value1, ExpressionToken token, float value2)
        {
            switch (token.name)
            {
                case "+=":
                    return (value1 + value2);

                case "-=":
                    return (value1 - value2);

                case "*=":
                    return (value1 * value2);

                case "/=":
                    return (((float) value1) / value2);

                case "%=":
                    return (((float) value1) % value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcSubstitionSub(int value1, ExpressionToken token, string value2)
        {
            string name = token.name;
            if ((name != null) && (name == "+="))
            {
                return (value1 + value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcSubstitionSub(float value1, ExpressionToken token, int value2)
        {
            switch (token.name)
            {
                case "+=":
                    return (value1 + value2);

                case "-=":
                    return (value1 - value2);

                case "*=":
                    return (value1 * value2);

                case "/=":
                    return (value1 / ((float) value2));

                case "%=":
                    return (value1 % ((float) value2));
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcSubstitionSub(float value1, ExpressionToken token, float value2)
        {
            switch (token.name)
            {
                case "+=":
                    return (value1 + value2);

                case "-=":
                    return (value1 - value2);

                case "*=":
                    return (value1 * value2);

                case "/=":
                    return (value1 / value2);

                case "%=":
                    return (value1 % value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcSubstitionSub(float value1, ExpressionToken token, string value2)
        {
            string name = token.name;
            if ((name != null) && (name == "+="))
            {
                return (value1 + value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcSubstitionSub(string value1, ExpressionToken token, int value2)
        {
            string name = token.name;
            if ((name != null) && (name == "+="))
            {
                return (value1 + value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcSubstitionSub(string value1, ExpressionToken token, float value2)
        {
            string name = token.name;
            if ((name != null) && (name == "+="))
            {
                return (value1 + value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcSubstitionSub(string value1, ExpressionToken token, string value2)
        {
            string name = token.name;
            if ((name != null) && (name == "+="))
            {
                return (value1 + value2);
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        private static object CalcUnary(object value, ExpressionToken token)
        {
            string name = token.name;
            if (name != null)
            {
                if (name == "!")
                {
                    if (value is bool)
                    {
                        return !((bool) value);
                    }
                    object[] objArray1 = new object[] { token.name };
                    throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, objArray1));
                }
                if (name == "+")
                {
                    if (value is float)
                    {
                        return value;
                    }
                    if (value is int)
                    {
                        return value;
                    }
                    object[] objArray2 = new object[] { token.name };
                    throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, objArray2));
                }
                if (name == "-")
                {
                    if (value is float)
                    {
                        return -((float) value);
                    }
                    if (value is int)
                    {
                        return -((int) value);
                    }
                    object[] objArray3 = new object[] { token.name };
                    throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, objArray3));
                }
            }
            object[] args = new object[] { token.name };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperator, args));
        }

        public static bool CheckSeparator(char c)
        {
            if (!char.IsWhiteSpace(c) && (c != ','))
            {
                switch (c)
                {
                    case '%':
                    case '&':
                    case '(':
                    case ')':
                    case '*':
                    case '+':
                    case ',':
                    case '-':
                    case '/':
                        break;

                    default:
                        switch (c)
                        {
                            case '<':
                            case '=':
                            case '>':
                            case '!':
                            case '|':
                                break;

                            default:
                                return false;
                        }
                        break;
                }
            }
            return true;
        }

        private void Create(string name, bool isAlphabet, TokenType type, int priority, object variable)
        {
            this.name = name;
            this.isAlphabet = isAlphabet;
            this.type = type;
            this.priority = priority;
            this.variable = variable;
        }

        public static ExpressionToken CreateToken(string name)
        {
            int num;
            float num2;
            bool flag;
            string str;
            ExpressionToken token;
            if (name.Length == 0)
            {
                Debug.LogError(" Token is enmpty");
            }
            if (int.TryParse(name, out num))
            {
                return new ExpressionToken(name, false, TokenType.Number, 0, num);
            }
            if (float.TryParse(name, out num2))
            {
                return new ExpressionToken(name, false, TokenType.Number, 0, num2);
            }
            if (bool.TryParse(name, out flag))
            {
                return new ExpressionToken(name, false, TokenType.Number, 0, flag);
            }
            if (TryParseString(name, out str))
            {
                return new ExpressionToken(name, false, TokenType.Number, 0, str);
            }
            if (TryParseFunction(name, out token))
            {
                return token;
            }
            return new ExpressionToken(name, false, TokenType.Value, 0);
        }

        public static ExpressionToken FindOperator(string exp, int index)
        {
            foreach (ExpressionToken token in OperatorArray)
            {
                if ((!token.isAlphabet && (token.name.Length <= (exp.Length - index))) && (exp.IndexOf(token.name, index, token.name.Length) == index))
                {
                    return token;
                }
            }
            return null;
        }

        public static ExpressionToken OperateBinary(ExpressionToken value1, ExpressionToken token, ExpressionToken value2)
        {
            return new ExpressionToken(string.Empty, false, TokenType.Number, 0, CalcBinary(value1.variable, token, value2.variable));
        }

        public static ExpressionToken OperateFunction(ExpressionToken token, ExpressionToken[] args)
        {
            switch (token.name)
            {
                case "Random":
                    return new ExpressionToken(string.Empty, false, TokenType.Number, 0, Random.Range(ExpressionCast.ToInt(args[0].variable), ExpressionCast.ToInt(args[1].variable) + 1));

                case "RandomF":
                    return new ExpressionToken(string.Empty, false, TokenType.Number, 0, Random.Range(ExpressionCast.ToFloat(args[0].variable), ExpressionCast.ToFloat(args[1].variable)));

                case "Ceil":
                    return new ExpressionToken(string.Empty, false, TokenType.Number, 0, Mathf.Ceil(ExpressionCast.ToFloat(args[0].variable)));

                case "CeilToInt":
                    return new ExpressionToken(string.Empty, false, TokenType.Number, 0, Mathf.CeilToInt(ExpressionCast.ToFloat(args[0].variable)));

                case "Floor":
                    return new ExpressionToken(string.Empty, false, TokenType.Number, 0, Mathf.Floor(ExpressionCast.ToFloat(args[0].variable)));

                case "FloorToInt":
                    return new ExpressionToken(string.Empty, false, TokenType.Number, 0, Mathf.FloorToInt(ExpressionCast.ToFloat(args[0].variable)));
            }
            throw new Exception("Unkonw Function :" + token.name);
        }

        public static ExpressionToken OperateSubstition(ExpressionToken value1, ExpressionToken token, ExpressionToken value2, Func<string, object, bool> callbackSetValue)
        {
            value1.variable = CalcSubstition(value1.variable, token, value2.variable);
            if ((value1.type == TokenType.Value) && !callbackSetValue(value1.name, value1.variable))
            {
                object[] args = new object[] { token.name, value1.variable };
                throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.ExpressionOperateSubstition, args));
            }
            return value1;
        }

        public static ExpressionToken OperateUnary(ExpressionToken value, ExpressionToken token)
        {
            return new ExpressionToken(string.Empty, false, TokenType.Number, 0, CalcUnary(value.variable, token));
        }

        private static bool TryParseFunction(string name, out ExpressionToken token)
        {
            if (name != null)
            {
                if ((name == "Random") || (name == "RandomF"))
                {
                    token = new ExpressionToken(name, false, TokenType.Function, 0);
                    token.numFunctionArg = 2;
                    return true;
                }
                if (((name == "Ceil") || (name == "CeilToInt")) || ((name == "Floor") || (name == "FloorToInt")))
                {
                    token = new ExpressionToken(name, false, TokenType.Function, 0);
                    token.numFunctionArg = 1;
                    return true;
                }
            }
            token = null;
            return false;
        }

        private static bool TryParseString(string str, out string outStr)
        {
            outStr = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length < 2)
                {
                    return false;
                }
                if ((str[0] == '"') && (str[str.Length - 1] == '"'))
                {
                    outStr = str.Substring(1, str.Length - 2);
                    return true;
                }
            }
            return false;
        }

        public bool IsValueType
        {
            get
            {
                TokenType type = this.Type;
                if ((type != TokenType.Number) && (type != TokenType.Value))
                {
                    return false;
                }
                return true;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public int NumFunctionArg
        {
            get
            {
                return this.numFunctionArg;
            }
        }

        public int Priority
        {
            get
            {
                return this.priority;
            }
        }

        public TokenType Type
        {
            get
            {
                return this.type;
            }
        }

        public object Variable
        {
            get
            {
                return this.variable;
            }
            set
            {
                this.variable = value;
            }
        }

        public enum TokenType
        {
            Lpa,
            Rpa,
            Comma,
            Unary,
            Binary,
            Substitution,
            Number,
            Value,
            Function
        }
    }
}

