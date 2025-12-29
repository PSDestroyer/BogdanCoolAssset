using System;

namespace GenesisStudio
{
    public class PowerPole : EnergyTransmiter
    {
        protected override void Start()
        {
            CanInteract = false;
            on = true;
            if(energyBlock != null)
                energyBlock.Energy = on;
        }

        private void Update()
        {
            if(energyBlock != null)
                energyBlock.Energy = on;
        }
    }
}