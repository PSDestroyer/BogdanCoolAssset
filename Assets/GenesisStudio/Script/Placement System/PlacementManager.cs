using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenesisStudio
{
    public class PlacementManager : MonoBehaviour
    {
        [SerializeField] private LayerMask placeableLayer;
        [SerializeField] private Material transparent;
        public Material Transparent
        {
            get
            {
                if (transparent == null)
                    transparent = Resources.Load<Material>("Materials/Transparent");

                return transparent;
            }
        }

        private bool _isPlacing;
        public bool IsPlacing => _isPlacing;
        
        private GameObject _instance;
        private List<GameObject> _placeableObjects = new List<GameObject>();
        private bool _canPlace;
        private ItemData _currentData;
        
        
        private void Update()
        {
            if (!_isPlacing) return;
            if (Physics.Raycast(GameManager.Instance.Player.CameraMotor.ray, out var hit, 10f, placeableLayer))
            {
                _instance.transform.position = hit.point;
            }
        }

        public void StartPlacement(ItemData obj, Vector3 pos = default)
        {
            _currentData = obj;
            _isPlacing = true;
            _instance = Instantiate(obj.Prefab, pos, Quaternion.identity);
            
            // var objRenderer = placement_instance.GetComponentInChildren<MeshRenderer>();
            // placement_instance_material = objRenderer.sharedMaterial;
            // objRenderer.sharedMaterials[0] = placement_manager.Transparent;
        }

        public void Place(InputAction.CallbackContext context)
        {
            _placeableObjects.Add(_instance);
            _instance = null;
            _isPlacing = false;
            GameManager.Instance.Player.RemoveItem(_currentData);
        }

        public void StopPlacing()
        {
            if(_instance)
            {
                _isPlacing = false;
                _currentData = null;
                Destroy(_instance);
                
                InputManager.Instance.playerInput.actions[Needs.Interact].performed -= Place;
                
                // placement_instance.GetComponent<MeshRenderer>().materials[0] = placement_instance_material;
            }
        }
        
    }
}