namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Utage/ADV/Internal/GraphicObject/Particle")]
    public class AdvGraphicObjectParticle : AdvGraphicObjectPrefabBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ParticleSystem[] <ParticleArray>k__BackingField;

        protected override void ChangeResourceOnDrawSub(AdvGraphicInfo grapic)
        {
            this.SetSortingOrder(base.Layer.Canvas.get_sortingOrder(), base.Layer.Canvas.get_sortingLayerName());
        }

        private void FadInOut(ParticleSystem particle, float alpha)
        {
            ParticleSystem.MainModule module = particle.get_main();
            Color color = module.get_startColor().get_color();
            color.a = alpha;
            module.set_startColor(color);
        }

        public override void Init(AdvGraphicObject parentObject)
        {
            base.Init(parentObject);
            parentObject.get_gameObject().AddComponent<ParticleAutomaticDestroyer>();
        }

        internal override void OnEffectColorsChange(AdvEffectColor color)
        {
            if (base.currentObject != null)
            {
            }
        }

        protected void SetSortingOrder(int sortingOrder, string sortingLayerName)
        {
            this.ParticleArray = base.currentObject.GetComponentsInChildren<ParticleSystem>(true);
            foreach (ParticleSystem system in this.ParticleArray)
            {
                Renderer component = system.GetComponent<Renderer>();
                component.set_sortingOrder(component.get_sortingOrder() + sortingOrder);
                component.set_sortingLayerName(component.get_sortingLayerName() + sortingLayerName);
            }
        }

        protected ParticleSystem[] ParticleArray { get; set; }
    }
}

