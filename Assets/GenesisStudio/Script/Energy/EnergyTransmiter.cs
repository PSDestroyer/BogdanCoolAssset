using System;
using System.Collections;
using System.Globalization;
using UnityEngine;

namespace GenesisStudio
{
    public abstract class EnergyTransmiter : MonoBehaviour, IInteractable
    {
        protected EnergyBlock energyBlock;
        public OnOffVizual Vizual;
        protected bool on;
        public bool CanInteract { get; set; } = true;
        [field: SerializeField] public bool Hold { get; set; }
        [field: SerializeField] public float HoldTime { get; set; }

        protected virtual void Start()
        {
            Vizual.Initialize();
            Vizual.ToggleMesh(on);
            if(energyBlock != null)
                energyBlock.Energy = on;
        }

        public virtual void Interact(object sender)
        {
            on = !on;
            Vizual.ToggleMesh(on);
            if(energyBlock != null)
                energyBlock.Energy = on;
        }
    }

    [Serializable]
    public class OnOffVizual
    {
        [SerializeField] private MeshRenderer mesh_on;
        [SerializeField] private MeshRenderer mesh_off;
        
        Material m_on;
        Material m_off;

        public void Initialize()
        {
            m_on = Resources.Load<Material>("Materials/ON");
            m_off = Resources.Load<Material>("Materials/OFF");
        }
        
        public void ToggleMesh(bool value)
        {
            if (mesh_on == null || mesh_off == null) return;
            if (value)
            {
                mesh_off.material.DisableKeyword("_EMISSION");
                mesh_on.material.EnableKeyword("_EMISSION");
            }
            else
            {
                mesh_off.material.EnableKeyword("_EMISSION");
                mesh_on.material.DisableKeyword("_EMISSION");
            }
        }
    }
}
