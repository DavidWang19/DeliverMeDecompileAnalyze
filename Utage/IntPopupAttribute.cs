namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class IntPopupAttribute : PropertyAttribute
    {
        private List<int> popupList = new List<int>();

        public IntPopupAttribute(params int[] args)
        {
            this.popupList.AddRange(args);
        }

        public List<int> PopupList
        {
            get
            {
                return this.popupList;
            }
        }
    }
}

