using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenesisStudio
{
    public class UIManager : MonoBehaviour
    {
        //TODO: Implement logic, tie with GameEventBus.cs
        public CanvasGroup UI_ScreenFadeObject;
        public float UI_ScreenFadeSpeed = 1f;
        public float UI_ScreenFadeHoldFor = 1f;
        [SerializeField] private TMP_Text UI_levelName_tmp;
        [SerializeField] private float checkCooldown;
        [SerializeField] GameObject UI_MainPanel;
        [SerializeField] private TMP_Text money_Text;
        [SerializeField] private TMP_Text money_floatingTextPrefab;
        [SerializeField] private Transform money_floatingTextSpawnPosition;
        
        // public float UI_holdingValue
        // {
        //     get => UI_interactSlider.value;
        //     set
        //     {
        //         UI_interactSlider.value = Mathf.Clamp01(value);
        //     }
        // }
        //
        // [SerializeField] private string UI_interactButton;
        // [SerializeField] private GameObject UI_interactMarker;
        // [SerializeField] private TMP_Text UI_message;
        // [SerializeField] private Menu UI_menu;
        // [SerializeField] private bool UI_isMessage;
        // [SerializeField] private bool UI_isInteractable;
        // [SerializeField] private Slider UI_interactSlider;
        // [SerializeField] private TMP_Text UI_interactButtonTMP;
        
        
        private string UI_levelName
        {
            get => UI_levelName_tmp.text;
            set => UI_levelName_tmp.text = value;
        }
        

        private IEnumerator UI_FadeInScreenCoroutine(
            string name = "", 
            float HoldFade = 0, 
            float showText = 1f, 
            string sound = "Show Text")
        {
            // PlayerEnable(false);
            UI_MainPanel.SetActive(false);
            UI_levelName_tmp.gameObject.SetActive(false);
            UI_ScreenFadeObject.alpha = 1;
            var holdFadeTimer = 0f;

            if (!string.IsNullOrEmpty(name))
            {
                UI_levelName = name;
                yield return new WaitForSeconds(showText);
                AudioManager.Instance.PlaySound(sound);
                UI_levelName_tmp.gameObject.SetActive(true);
            }

            while (holdFadeTimer <= HoldFade)
            {
                holdFadeTimer += Time.deltaTime;
                yield return null;
            }

            // PlayerEnable(true);
            UI_ScreenFadeObject.FadeIn(2f);
            UI_MainPanel.gameObject.SetActive(true);
        }

        public void UI_FadeInScreen(string name = "")
        {
            StartCoroutine(UI_FadeInScreenCoroutine(showText: 2f, HoldFade: UI_ScreenFadeHoldFor, name: name));
        }
        
        public void ToggleMenu(InputAction.CallbackContext context)
        {
        }
        
        public void AddFloatingText(int value, char sign)
        {
            var instance = Instantiate(money_floatingTextPrefab, money_floatingTextSpawnPosition);
            Color clr = sign switch
            {
                '+' => Color.green,
                '-' => Color.red,
                _ => Color.clear
            };
            instance.color = clr;
            instance.text = sign.ToString() + value + "$";
            StartCoroutine(FadeAndRise(instance, value));
        }

        private IEnumerator FadeAndRise(TMP_Text instance, int value)
        {
            var cg = instance.GetComponent<CanvasGroup>();
            while (cg.alpha >= 0)
            {
                instance.transform.position += Vector3.up * 6f * Time.deltaTime;
                cg.alpha -= Time.deltaTime / 1.5f;
                yield return null;  
            }
            Destroy(instance.gameObject);
        }

        //public IEnumerator UI_FadeOutScreenCoroutine(
        //    string name = "", 
        //    float DelayText = 0,
        //    string sound = "Show Text") 
        //{
        //    UI_levelName_tmp.gameObject.SetActive(false);
        //    UI_ScreenFadeObject.alpha = 0;
        //    while (UI_ScreenFadeObject.alpha < 1)
        //    {
        //        UI_ScreenFadeObject.alpha += UI_ScreenFadeSpeed * Time.deltaTime;
        //        yield return null;
        //    }

        //    if(string.IsNullOrEmpty(name)) yield break;
        //    UI_levelName = name;
        //    UI_levelName_tmp.gameObject.SetActive(false);
        //    yield return new WaitForSeconds(DelayText);
        //    UI_levelName_tmp.gameObject.SetActive(true);
        //}
    }
}