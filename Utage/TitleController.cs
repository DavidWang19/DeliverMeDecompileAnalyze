namespace Utage
{
    using System;
    using UnityEngine;

    public class TitleController : MonoBehaviour
    {
        public void OnTitleAnimationFinish()
        {
            SystemUi.GetInstance().IsEnableTitleAniamtion = false;
        }

        public void OnTitleAnimationStart()
        {
            if (!SystemUi.GetInstance().IsEnableTitleAniamtion)
            {
                base.GetComponent<Animator>().Play("fadeIn", 0, 1f);
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

