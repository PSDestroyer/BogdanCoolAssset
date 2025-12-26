using System;
using UnityEngine;

namespace GenesisStudio
{
    public class Lamp : MonoBehaviour
    {
        [SerializeField] private Light light;
        [SerializeField] private float intensity = 1f;
        [SerializeField] private Color lampColor = Color.white;

        private Renderer _renderer;
        private MaterialPropertyBlock _block;
        private bool _on;

        public bool On
        {
            set
            {
                _on = value;
                UpdateEmission();
                light.intensity = _on ? intensity : 0f;
            }
        }

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _renderer.material.EnableKeyword("_EMISSION");
            _block = new MaterialPropertyBlock();
            light.color = lampColor;

            On = false;
        }

        private void UpdateEmission()
        {
            _renderer.GetPropertyBlock(_block);
            _block.SetColor("_EmissionColor", _on ? lampColor : Color.black);
            _renderer.SetPropertyBlock(_block);
        }
    }
}