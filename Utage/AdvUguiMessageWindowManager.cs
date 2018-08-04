namespace Utage
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Utage/ADV/MessageWindowManager")]
    public class AdvUguiMessageWindowManager : MonoBehaviour
    {
        internal void Close()
        {
            base.get_gameObject().SetActive(false);
        }

        internal void Open()
        {
            base.get_gameObject().SetActive(true);
        }
    }
}

