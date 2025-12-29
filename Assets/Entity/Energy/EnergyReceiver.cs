using System;
using UnityEngine;

namespace GenesisStudio
{
    public class EnergyReceiver : MonoBehaviour, IInteractable
    {
        [NonSerialized] public EnergyBlock energy;
        public OnOffVizual Vizual;
        private bool on;
        [SerializeField] private Lamp[] lamps;
        public bool CanInteract { get; set; } = true;
        public bool hasEnergy
        {
            get => energy.Energy && energy != null;
            set
            {
                if(value == false)
                {
                    on = false;
                    for (int i = 0; i < lamps.Length; i++)
                    {
                        lamps[i].On = false;
                    }
                }
                energy.Energy = value;
                Vizual.ToggleMesh(on);
            }
        }

        [field: SerializeField] public bool Hold { get; set; }
        [field: SerializeField] public float HoldTime { get; set; }

        private void Start()
        {
            on = false;
            for (int i = 0; i < lamps.Length; i++)
            {
                lamps[i].On = on;
            }
            Vizual.Initialize();
            Vizual.ToggleMesh(on);
        }
        public void Interact(object sender)
        {
            if(energy == null) return;
            if(!hasEnergy) return;
            on = !on;
            for (int i = 0; i < lamps.Length; i++)
            {
                lamps[i].On = on;
            }
            Vizual.ToggleMesh(on);
        }
    }
}