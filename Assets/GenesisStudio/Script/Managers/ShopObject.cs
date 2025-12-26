using System;
using System.Collections;
using System.Collections.Generic;
using GenesisStudio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShopObject : MonoBehaviour
{
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemPrice;

    public Shop CurrentShop; 

    public string item_name
    {
        get => itemName.text;
        set => itemName.text = value;
    }

    public int item_price
    {
        get
        {
            if (string.IsNullOrEmpty(itemName.text)) itemName.text = "0";
            return int.Parse(itemPrice.text);
        }
        set => itemPrice.text = value.ToString();
    }

    public Button Button => GetComponent<Button>();
    public ItemData data;


}
