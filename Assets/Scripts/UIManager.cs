using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MenuScript {

	public GameObject pausePanel;

	public bool isPaused;


	void Start(){
		isPaused = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isPaused) {
			PauseGame (true);
		} else {
			PauseGame (false);
		}

		if (Input.GetButtonDown ("Cancel")) {
			SwitchPaused ();
		}
	}

	void PauseGame(bool state){
		if (state) {
			pausePanel.SetActive (true);
			Time.timeScale = 0.0f;
		} else {
			pausePanel.SetActive (false);
			Time.timeScale = 1.0f;
		}
	}

	public void SwitchPaused(){
		if (isPaused) {
			isPaused = false;
		} else {
			isPaused = true;
		}
	}
}
