namespace Utage
{
    using System;

    internal class AdvIfData
    {
        private bool isIf;
        private bool isSkpping;
        private AdvIfData parent;

        public void BeginIf(AdvParamManager param, ExpressionParser exp)
        {
            this.isIf = param.CalcExpressionBoolean(exp);
            this.isSkpping = !this.isIf;
        }

        public void Else()
        {
            if (!this.isIf)
            {
                this.isIf = true;
                this.isSkpping = false;
            }
            else
            {
                this.isSkpping = true;
            }
        }

        public void ElseIf(AdvParamManager param, ExpressionParser exp)
        {
            if (!this.isIf)
            {
                this.isIf = param.CalcExpressionBoolean(exp);
                this.isSkpping = !this.isIf;
            }
            else
            {
                this.isSkpping = true;
            }
        }

        public void EndIf()
        {
            this.isSkpping = false;
        }

        public bool IsSkpping
        {
            get
            {
                return this.isSkpping;
            }
            set
            {
                this.isSkpping = value;
            }
        }

        public AdvIfData Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = value;
            }
        }
    }
}

