using System;
using System.Collections;
using UnityEngine;
using static GenesisStudio.Needs;
using Object = UnityEngine.Object;

namespace GenesisStudio
{
    public class Generator : EnergyTransmiter , IMessage 
    {
        [field: SerializeField] public ItemData ObjectData { get; private set; }

        public bool IsFull => false;

        [Header("Fuel")] 
        [Min(0)] public float consumeFuel;
        private Coroutine consumeCoroutine;
        private EnergyBlock _energyBlock;


        public string Status => $"Status {(on ? "<color=green>On</color>" : "<color=red>Off</color>")} ";
        
        public override void Interact(object sender)
        {
            base.Interact(sender);
            if (on)
            {
                consumeCoroutine ??= StartCoroutine(Consume());
            }
            else
            {
                if(consumeCoroutine != null)
                {
                    StopCoroutine(consumeCoroutine);
                    consumeCoroutine = null;
                }
            }
        }
        

        private void Update()
        {
            
        }

        public IEnumerator AddLiquid()
        {
            yield return null;
        }
        
        private void OnEnable()
        {
            energyBlock = FindObjectsByType<EnergyBlock>((FindObjectsSortMode)FindObjectsInactive.Exclude).GetNearestObject(transform);
        }

        private IEnumerator Consume()
        {
            yield return null;
        }
        
       
    }
}