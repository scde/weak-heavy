using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text coinText;

	private int coinCounter;

	private void Start() {
        coinCounter = 0;
        UpdateCoinText();
    }

    public void AddCoin() {
        coinCounter++;
        UpdateCoinText();
    }

    private void UpdateCoinText() {
        coinText.text = "Coin:\n" + coinCounter;
    }
}
