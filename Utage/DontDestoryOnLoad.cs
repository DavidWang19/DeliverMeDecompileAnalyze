namespace Utage
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/Other/DontDestoryOnLoad")]
    public class DontDestoryOnLoad : MonoBehaviour
    {
        [SerializeField]
        private bool dontDestoryOnLoad;

        private void Awake()
        {
            if (this.dontDestoryOnLoad)
            {
                Object.DontDestroyOnLoad(base.get_gameObject());
            }
        }
    }
}

