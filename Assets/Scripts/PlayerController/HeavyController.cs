﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyController : PlayerController
{

    private static HeavyController instance = null;

    private AttackController attackController;

    public static HeavyController Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        // source: https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/writing-game-manager
        // and https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        // and https://stackoverflow.com/documentation/unity3d/2137/singletons-in-unity/14518/a-simple-singleton-monobehaviour-in-unity-c-sharp#t=201707311922517721043
        if (instance == null)
        {
            instance = this;
            playerId = 1;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private new void OnEnable()
    {
        base.OnEnable();

        EventManager.Instance.StartListening("ItemMenuHide_" + playerId, HideItemMenu);
    }

    private new void OnDisable()
    {
        base.OnDisable();

        EventManager.Instance.StopListening("ItemMenuHide_" + playerId, HideItemMenu);
    }

    private new void Start()
    {
        base.Start();

        attackController = GetComponent<AttackController>();

        foreach (MenuController menu in GetComponentsInChildren<MenuController>())
        {
            switch (menu.name)
            {
                case "ItemMenuHeavy":
                    itemMenu = menu;
                    break;
            }
        }
    }

    private new void Update()
    {
        base.Update();

        if (!GameManager.Instance.IsPaused)
        {
            if (Input.GetButtonDown("Fire1_" + playerId))
            {
                attackController.Attack();
            }
        }
    }

    private new void HideItemMenu()
    {
        base.HideItemMenu();

        if (itemController.HighlightedItem == ItemID.None
            || itemController.unlockedItems[itemController.HighlightedItem])
        {
            attackController.SwitchWeapon(itemController.EquipedItem);
        }
    }
}
