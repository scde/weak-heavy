using System.Collections;
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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    new void Start()
    {
        base.Start();

        attackController = GetComponent<AttackController>();
    }

    new void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Fire1_" + playerId))
        {
            attackController.Attack();
        }

        // FIXME testcode
        if (Input.GetKeyDown("1"))
            attackController.SwitchWeapon(0);
        if (Input.GetKeyDown("2"))
            attackController.SwitchWeapon(1);
        if (Input.GetKeyDown("3"))
            attackController.SwitchWeapon(2);
        if (Input.GetKeyDown("4"))
            attackController.SwitchWeapon(3);
    }
}
