namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class UtagCustomCommand : AdvCustomCommandManager
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map0;
        public ActionManager actionM;
        public CountDownManager countdownM;
        public DateManager dateM;
        public IMManager iMM;
        public InputCanvas inputC;
        public MapManager mapM;
        public PowerManager powerM;

        public void CreateCustomCommand(string id, StringGridRow row, AdvSettingDataManager dataManager, ref AdvCommand command)
        {
            if (id != null)
            {
                int num;
                if (<>f__switch$map0 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(0x12);
                    dictionary.Add("ShowMap", 0);
                    dictionary.Add("HideMap", 1);
                    dictionary.Add("JumpParam", 2);
                    dictionary.Add("ShowAction", 3);
                    dictionary.Add("ShowIM", 4);
                    dictionary.Add("HideIM", 5);
                    dictionary.Add("SetIMContact", 6);
                    dictionary.Add("ClearIM", 7);
                    dictionary.Add("AddIM", 8);
                    dictionary.Add("ShowDate", 9);
                    dictionary.Add("HideDate", 10);
                    dictionary.Add("StartCountdown", 11);
                    dictionary.Add("StopCountdown", 12);
                    dictionary.Add("ShowPower", 13);
                    dictionary.Add("HidePower", 14);
                    dictionary.Add("ShowInput", 15);
                    dictionary.Add("HideInput", 0x10);
                    dictionary.Add("GenerateSeed", 0x11);
                    <>f__switch$map0 = dictionary;
                }
                if (<>f__switch$map0.TryGetValue(id, out num))
                {
                    switch (num)
                    {
                        case 0:
                            command = new AdvCommandShowMap(row, this.mapM);
                            break;

                        case 1:
                            command = new AdvCommandHideMap(row, this.mapM);
                            break;

                        case 2:
                            command = new AdvCommandJumpParam(row, dataManager);
                            break;

                        case 3:
                            command = new AdvCommandShowAction(row, this.actionM);
                            break;

                        case 4:
                            command = new AdvCommandShowIM(row, this.iMM);
                            break;

                        case 5:
                            command = new AdvCommandHideIM(row, this.iMM);
                            break;

                        case 6:
                            command = new AdvCommandSetIMContact(row, this.iMM);
                            break;

                        case 7:
                            command = new AdvCommandClearIM(row, this.iMM);
                            break;

                        case 8:
                            command = new AdvCommandAddIM(row, this.iMM);
                            break;

                        case 9:
                            command = new AdvCommandShowDate(row, this.dateM);
                            break;

                        case 10:
                            command = new AdvCommandHideDate(row, this.dateM);
                            break;

                        case 11:
                            command = new AdvCommmandStartCountdown(row, this.countdownM);
                            break;

                        case 12:
                            command = new AdvCommmandStopCountdown(row, this.countdownM);
                            break;

                        case 13:
                            command = new AdvCommmandShowPower(row, this.powerM);
                            break;

                        case 14:
                            command = new AdvCommmandHidePower(row, this.powerM);
                            break;

                        case 15:
                            command = new AdvCommmandShowInput(row, this.inputC);
                            break;

                        case 0x10:
                            command = new AdvCommmandHideInput(row, this.inputC);
                            break;

                        case 0x11:
                            command = new AdvCommandGenerateSeed(row);
                            break;
                    }
                }
            }
        }

        public override void OnBootInit()
        {
            AdvCommandParser.OnCreateCustomCommandFromID = (AdvCommandParser.CreateCustomCommandFromID) Delegate.Combine(AdvCommandParser.OnCreateCustomCommandFromID, new AdvCommandParser.CreateCustomCommandFromID(this.CreateCustomCommand));
        }

        public override void OnClear()
        {
        }
    }
}

