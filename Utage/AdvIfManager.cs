namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvIfManager
    {
        private AdvIfData current;
        private bool isLoadInit;

        public void BeginIf(AdvParamManager param, ExpressionParser exp)
        {
            this.IsLoadInit = false;
            AdvIfData data = new AdvIfData();
            if (this.current != null)
            {
                data.Parent = this.current;
            }
            this.current = data;
            this.current.BeginIf(param, exp);
        }

        public bool CheckSkip(AdvCommand command)
        {
            if (command == null)
            {
                return false;
            }
            if (this.current == null)
            {
                return false;
            }
            return (this.current.IsSkpping && !command.IsIfCommand);
        }

        public void Clear()
        {
            this.current = null;
        }

        public void Else()
        {
            if (this.current == null)
            {
                if (!this.IsLoadInit)
                {
                    Debug.LogError(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.Else, new object[0]));
                }
                this.current = new AdvIfData();
            }
            this.current.Else();
        }

        public void ElseIf(AdvParamManager param, ExpressionParser exp)
        {
            if (this.current == null)
            {
                if (!this.IsLoadInit)
                {
                    object[] args = new object[] { exp };
                    Debug.LogError(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.ElseIf, args));
                }
                this.current = new AdvIfData();
            }
            this.current.ElseIf(param, exp);
        }

        public void EndIf()
        {
            if (this.current == null)
            {
                if (!this.IsLoadInit)
                {
                    Debug.LogError(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.EndIf, new object[0]));
                }
                this.current = new AdvIfData();
            }
            this.current.EndIf();
            this.current = this.current.Parent;
        }

        public bool IsLoadInit
        {
            get
            {
                return this.isLoadInit;
            }
            set
            {
                this.isLoadInit = value;
            }
        }
    }
}

