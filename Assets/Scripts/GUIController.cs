using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

    private static GUIController instance;

    public static GUIController Instance
    {
        get
        {
            return instance;
        }
    }

    private Text[] texts;
	private Text coinText;
	private Text healthText;
	private int coinCounter;

    private void Awake() {
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

	void Start () {
		// TODO move this to its own GUI controller
		texts = gameObject.GetComponentsInChildren<Text>();
		foreach (Text t in texts)
		{
			// TODO investigate if "(Clone)" (when instantiating from prefab) hinders searching by name
			switch (t.name)
			{
				case "CoinText":
					coinText = t;
					coinCounter = 0;
					UpdateCoinText();
					break;
				case "HealthText":
					healthText = t;
					break;
				default:
					Debug.LogWarning("Unhandled GUI/Canvas Text reference: " + t);
					break;
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
