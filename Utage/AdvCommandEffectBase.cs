namespace Utage
{
    using System;
    using UnityEngine;

    public abstract class AdvCommandEffectBase : AdvCommandWaitBase
    {
        protected string targetName;
        protected AdvEffectManager.TargetType targetType;

        protected AdvCommandEffectBase(StringGridRow row) : base(row)
        {
            this.OnParse();
        }

        protected virtual void OnParse()
        {
            this.ParseEffectTarget(AdvColumnName.Arg1);
            this.ParseWait(AdvColumnName.WaitType);
        }

        protected override void OnStart(AdvEngine engine, AdvScenarioThread thread)
        {
            GameObject target = engine.EffectManager.FindTarget(this);
            if (target == null)
            {
                Debug.LogError(base.RowData.ToErrorString(this.TargetName + " is not found"));
                this.OnComplete(thread);
            }
            else
            {
                this.OnStartEffect(target, engine, thread);
            }
        }

        protected abstract void OnStartEffect(GameObject target, AdvEngine engine, AdvScenarioThread thread);
        protected virtual void ParseEffectTarget(AdvColumnName columnName)
        {
            this.targetName = base.ParseCell<string>(columnName);
            if (!ParserUtil.TryParaseEnum<AdvEffectManager.TargetType>(this.targetName, out this.targetType))
            {
                this.targetType = AdvEffectManager.TargetType.Default;
            }
        }

        protected virtual void ParseWait(AdvColumnName columnName)
        {
            if (base.IsEmptyCell(columnName))
            {
                base.WaitType = AdvCommandWaitType.ThisAndAdd;
            }
            else
            {
                AdvCommandWaitType type;
                if (!ParserUtil.TryParaseEnum<AdvCommandWaitType>(base.ParseCell<string>(columnName), out type))
                {
                    base.WaitType = AdvCommandWaitType.NoWait;
                    Debug.LogError(base.ToErrorString("UNKNOWN WaitType"));
                }
                else
                {
                    base.WaitType = type;
                }
            }
        }

        public AdvEffectManager.TargetType Target
        {
            get
            {
                return this.targetType;
            }
        }

        public string TargetName
        {
            get
            {
                return this.targetName;
            }
        }
    }
}

