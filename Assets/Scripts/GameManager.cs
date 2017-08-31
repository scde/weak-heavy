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

    private bool isPaused;

    public bool IsPaused
    {
        get
        {
            return isPaused;
        }
    }

    // TODO add EventSystem?
    public EventManager eventManagerPrefab;
    public WeakController playerWeakPrefab;
    public HeavyController playerHeavyPrefab;
    public CameraControllerHorizontal cameraControllerHorizontalPrefab;
    public GUIController guiControllerPrefab;

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

        // Event Manager
        if (EventManager.Instance == null)
        {
            Instantiate(eventManagerPrefab);
        }

        // Players
        if (WeakController.Instance == null)
        {
            Instantiate(playerWeakPrefab);
        }

        if (HeavyController.Instance == null)
        {
            Instantiate(playerHeavyPrefab);
        }

        // Camera
        if (CameraControllerHorizontal.Instance == null)
        {
            Instantiate(cameraControllerHorizontalPrefab);
        }
        CameraControllerHorizontal.Instance.m_Targets = new Transform[] { WeakController.Instance.transform, HeavyController.Instance.transform };

        // GUI
        if (GUIController.Instance == null)
        {
            Instantiate(guiControllerPrefab);
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening("Pause", PauseGame);
    }

    private void OnDisable()
    {
        EventManager.StopListening("Pause", PauseGame);
    }

    private void Start()
    {
        isPaused = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            EventManager.TriggerEvent("Pause");
        }
    }

    private void PauseGame()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1.0f;
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0.0f;
        }
    }
}
