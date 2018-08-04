namespace Utage
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/System UI/DebugPauseEditor")]
    public class DebugPauseEditor : MonoBehaviour
    {
        public bool isPauseOnMouseDown;
        public bool isPauseOnMouseUp;
        [Range(1E-05f, 10f)]
        public float timeScale = 1f;
    }
}

