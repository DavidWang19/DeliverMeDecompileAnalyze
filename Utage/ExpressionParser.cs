namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ExpressionParser
    {
        private string errorMsg;
        private string exp;
        private List<ExpressionToken> tokens;

        public ExpressionParser(string exp, Func<string, object> callbackGetValue, Func<string, object, bool> callbackCheckSetValue)
        {
            this.Create(exp, callbackGetValue, callbackCheckSetValue, false);
        }

        public ExpressionParser(string exp, Func<string, object> callbackGetValue, Func<string, object, bool> callbackCheckSetValue, bool isBoolean)
        {
            this.Create(exp, callbackGetValue, callbackCheckSetValue, isBoolean);
        }

        private void AddErrorMsg(string msg)
        {
            if (string.IsNullOrEmpty(this.errorMsg))
            {
                this.errorMsg = string.Empty;
            }
            else
            {
                this.errorMsg = this.errorMsg + "\n";
            }
            this.errorMsg = this.errorMsg + msg;
        }

        private object Calc(Func<string, object, bool> callbackSetValue)
        {
            try
            {
                Stack<ExpressionToken> stack = new Stack<ExpressionToken>();
                foreach (ExpressionToken token3 in this.tokens)
                {
                    ExpressionToken token;
                    ExpressionToken token2;
                    int numFunctionArg;
                    ExpressionToken[] tokenArray;
                    int num2;
                    switch (token3.Type)
                    {
                        case ExpressionToken.TokenType.Unary:
                        {
                            stack.Push(ExpressionToken.OperateUnary(stack.Pop(), token3));
                            continue;
                        }
                        case ExpressionToken.TokenType.Binary:
                        {
                            token2 = stack.Pop();
                            token = stack.Pop();
                            stack.Push(ExpressionToken.OperateBinary(token, token3, token2));
                            continue;
                        }
                        case ExpressionToken.TokenType.Substitution:
                        {
                            token2 = stack.Pop();
                            token = stack.Pop();
                            stack.Push(ExpressionToken.OperateSubstition(token, token3, token2, callbackSetValue));
                            continue;
                        }
                        case ExpressionToken.TokenType.Number:
                        case ExpressionToken.TokenType.Value:
                        {
                            stack.Push(token3);
                            continue;
                        }
                        case ExpressionToken.TokenType.Function:
                            numFunctionArg = token3.NumFunctionArg;
                            tokenArray = new ExpressionToken[numFunctionArg];
                            num2 = 0;
                            goto Label_00E3;

                        default:
                        {
                            continue;
                        }
                    }
                Label_00CD:
                    tokenArray[(numFunctionArg - num2) - 1] = stack.Pop();
                    num2++;
                Label_00E3:
                    if (num2 < numFunctionArg)
                    {
                        goto Label_00CD;
                    }
                    stack.Push(ExpressionToken.OperateFunction(token3, tokenArray));
                }
                if (stack.Count != 1)
                {
                    throw new Exception(LanguageErrorMsg.LocalizeTextFormat(Utage.ErrorMsg.ExpIllegal, new object[0]));
                }
                return stack.Peek().Variable;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message + exception.StackTrace);
                this.AddErrorMsg(exception.Message);
                return null;
            }
        }

        public object CalcExp(Func<string, object> callbackGetValue, Func<string, object, bool> callbackSetValue)
        {
            bool flag = false;
            foreach (ExpressionToken token in this.tokens)
            {
                if (token.Type == ExpressionToken.TokenType.Value)
                {
                    object obj2 = callbackGetValue(token.Name);
                    if (obj2 == null)
                    {
                        object[] args = new object[] { token.Name };
                        this.AddErrorMsg(LanguageErrorMsg.LocalizeTextFormat(Utage.ErrorMsg.ExpUnknownParameter, args));
                        flag = true;
                    }
                    else
                    {
                        token.Variable = obj2;
                    }
                }
            }
            if (!flag)
            {
                return this.Calc(callbackSetValue);
            }
            return null;
        }

        public bool CalcExpBoolean(Func<string, object> callbackGetValue, Func<string, object, bool> callbackSetValue)
        {
            object obj2 = this.CalcExp(callbackGetValue, callbackSetValue);
            if ((obj2 != null) && (obj2.GetType() == typeof(bool)))
            {
                return (bool) obj2;
            }
            this.AddErrorMsg(LanguageErrorMsg.LocalizeTextFormat(Utage.ErrorMsg.ExpResultNotBool, new object[0]));
            return false;
        }

        private static bool CheckStringSeparate(char c, string strToken)
        {
            if ((strToken.Length > 0) && (strToken[0] == '"'))
            {
                return false;
            }
            return true;
        }

        private bool CheckTokenCount(List<ExpressionToken> tokenArray)
        {
            int num = 0;
            foreach (ExpressionToken token in tokenArray)
            {
                switch (token.Type)
                {
                    case ExpressionToken.TokenType.Binary:
                    case ExpressionToken.TokenType.Substitution:
                        num--;
                        break;

                    case ExpressionToken.TokenType.Number:
                    case ExpressionToken.TokenType.Value:
                        num++;
                        break;

                    case ExpressionToken.TokenType.Function:
                        num += 1 - token.NumFunctionArg;
                        break;
                }
            }
            if (num != 1)
            {
                Debug.LogError(num);
            }
            return (num == 1);
        }

        private void Create(string exp, Func<string, object> callbackGetValue, Func<string, object, bool> callbackCheckSetValue, bool isBoolean)
        {
            this.exp = exp;
            this.tokens = this.ToReversePolishNotation(exp);
            if (string.IsNullOrEmpty(this.ErrorMsg))
            {
                if (isBoolean)
                {
                    this.CalcExpBoolean(callbackGetValue, callbackCheckSetValue);
                }
                else
                {
                    this.CalcExp(callbackGetValue, callbackCheckSetValue);
                }
            }
        }

        private static bool SkipGroup(char begin, char end, ref string strToken, string exp, ref int index)
        {
            strToken = strToken + begin;
            index++;
            while (index < exp.Length)
            {
                char ch = exp[index];
                if (ch != end)
                {
                    strToken = strToken + ch;
                }
                else if (strToken[strToken.Length - 1] == '\\')
                {
                    strToken = strToken.Remove(strToken.Length - 1) + ch;
                }
                else
                {
                    strToken = strToken + ch;
                    index++;
                    return true;
                }
                index++;
            }
            return false;
        }

        private static List<ExpressionToken> SplitToken(string exp)
        {
            List<ExpressionToken> list = new List<ExpressionToken> {
                ExpressionToken.LpaToken
            };
            int index = 0;
            string strToken = string.Empty;
            while (index < exp.Length)
            {
                char c = exp[index];
                bool flag = false;
                switch (c)
                {
                    case '"':
                        SkipGroup('"', '"', ref strToken, exp, ref index);
                        flag = true;
                        list.Add(ExpressionToken.CreateToken(strToken));
                        strToken = string.Empty;
                        break;

                    case '[':
                        SkipGroup('[', ']', ref strToken, exp, ref index);
                        flag = true;
                        break;
                }
                if (!flag)
                {
                    if (char.IsWhiteSpace(c))
                    {
                        if (!string.IsNullOrEmpty(strToken))
                        {
                            list.Add(ExpressionToken.CreateToken(strToken));
                        }
                        strToken = string.Empty;
                        index++;
                        continue;
                    }
                    ExpressionToken item = ExpressionToken.FindOperator(exp, index);
                    if (item == null)
                    {
                        strToken = strToken + c;
                        index++;
                        continue;
                    }
                    if (!string.IsNullOrEmpty(strToken))
                    {
                        ExpressionToken token2 = ExpressionToken.CreateToken(strToken);
                        list.Add(token2);
                    }
                    bool flag2 = (list.Count > 0) && list[list.Count - 1].IsValueType;
                    if (!flag2 && (item.Name == "-"))
                    {
                        list.Add(ExpressionToken.UniMinus);
                    }
                    else if (!flag2 && (item.Name == "+"))
                    {
                        list.Add(ExpressionToken.UniPlus);
                    }
                    else
                    {
                        list.Add(item);
                    }
                    strToken = string.Empty;
                    index += item.Name.Length;
                }
            }
            if (!string.IsNullOrEmpty(strToken))
            {
                list.Add(ExpressionToken.CreateToken(strToken));
            }
            list.Add(ExpressionToken.RpaToken);
            return list;
        }

        private List<ExpressionToken> ToReversePolishNotation(string exp)
        {
            List<ExpressionToken> tokenArray = SplitToken(exp);
            if (!this.CheckTokenCount(tokenArray))
            {
                this.AddErrorMsg(LanguageErrorMsg.LocalizeTextFormat(Utage.ErrorMsg.ExpIllegal, new object[0]));
            }
            return this.ToReversePolishNotationSub(tokenArray);
        }

        private List<ExpressionToken> ToReversePolishNotationSub(List<ExpressionToken> tokens)
        {
            List<ExpressionToken> list = new List<ExpressionToken>();
            Stack<ExpressionToken> stack = new Stack<ExpressionToken>();
            foreach (ExpressionToken token in tokens)
            {
                try
                {
                    object[] objArray1;
                    ExpressionToken token3;
                    switch (token.Type)
                    {
                        case ExpressionToken.TokenType.Lpa:
                        {
                            stack.Push(token);
                            continue;
                        }
                        case ExpressionToken.TokenType.Rpa:
                            goto Label_0095;

                        case ExpressionToken.TokenType.Comma:
                            goto Label_010F;

                        case ExpressionToken.TokenType.Unary:
                        case ExpressionToken.TokenType.Binary:
                        case ExpressionToken.TokenType.Substitution:
                        case ExpressionToken.TokenType.Function:
                            token3 = stack.Peek();
                            goto Label_00DA;

                        case ExpressionToken.TokenType.Number:
                        case ExpressionToken.TokenType.Value:
                        {
                            list.Add(token);
                            continue;
                        }
                        default:
                            goto Label_013E;
                    }
                Label_0069:
                    if (stack.Peek().Type == ExpressionToken.TokenType.Lpa)
                    {
                        stack.Pop();
                        continue;
                    }
                    list.Add(stack.Pop());
                Label_0095:
                    if (stack.Count != 0)
                    {
                        goto Label_0069;
                    }
                    continue;
                Label_00B2:
                    if (token3.Type == ExpressionToken.TokenType.Lpa)
                    {
                        goto Label_00F7;
                    }
                    list.Add(token3);
                    stack.Pop();
                    token3 = stack.Peek();
                Label_00DA:
                    if ((stack.Count != 0) && (token.Priority > token3.Priority))
                    {
                        goto Label_00B2;
                    }
                Label_00F7:
                    stack.Push(token);
                    continue;
                Label_010F:
                    if (stack.Peek().Type == ExpressionToken.TokenType.Lpa)
                    {
                        continue;
                    }
                    list.Add(stack.Pop());
                    goto Label_010F;
                Label_013E:
                    objArray1 = new object[] { token.Type.ToString() };
                    this.AddErrorMsg(LanguageErrorMsg.LocalizeTextFormat(Utage.ErrorMsg.UnknownType, objArray1));
                }
                catch (Exception exception)
                {
                    this.AddErrorMsg(exception.ToString());
                }
            }
            return list;
        }

        public string ErrorMsg
        {
            get
            {
                return this.errorMsg;
            }
        }

        public string Exp
        {
            get
            {
                return this.exp;
            }
        }
    }
}

