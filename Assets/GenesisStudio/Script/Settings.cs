using System;
using System.Collections;
using System.Collections.Generic;
using GenesisStudio;
using HalvaStudio.Save;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject[] ActiveTabs;
    private GameObject activeTab;

    public Slider slider_sensivity;
    public TMP_Dropdown dropdown_typeOfInput;
    
    private void Start()
    {
        for (int i = 0; i < ActiveTabs.Length; i++)
        {
            ActiveTabs[i].SetActive(false);
        }

        slider_sensivity.maxValue = 3f;
        slider_sensivity.value = SaveManager.Instance.saveData.sensivity;
        slider_sensivity.minValue = 0f;
        slider_sensivity.onValueChanged.AddListener(Save_Sensivity);
        
        
        if(dropdown_typeOfInput == null) return;
        dropdown_typeOfInput.options.Clear();
        var schemes = InputManager.Instance.playerInput.actions.controlSchemes;
        for (int i = 0; i < schemes.Count; i++)
        {
            dropdown_typeOfInput.options.Add(new TMP_Dropdown.OptionData(schemes[i].name));
        }
        dropdown_typeOfInput.onValueChanged.AddListener(Save_ChangeInput);
    }

    public void Save_Sensivity(float value)
    {
        slider_sensivity.value = value;
        GameManager.Instance.Player.CameraMotor.Sensivity = slider_sensivity.value;
    }
    
    public void Save_ChangeInput(Int32 value)
    {
        InputManager.Instance.playerInput.SwitchCurrentControlScheme(dropdown_typeOfInput.options[value].text);
    }
    

    public void ChangeTab(GameObject newTab)
    {
        if(activeTab) activeTab.SetActive(false);
        activeTab = newTab;
        activeTab.SetActive(true);
    }
}