namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/Extra/SelectionAutomatic")]
    public class AdvAgingTest : MonoBehaviour
    {
        public bool clearOnEnd = true;
        [SerializeField]
        private bool disable;
        [SerializeField]
        protected AdvEngine engine;
        private Dictionary<AdvScenarioPageData, int> selectedDictionary = new Dictionary<AdvScenarioPageData, int>();
        [SerializeField, EnumFlags]
        private SkipFlags skipFilter;
        private float time;
        [SerializeField]
        private Type type;
        public float waitTime = 1f;

        private void Awake()
        {
            this.Engine.SelectionManager.OnBeginWaitInput.AddListener(new UnityAction<AdvSelectionManager>(this, (IntPtr) this.OnBeginWaitInput));
            this.Engine.SelectionManager.OnUpdateWaitInput.AddListener(new UnityAction<AdvSelectionManager>(this, (IntPtr) this.OnUpdateWaitInput));
            this.Engine.ScenarioPlayer.OnBeginCommand.AddListener(new UnityAction<AdvCommand>(this, (IntPtr) this.OnBeginCommand));
            this.Engine.ScenarioPlayer.OnUpdatePreWaitingCommand.AddListener(new UnityAction<AdvCommand>(this, (IntPtr) this.OnUpdatePreWaitingCommand));
            this.Engine.ScenarioPlayer.OnEndScenario.AddListener(new UnityAction<AdvScenarioPlayer>(this, (IntPtr) this.OnEndScenario));
        }

        private int GetIndex(AdvSelectionManager selection)
        {
            if (this.type != Type.DepthFirst)
            {
                return Random.Range(0, selection.TotalCount);
            }
            return this.GetIndexDepthFirst(selection);
        }

        private int GetIndexDepthFirst(AdvSelectionManager selection)
        {
            int num;
            if (!this.selectedDictionary.TryGetValue(this.Engine.Page.CurrentData, out num))
            {
                num = 0;
                this.selectedDictionary.Add(this.Engine.Page.CurrentData, num);
                return num;
            }
            if ((num + 1) < selection.TotalCount)
            {
                num++;
            }
            this.selectedDictionary[this.Engine.Page.CurrentData] = num;
            return num;
        }

        private bool IsWaitInputCommand(AdvCommand command)
        {
            if (command is AdvCommandWaitInput)
            {
                return true;
            }
            if (command is AdvCommandSendMessage)
            {
                return true;
            }
            if (command is AdvCommandMovie)
            {
                return ((this.skipFilter & SkipFlags.Movie) == SkipFlags.Movie);
            }
            return ((command is AdvCommandText) && ((this.skipFilter & SkipFlags.Voice) == SkipFlags.Voice));
        }

        private void OnBeginCommand(AdvCommand command)
        {
            this.time = -Time.get_deltaTime();
        }

        private void OnBeginWaitInput(AdvSelectionManager selection)
        {
            this.time = -Time.get_deltaTime();
        }

        private void OnEndScenario(AdvScenarioPlayer player)
        {
            if (this.clearOnEnd)
            {
                this.selectedDictionary.Clear();
            }
        }

        private void OnUpdatePreWaitingCommand(AdvCommand command)
        {
            if (!this.Disable && this.IsWaitInputCommand(command))
            {
                this.time += Time.get_deltaTime();
                if (this.time >= this.waitTime)
                {
                    if (command is AdvCommandWaitInput)
                    {
                        this.Engine.UiManager.IsInputTrig = true;
                    }
                    if (command is AdvCommandSendMessage)
                    {
                        this.engine.ScenarioPlayer.SendMessageTarget.SafeSendMessage("OnAgingInput", command, false);
                    }
                    if (command is AdvCommandMovie)
                    {
                        this.Engine.UiManager.IsInputTrig = true;
                    }
                    if ((command is AdvCommandText) && this.Engine.SoundManager.IsPlayingVoice())
                    {
                        this.Engine.Page.InputSendMessage();
                    }
                }
            }
        }

        private void OnUpdateWaitInput(AdvSelectionManager selection)
        {
            if (!this.Disable)
            {
                this.time += Time.get_deltaTime();
                if (this.time >= this.waitTime)
                {
                    selection.SelectWithTotalIndex(this.GetIndex(selection));
                }
            }
        }

        public bool Disable
        {
            get
            {
                return this.disable;
            }
            set
            {
                this.disable = value;
            }
        }

        public AdvEngine Engine
        {
            get
            {
                if (this.engine == null)
                {
                }
                return (this.engine = Object.FindObjectOfType<AdvEngine>());
            }
        }

        [Flags]
        private enum SkipFlags
        {
            Movie = 2,
            Voice = 1
        }

        public enum Type
        {
            Random,
            DepthFirst
        }
    }
}

