using System.Collections;
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

    public float slideDuration;

    private Text coinText;
    private Text weakHealthText;
    private Text heavyHealthText;
    private Text paperRoleText;
    private Slider paperRoleSlider;
    private float slideTarget;
    private float curSlideTime;
    private float lastTimeStamp;
    // TODO Move to ItemManager?
    private int coinCounter;
    private float weakHealth;
    private float heavyHealth;
    private float passedTime;

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

    private void OnDisable()
    {
        EventManager.StopListening("Action_" + WeakController.Instance.PlayerId, HideFullScreenPopUp);
        EventManager.StopListening("Action_" + HeavyController.Instance.PlayerId, HideFullScreenPopUp);
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
                case "PaperRoleText":
                    paperRoleText = t;
                    paperRoleText.enabled = false;
                    paperRoleSlider = GetComponentInChildren<Slider>();
                    paperRoleSlider.gameObject.SetActive(false);
                    slideTarget = paperRoleSlider.minValue;
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

    public void ShowFullScreenPopUp(string text)
    {
        EventManager.StartListening("Action_" + WeakController.Instance.PlayerId, HideFullScreenPopUp);
        EventManager.StartListening("Action_" + HeavyController.Instance.PlayerId, HideFullScreenPopUp);
        paperRoleSlider.value = 1.0f;
        paperRoleSlider.gameObject.SetActive(true);
        // Allow escape characters like line breaks ("\n") passed string
        // source: https://forum.unity3d.com/threads/inputing-a-line-break-in-a-text-field-for-ui.319223/#post-3077848
        paperRoleText.text = System.Text.RegularExpressions.Regex.Unescape(text);
        paperRoleText.enabled = true;
        if (!GameManager.Instance.IsPaused)
        {
            EventManager.TriggerEvent("Pause");
        }
    }

    private void HideFullScreenPopUp()
    {
        // TODO animation of opening the scroll
        EventManager.StopListening("Action_" + WeakController.Instance.PlayerId, HideFullScreenPopUp);
        EventManager.StopListening("Action_" + HeavyController.Instance.PlayerId, HideFullScreenPopUp);
        paperRoleText.enabled = false;
        paperRoleSlider.gameObject.SetActive(false);
        paperRoleSlider.value = 0.0f;
        if (GameManager.Instance.IsPaused)
        {
            EventManager.TriggerEvent("Pause");
        }
    }
}
