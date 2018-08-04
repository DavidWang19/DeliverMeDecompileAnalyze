namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AdvCommandText : AdvCommand
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <IndexPageData>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsNextBr>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsPageEnd>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvPageControllerType <PageCtrlType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvScenarioPageData <PageData>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AssetFile <VoiceFile>k__BackingField;

        public AdvCommandText(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            string str = base.ParseCellOptional<string>(AdvColumnName.Voice, string.Empty);
            if (!string.IsNullOrEmpty(str) && !AdvCommand.IsEditorErrorCheck)
            {
                this.VoiceFile = base.AddLoadFile(dataManager.BootSetting.GetLocalizeVoiceFilePath(str), new AdvVoiceSetting(base.RowData));
            }
            this.PageCtrlType = base.ParseCellOptional<AdvPageControllerType>(AdvColumnName.PageCtrl, AdvPageControllerType.InputBrPage);
            this.IsNextBr = AdvPageController.IsBrType(this.PageCtrlType);
            this.IsPageEnd = AdvPageController.IsPageEndType(this.PageCtrlType);
            if (AdvCommand.IsEditorErrorCheck)
            {
                TextData data = new TextData(base.ParseCellLocalizedText());
                if (!string.IsNullOrEmpty(data.ErrorMsg))
                {
                    Debug.LogError(base.ToErrorString(data.ErrorMsg));
                }
            }
        }

        public override void DoCommand(AdvEngine engine)
        {
            if (base.IsEmptyCell(AdvColumnName.Arg1))
            {
                engine.Page.CharacterInfo = null;
            }
            if ((this.VoiceFile != null) && (!engine.Page.CheckSkip() || !engine.Config.SkipVoiceAndSe))
            {
                engine.SoundManager.PlayVoice(engine.Page.CharacterLabel, this.VoiceFile);
            }
            engine.Page.UpdatePageTextData(this);
        }

        public override void InitFromPageData(AdvScenarioPageData pageData)
        {
            this.PageData = pageData;
            this.IndexPageData = this.PageData.TextDataList.Count;
            this.PageData.AddTextData(this);
            this.PageData.InitMessageWindowName(this, base.ParseCellOptional<string>(AdvColumnName.WindowType, string.Empty));
        }

        internal void InitOnCreateEntity(AdvCommandText original)
        {
            this.PageData = original.PageData;
            this.PageData.ChangeTextDataOnCreateEntity(original.IndexPageData, this);
        }

        public override bool IsTypePage()
        {
            return true;
        }

        public override bool IsTypePageEnd()
        {
            return this.IsPageEnd;
        }

        public override bool Wait(AdvEngine engine)
        {
            return engine.Page.IsWaitTextCommand;
        }

        private int IndexPageData { get; set; }

        public bool IsNextBr { get; private set; }

        public bool IsPageEnd { get; private set; }

        public AdvPageControllerType PageCtrlType { get; private set; }

        private AdvScenarioPageData PageData { get; set; }

        public AssetFile VoiceFile { get; private set; }
    }
}

