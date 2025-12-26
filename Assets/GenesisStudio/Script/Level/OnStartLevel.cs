using UnityEngine;

namespace GenesisStudio
{
    public class OnStartLevel : MonoBehaviour
    {
        [SerializeField] bool beginWithFade;
        [SerializeField] string levelName; // if beginWithFade is true
        
        void Start()
        {
            // if (beginWithFade) GameManager.Instance.UI_FadeInScreen(levelName);
        }
    }
}
