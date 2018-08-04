namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/UI/NovelTextBrPageIcon")]
    public class UguiNovelTextBrPageIcon : Text
    {
        public UguiNovelText novelText;

        private void Update()
        {
            Vector2 endPosition = this.novelText.EndPosition;
            base.get_gameObject().get_transform().set_localPosition(endPosition);
        }
    }
}

