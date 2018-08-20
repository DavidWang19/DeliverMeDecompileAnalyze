namespace Utage
{
    using System;
    using UnityEngine;

    public class PowerManager : MonoBehaviour
    {
        private CanvasGroup canvas;
        private int currentPower;
        public AdvEngine Engine;
        private bool show;

        private void Display()
        {
            bool parameterBoolean = this.Engine.Param.GetParameterBoolean("DisplayPower");
            Debug.Log("Show:" + parameterBoolean);
            if (parameterBoolean)
            {
                this.Show();
            }
            else
            {
                this.Hide();
            }
        }

        public void Hide()
        {
            this.Reset();
            this.canvas.set_alpha(0f);
            this.show = false;
            this.Engine.Param.SetParameterBoolean("DisplayPower", this.show);
        }

        private void OnEnable()
        {
            this.canvas = base.get_transform().GetComponent<CanvasGroup>();
            this.Display();
        }

        private void Reset()
        {
            for (int i = 0; i < 10; i++)
            {
                base.get_transform().Find("Image" + i).GetComponent<Image>().CrossFadeAlpha(0f, 0f, true);
            }
        }

        public void Show()
        {
            this.Reset();
            this.canvas.set_alpha(1f);
            this.show = true;
            this.Engine.Param.SetParameterBoolean("DisplayPower", this.show);
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (this.show)
            {
                Debug.Log("Show power: " + this.show);
                int parameterInt = this.Engine.Param.GetParameterInt("power");
                if (parameterInt != this.currentPower)
                {
                    if (parameterInt > this.currentPower)
                    {
                        for (int i = this.currentPower; i < parameterInt; i++)
                        {
                            base.get_transform().Find("Image" + i).GetComponent<Image>().CrossFadeAlpha(1f, 1f, false);
                        }
                    }
                    else
                    {
                        for (int j = parameterInt; j < this.currentPower; j++)
                        {
                            base.get_transform().Find("Image" + j).GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
                        }
                    }
                    this.currentPower = parameterInt;
                }
            }
        }
    }
}

