using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{

    // Propriétés
    private Text text;

    private uint coins = 0;
    public uint Coins
    {
        get { return coins; }
        set
        {
            coins = value;
            UpdateText();
        }
    }

    private void Start()
    {
        text = transform.Find("Text").GetComponent<Text>();
    }

    public void Add(uint coins)
    {
        Coins += coins;
    }

    public void Remove(uint coins)
    {
        Coins -= coins;
    }

    private void UpdateText()
    {
        text.text = Coins.ToString();
    }

}
