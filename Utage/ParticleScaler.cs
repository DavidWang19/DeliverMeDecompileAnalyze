namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/Effect/ParticleScaler")]
    public class ParticleScaler : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <HasChanged>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsInit>k__BackingField;
        [SerializeField]
        private bool changeGravity = true;
        [SerializeField]
        private bool changeRenderMode = true;
        private Dictionary<ParticleSystem, float> defaultGravities = new Dictionary<ParticleSystem, float>();
        [SerializeField, Hide("UseLocalScale")]
        private float scale = 1f;
        [SerializeField]
        private bool useLocalScale;

        private void ChangeSetting()
        {
            foreach (ParticleSystem system in base.GetComponentsInChildren<ParticleSystem>(true))
            {
                this.ChangeSetting(system);
            }
        }

        private void ChangeSetting(ParticleSystem particle)
        {
            ParticleSystem.MainModule module = particle.get_main();
            module.set_scalingMode(0);
            if (particle.get_velocityOverLifetime().get_enabled())
            {
                particle.get_velocityOverLifetime().set_space(0);
            }
            if (particle.get_forceOverLifetime().get_enabled())
            {
                particle.get_forceOverLifetime().set_space(0);
            }
            if (this.ChangeGravity)
            {
                float num;
                if (!this.defaultGravities.TryGetValue(particle, out num))
                {
                    num = module.get_gravityModifier().get_constant();
                    this.defaultGravities.Add(particle, num);
                }
                module.set_gravityModifier((ParticleSystem.MinMaxCurve) (num * base.get_transform().get_lossyScale().y));
            }
            if (this.ChangeRenderMode)
            {
                ParticleSystemRenderer component = particle.GetComponent<ParticleSystemRenderer>();
                if ((component != null) && (component.get_renderMode() == 1))
                {
                    component.set_renderMode(0);
                }
            }
        }

        private void OnValidate()
        {
            this.HasChanged = true;
        }

        private void Start()
        {
            this.HasChanged = true;
        }

        private void Update()
        {
            if (this.HasChanged)
            {
                if (!this.useLocalScale)
                {
                    base.get_transform().set_localScale((Vector3) (this.Scale * Vector3.get_one()));
                }
                this.ChangeSetting();
            }
        }

        public bool ChangeGravity
        {
            get
            {
                return this.changeGravity;
            }
            set
            {
                this.changeGravity = value;
                this.HasChanged = true;
            }
        }

        public bool ChangeRenderMode
        {
            get
            {
                return this.changeRenderMode;
            }
            set
            {
                this.changeRenderMode = value;
                this.HasChanged = true;
            }
        }

        private bool HasChanged { get; set; }

        private bool IsInit { get; set; }

        public float Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;
                this.HasChanged = true;
            }
        }

        public bool UseLocalScale
        {
            get
            {
                return this.useLocalScale;
            }
            set
            {
                this.useLocalScale = value;
                this.HasChanged = true;
            }
        }
    }
}

