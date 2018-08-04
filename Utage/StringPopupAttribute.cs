namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class StringPopupAttribute : PropertyAttribute
    {
        private List<string> stringList = new List<string>();

        public StringPopupAttribute(params string[] args)
        {
            this.stringList.AddRange(args);
        }

        public List<string> StringList
        {
            get
            {
                return this.stringList;
            }
        }
    }
}

