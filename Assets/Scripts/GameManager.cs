using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Setup GameObjects/Prefabs at startup
// loads Levels
// Listens to global events (Pause, Menu, Load, Save, etc.)
public class GameManager : MonoBehaviour
{

    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    // TODO add EventSystem?
    public WeakController playerWeakPrefab;
    public HeavyController playerHeavyPrefab;
	public CameraControllerHorizontal cameraControllerPrefab;
    public GUIController guiControllerPrefab;

    // TODO maybe use these references instead of singleton on the players
    //private WeakController playerWeak;
    //private HeavyController playerHeavy;

    //public WeakController PlayerWeak
    //{
    //	get
    //	{
    //		return playerWeak;
    //	}
    //}

    //public HeavyController PlayerHeavy
    //{
    //	get
    //	{
    //		return playerHeavy;
    //	}
    //}

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

        // Players
        if (WeakController.Instance == null)
        {
            Instantiate(playerWeakPrefab);
        }
        //playerWeak = WeakController.Instance;
        if (HeavyController.Instance == null)
        {
            Instantiate(playerHeavyPrefab);
        }
        //playerHeavy = HeavyController.Instance;

        // Camera
		if (CameraControllerHorizontal.Instance == null)
        {
            Instantiate(cameraControllerPrefab);
        }
		CameraControllerHorizontal.Instance.m_Targets = new Transform[] { WeakController.Instance.transform, HeavyController.Instance.transform };

        // GUI
        if (GUIController.Instance == null)
        {
            Instantiate(guiControllerPrefab);
        }
    }
}
