using System;
using System.Collections;
using System.Collections.Generic;
using GenesisStudio;
using HalvaStudio.Save;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Settings : UIScreen
{
    [SerializeField] private List<GameObject> Tabs;
    private GameObject activeTab;

    public Slider slider_sensivity;
    
    private void Start()
    {
        slider_sensivity.maxValue = 3f;
        slider_sensivity.value = SaveManager.Instance.saveData.sensivity;
        slider_sensivity.minValue = 0f;
        slider_sensivity.onValueChanged.AddListener(Save_Sensivity);
    }

    public void Save_Sensivity(float value)
    {
        slider_sensivity.value = value;
        GameManager.Instance.Player.CameraMotor.Sensivity = slider_sensivity.value;
    }
   
    

    public void ChangeTab(GameObject newTab)
    {
        if(activeTab) activeTab.SetActive(false);
        activeTab = newTab;
        activeTab.SetActive(true);
    }

    public override void OnShow()
    {
        for (int i = 0; i < Tabs.Count; i++)
        {
            Tabs[i].SetActive(false);
        }
        ChangeTab(Tabs[0]);
    }

    public override void OnHide()
    {
        
    }
}