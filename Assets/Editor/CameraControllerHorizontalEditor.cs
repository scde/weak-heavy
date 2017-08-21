using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraControllerHorizontal))]
public class NewBehaviourScript : Editor {

	private const string ADD_MIN_STRING = "Set min cam position";
	private const string ADD_MAX_STRING = "Set max cam position";

	public override void OnInspectorGUI(){
		DrawDefaultInspector ();
		CameraControllerHorizontal cameraController = (CameraControllerHorizontal)target;

		if (GUILayout.Button (ADD_MIN_STRING)) {
			cameraController.SetMinCamPosition();
			if (GUI.changed) {
				EditorUtility.SetDirty (cameraController);
			}
		}

		if (GUILayout.Button (ADD_MAX_STRING)) {
			cameraController.SetMaxCamPosition ();
			if (GUI.changed) {
				EditorUtility.SetDirty (cameraController);
			}
		}
	}
}
