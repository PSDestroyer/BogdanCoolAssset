using UnityEngine;

namespace GenesisStudio
{
    public class EnergyBlock : MonoBehaviour, IMessage
    {
        private bool energy;
        public bool Energy
        {
            get => energy;
            set
            {
                if(energy == value) return;
                energy = value;
                UpdateReceivers();
            }
        }

        [field: SerializeField] public EnergyReceiver[] EnergyReceivers { get; private set; }

        public string Status => Energy.ToString();

        public void UpdateReceivers()
        {
            if(EnergyReceivers.Length == 0) return;
            
            if (energy)
            {
                for (int i = 0; i < EnergyReceivers.Length; i++)
                {
                    EnergyReceivers[i].energy = this;
                }
            }
            else
            {
                for (int i = 0; i < EnergyReceivers.Length; i++)
                {
                    EnergyReceivers[i].hasEnergy = false;
                }
            }
        }

       
    }
}