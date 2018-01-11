using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    // Propriétés
    public uint Coins { get; private set; } = 0;
    public List<int> Keys { get; private set; }

    private CoinCounter coinCounter;

    private void Start()
    {
        Keys = new List<int>();
        coinCounter = GameObject.Find("Coin Counter").GetComponent<CoinCounter>();
    }

    // Update is called once per frame
    void Update () {
	}


    public void AddCoins(uint coins)
    {
        Coins += coins;
        if (coinCounter != null)
        {
            coinCounter.Add(coins);
        }
    }

    public void RemoveCoins(uint coins)
    {
        Coins -= coins;
        if (coinCounter != null)
        {
            coinCounter.Remove(coins);
        }
    }

    public void AddKey(int key)
    {
        Keys.Add(key);
        Debug.Log("Add Key : " + key);
    }
}
