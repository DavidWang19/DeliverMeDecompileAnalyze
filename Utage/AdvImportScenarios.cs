namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AdvImportScenarios : ScriptableObject
    {
        [SerializeField]
        private List<AdvChapterData> chapters = new List<AdvChapterData>();
        [SerializeField]
        private int importVersion;
        private const int Version = 3;

        public void AddChapter(AdvChapterData chapterData)
        {
            this.Chapters.Add(chapterData);
        }

        public bool CheckVersion()
        {
            return (this.importVersion == 3);
        }

        public bool TryAddChapter(AdvChapterData chapterData)
        {
            <TryAddChapter>c__AnonStorey0 storey = new <TryAddChapter>c__AnonStorey0 {
                chapterData = chapterData
            };
            if (this.Chapters.Exists(new Predicate<AdvChapterData>(storey.<>m__0)))
            {
                return false;
            }
            this.Chapters.Add(storey.chapterData);
            return true;
        }

        public List<AdvChapterData> Chapters
        {
            get
            {
                return this.chapters;
            }
        }

        [CompilerGenerated]
        private sealed class <TryAddChapter>c__AnonStorey0
        {
            internal AdvChapterData chapterData;

            internal bool <>m__0(AdvChapterData x)
            {
                return (x.get_name() == this.chapterData.get_name());
            }
        }
    }
}

