namespace Utage
{
    using System;
    using UnityEngine;

    public class TitleController : MonoBehaviour
    {
        public AdvEngine engine;

        private void OnEnable()
        {
            bool parameterBoolean = this.engine.Param.GetParameterBoolean("Extra");
            if (SystemUi.GetInstance().IsEnableTitleAniamtion)
            {
                if (parameterBoolean)
                {
                    base.GetComponent<Animator>().Play("fadeInWithExtra");
                }
                else
                {
                    base.GetComponent<Animator>().Play("fadeInNoExtra");
                }
            }
        }

        public void OnTitleAnimationFinish()
        {
            SystemUi.GetInstance().IsEnableTitleAniamtion = false;
        }

        public void OnTitleAnimationStart()
        {
            int num = !SystemUi.GetInstance().IsEnableTitleAniamtion ? 1 : 0;
            if (!SystemUi.GetInstance().IsEnableTitleAniamtion)
            {
                if (this.engine.Param.GetParameterBoolean("Extra"))
                {
                    base.GetComponent<Animator>().Play("fadeInWithExtra", 0, (float) num);
                }
                else
                {
                    base.GetComponent<Animator>().Play("fadeInNoExtra", 0, (float) num);
                }
            }
        }

        private void Start()
        {
        }

        private void Update()
        {
        }
    }
}

