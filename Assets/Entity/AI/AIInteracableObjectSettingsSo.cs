using UnityEngine;

namespace GenesisStudio
{
    [CreateAssetMenu(menuName = "Interactable object for AI", fileName = "Interactable object for AI")]
    public class AIInteracableObjectSettingsSo : ScriptableObject
    {
        [field: SerializeField] public string ObjectName { get; private set; }
        [field: SerializeField] public EInteractType InteractType { get; private set; }
        // [field: SerializeField] public int maxUsers { get; private set; } = 1;
    }
}
