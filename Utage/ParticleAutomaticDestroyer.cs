namespace Utage
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/Effect/ParticleAutomaticDestroyer")]
    public class ParticleAutomaticDestroyer : MonoBehaviour
    {
        private bool CheckPlaying()
        {
            foreach (ParticleSystem system in base.GetComponentsInChildren<ParticleSystem>(true))
            {
                if (system.get_isPlaying())
                {
                    return true;
                }
            }
            return false;
        }

        private void Update()
        {
            if (!this.CheckPlaying())
            {
                Object.Destroy(base.get_gameObject());
            }
        }
    }
}

