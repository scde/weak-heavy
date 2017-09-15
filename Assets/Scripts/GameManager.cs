using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public EventManager eventManagerPrefab;
    public WeakController playerWeakPrefab;
    public HeavyController playerHeavyPrefab;
    public CameraControllerHorizontal cameraControllerHorizontalPrefab;
    public GUIController guiControllerPrefab;
    public float RespawnTime = 1f;

    private Transform checkpointWeak;
    public Transform CheckpointWeak
    {
        set
        {
            checkpointWeak = value;
        }
    }
    private Transform checkpointHeavy;
    public Transform CheckpointHeavy
    {
        set
        {
            checkpointHeavy = value;
        }
    }
    private MenuController pauseMenu;

    private void Awake()
    {
        // source: https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/writing-game-manager
        // and https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        // and https://stackoverflow.com/documentation/unity3d/2137/singletons-in-unity/14518/a-simple-singleton-monobehaviour-in-unity-c-sharp#t=201707311922517721043
        if (instance == null)
        {
            instance = this;
            // TODO between level saving
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //// SceneManager hook
        //SceneManager.sceneLoaded += OnSceneLoaded;

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

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    // Unpauses in case we switched from PauseMenu to another scene
    //    // and gets new pauseMenu reference
    //    Start();
    //}

    private void OnEnable()
    {
        EventManager.Instance.StartListening("Pause", PauseGame);
        EventManager.Instance.StartListening("UnPause", UnPauseGame);
        EventManager.Instance.StartListening("Respawn", Respawn);
    }

    private void OnDisable()
    {
        EventManager.Instance.StopListening("Pause", PauseGame);
        EventManager.Instance.StopListening("UnPause", UnPauseGame);
        EventManager.Instance.StopListening("Respawn", Respawn);
    }

    private void Start()
    {
        if (GUIController.Instance != null)
        {
            foreach (MenuController menu in GUIController.Instance.Menus)
            {
                switch (menu.name)
                {
                    case "PauseMenu":
                        pauseMenu = menu;
                        break;
                }
            }
        }
    }

    private void Update()
    {
        if (!isPaused)
        {
            if (Input.GetButtonDown("Cancel_0") || Input.GetButtonDown("Cancel_1"))
            {
                GUIController.Instance.ShowMenu(pauseMenu);
            }
        }
        else
        {
            if (Input.GetButtonDown("Cancel_0") || Input.GetButtonDown("Cancel_1"))
            {
                GUIController.Instance.HideMenu(pauseMenu);
            }
        }
    }

    private void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0.0f;
        }
    }

    private void UnPauseGame()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1.0f;
        }
    }

    private void Respawn()
    {
        // TODO handle Respawn/GameOver there is currently no GameOver
        StartCoroutine(WaitForRespawn());
    }

    private IEnumerator WaitForRespawn()
    {
        WeakController.Instance.gameObject.SetActive(false);
        HeavyController.Instance.gameObject.SetActive(false);
        yield return new WaitForSeconds(RespawnTime);
        WeakController.Instance.transform.position = checkpointWeak.position;
        HeavyController.Instance.transform.position = checkpointHeavy.position;
        WeakController.Instance.HealthController.ResetHealth();
        HeavyController.Instance.HealthController.ResetHealth();
        WeakController.Instance.gameObject.SetActive(true);
        HeavyController.Instance.gameObject.SetActive(true);
    }

    public void LoadScene(string sceneName)
    {
        UnPauseGame();
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        // TODO save game progress
        Application.Quit();
    }
}

public enum SceneName
{
    None,
    StartScreen,
    SelectLevelScreen,
    Tutorial,
    Level1Sabi,
    Level2Susi,
    Level3Jana
}