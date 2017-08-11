using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

	public void ChangeScene(string sceneName){
		Application.LoadLevel (sceneName);
	}

	public void OnCLick(){
		Debug.Log ("Click detected");
	}

	public void Finish(){
		Application.Quit ();
	}
}
