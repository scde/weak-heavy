using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    private bool uiShown;
    public bool UiShown
    {
        get
        {
            return uiShown;
        }
    }

    private EventSystem eventSystem;
    public EventSystem EventSystem
    {
        get
        {
            return eventSystem;
        }
    }

    public MenuController[] menus;
    public MenuController[] Menus
    {
        get
        {
            return menus;
        }
    }

    public EventSystem eventSystemPrefab;

    private MenuController currentUniqueMenu;
    private List<MenuController> currentMenus;
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
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            eventSystem = Instantiate(eventSystemPrefab) as EventSystem;
        }

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
            }
        }

        currentMenus = new List<MenuController>();
    }

    // Neccessary for Buttons (children) to load scenes and retain those settings in prefab
    public void LoadScene(string sceneName)
    {
        GameManager.Instance.LoadScene(sceneName);
    }

    // Neccessary for Buttons (children) to quit the game and retain those settings in prefab
    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    public void ShowMenu(MenuController menu)
    {
        if (!GameManager.Instance.IsPaused && menu.pausesGame)
        {
            EventManager.Instance.TriggerEvent("Pause");
        }

        if (menu.isUnique)
        {
            if (currentUniqueMenu != null)
            {
                currentUniqueMenu.IsShown = false;
            }

            currentUniqueMenu = menu;
            currentUniqueMenu.IsShown = true;
            uiShown = true;
        }
        else
        {
            if (!currentMenus.Contains(menu))
            {
                currentMenus.Add(menu);
            }
            menu.IsShown = true;
        }
    }

    public void HideMenu(MenuController menu)
    {
        if (GameManager.Instance.IsPaused)
        {
            EventManager.Instance.TriggerEvent("UnPause");
        }

        if (menu.isUnique)
        {
            if (currentUniqueMenu != null)
            {
                currentUniqueMenu.IsShown = false;
            }
            uiShown = false;
        }
        else
        {
            currentMenus.Remove(menu);
            menu.IsShown = false;
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
