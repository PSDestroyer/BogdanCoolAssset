using GenesisStudio;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Outline = GenesisStudio.Outline;
using SaveManager = HalvaStudio.Save.SaveManager;
public class GameManager : Singleton<GameManager>
{
    //TODO: Remove all UI elements
    
    [SerializeField] GameObject UI_MainPanel;

    [SerializeField] private PlacementManager placement_manager;
    public PlacementManager PlacementManager => placement_manager;
    
    public MoneyManager money;
    [SerializeField] private TMP_Text money_Text;
    [SerializeField] private TMP_Text money_floatingTextPrefab;
    [SerializeField] private Transform money_floatingTextSpawnPosition;

    public Player Player => player;
    private Player player;
    

    bool player_Enabled = true;
    public float UI_holdingValue
    {
        get => UI_interactSlider.value;
        set
        {
            UI_interactSlider.value = Mathf.Clamp01(value);
        }
    }

    [SerializeField] private string UI_interactButton;
    [SerializeField] private GameObject UI_interactMarker;
    [SerializeField] private TMP_Text UI_message;
    [SerializeField] private Menu UI_menu;
    [SerializeField] private bool UI_isMessage;
    [SerializeField] private bool UI_isInteractable;
    [SerializeField] private Slider UI_interactSlider;
    [SerializeField] private TMP_Text UI_interactButtonTMP;

    
    [field: SerializeField] public Mission mission_currentMission { get; set; }

    public float indicator_height = 2f;
    public float indicator_arriveRange = 2f;
    public Color indicator_color = Color.yellowNice;

    private bool interact_lastInteractState;
    private Transform interact_lastHit;
    private IMessage interact_lastMessage;
    private IInteractable interact_lastInteractable;
    private Item interact_lastItem;

    private Outline currentOutline;
    [field: SerializeField] public OutlineManager OutlineManager { get; private set; }
    
    public CanvasGroup UI_ScreenFadeObject;
    public float UI_ScreenFadeSpeed = 1f;
    public float UI_ScreenFadeHoldFor = 1f;

    [SerializeField] private TMP_Text UI_levelName_tmp;
    [SerializeField] private float checkCooldown;
    private float lastCheckTime;

    private string UI_levelName
    {
        get => UI_levelName_tmp.text;
        set => UI_levelName_tmp.text = value;
    }


    protected override void AwakeInit()
    {
        if(player == null) player = FindAnyObjectByType<Player>();
    }

    private void Start()
    {
        InitializeInventory();
        InitializeQuest();
        PlayerEnable(true);
        if (placement_manager) InitializePlacedObjects();
        InputManager.Instance.playerInput.actions[Needs.Cancel].performed += ToggleMenu;
        UI_interactButtonTMP.text = UI_interactButton;
        UI_holdingValue = 0;
        money = new MoneyManager(SaveManager.Instance.saveData.money, money_Text);
    }

    private void InitializePlacedObjects()
    {
    }
    
    public void PlayerEnable(bool value)
    {
        // player_Enabled = value;
        // player.Controls(value);  
        UI_MainPanel.SetActive(value);
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        if (!player_Enabled) return;
        UI_menu.Toggle();
        if (!UI_menu.Enabled) UI_menu.Close();
        else UI_menu.Open();
    }

    

    private void Update()
    {
        if (placement_manager != null && placement_manager.IsPlacing)
        {
            UI_isInteractable = false;
            UI_isMessage = false;
            UpdateUI();
            return;
        }

       
    }

    private void CheckForInteract()
    {
        //UI_isInteractable = false;
        //UI_isMessage = false;

        //if (Physics.Raycast(player.BrainControls.CameraMotor.ray, out RaycastHit hit, 3f))
        //{
        //    if (hit.transform != interact_lastHit)
        //    {
        //        interact_lastHit = hit.transform;
        //        Task.Factory.StartNew(() =>
        //        {
        //            interact_lastMessage = hit.transform.GetComponent<IMessage>();
        //        });
        //        Task.Factory.StartNew(() =>
        //        {
        //            interact_lastInteractable = hit.transform.GetComponent<IInteractable>();
        //        });
        //        Task.Factory.StartNew(() =>
        //        {
        //            interact_lastItem = hit.transform.GetComponent<Item>();
        //        });
        //    }

        //    if (interact_lastMessage != null)
        //    {
        //        UI_isMessage = true;
        //        UI_message.text = interact_lastMessage.Status;
        //        return;
        //    }

        //    if (interact_lastInteractable != null)
        //    {
        //        UI_isInteractable = interact_lastInteractable.CanInteract;

        //        if (interact_lastItem != null)
        //        {
        //            OutlineManager.ShowOutline(interact_lastItem.Renderer, indicator_color, 10);
        //        }
        //        else
        //        {
        //            OutlineManager.ClearOutline();
        //        }
        //        return;
        //    }
        //}
        //else
        //{
        //    interact_lastHit = null;
        //    interact_lastMessage = null;
        //    interact_lastInteractable = null;
        //    interact_lastItem = null;
        //}
    }

    private void UpdateUI()
    {
        UI_interactMarker.SetActive(UI_isInteractable);
        UI_message.gameObject.SetActive(UI_isMessage);
    }

    #region Quest

    private void InitializeQuest()
    {
    }
    

    #endregion

    #region Money

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

    public void AddMoney(int amount)
    {
        money.Money += amount;
    }

    public void RemoveMoney(int amount)
    {
        money.Money -= amount;
    }

    #endregion

    #region Inventory

    private void InitializeInventory()
    {
        
    }

    #endregion

    #region UI

    private IEnumerator UI_FadeInScreenCoroutine(
        string name = "", 
        float HoldFade = 0, 
        float showText = 1f, 
        string sound = "Show Text")
    {
        PlayerEnable(false);
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

        PlayerEnable(true);
        while (UI_ScreenFadeObject.alpha > 0)
        {
            UI_ScreenFadeObject.alpha -= UI_ScreenFadeSpeed * Time.deltaTime;
            yield return null;
        }
        UI_MainPanel.gameObject.SetActive(true);
    }

    public void UI_FadeInScreen(string name = "")
    {
        StartCoroutine(UI_FadeInScreenCoroutine(showText: 2f, HoldFade: UI_ScreenFadeHoldFor, name: name));
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

    #endregion


    public void Save()
    {
        SaveManager.Instance.Save();
    }

    public Coroutine InvokeCoroutineHelper(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

}
