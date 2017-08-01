﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{

    private static GUIController instance;

    public static GUIController Instance
    {
        get
        {
            return instance;
        }
    }

    private Text coinText;
    private Text weakHealthText;
    private Text heavyHealthText;
    // TODO Move to ItemManager?
    private int coinCounter;
    private float weakHealth;
    private float heavyHealth;

    private void Awake()
    {
        // source: https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/writing-game-manager
        // and https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        // and https://stackoverflow.com/documentation/unity3d/2137/singletons-in-unity/14518/a-simple-singleton-monobehaviour-in-unity-c-sharp#t=201707311922517721043
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach (Text t in GetComponentsInChildren<Text>())
        {
            // TODO investigate if "(Clone)" (when instantiating from prefab) hinders searching by name
            switch (t.name)
            {
                case "CoinText":
                    coinText = t;
                    coinCounter = 0;
                    UpdateCoinText();
                    break;
                case "WeakHealthText":
                    weakHealthText = t;
                    weakHealth = WeakController.Instance.GetComponent<HealthController>().maxHealth;
                    UpdateHealth(WeakController.Instance.gameObject, weakHealth);
                    break;
                case "HeavyHealthText":
                    heavyHealthText = t;
                    heavyHealth = HeavyController.Instance.GetComponent<HealthController>().maxHealth;
                    UpdateHealth(HeavyController.Instance.gameObject, heavyHealth);
                    break;
                default:
                    Debug.LogWarning("Unhandled GUI/Canvas Text reference: " + t);
                    break;
            }
        }
    }

    public void UpdateHealth(GameObject player, float health)
    {
        if (player == WeakController.Instance.gameObject)
        {
            weakHealth = health;
            if (weakHealthText != null)
            {
                weakHealthText.text = "Weak:\n" + weakHealth;
            }
        }
        else
        {
            heavyHealth = health;
            if (heavyHealthText != null)
            {
                heavyHealthText.text = "Heavy:\n" + heavyHealth;
            }
        }
    }

    public void AddCoin()
    {
        coinCounter++;
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        coinText.text = "Coins:\n" + coinCounter;
    }
}
