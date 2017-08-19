﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerHorizontal : MonoBehaviour {


	private static CameraControllerHorizontal instance = null;

	public static CameraControllerHorizontal Instance
	{
		get
		{
			return instance;
		}
	}

	public float m_DampTime = 0.2f;
	public float m_ScreenEdgeBuffer = 4f;
	public float m_MinSize = 6.5f;
	public bool isZooming = false;

	private Camera m_Camera;
	private float m_ZoomSpeed;
	private Vector3 m_MoveVelocity;
	private Vector3 m_DesiredPosition;

	[HideInInspector] public Transform[] m_Targets;

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

	// Use this for initialization
	void Start () {
		m_Camera = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Move ();
		if (isZooming) {
			Zoom ();
		}
	}

	private void Move()
	{
		FindAveragePosition();

		transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
	}

	private void Zoom(){
		float requiredSize = FindRequiredSize ();

		m_Camera.orthographicSize = Mathf.SmoothDamp (m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
	}

	private void FindAveragePosition()
	{
		Vector3 averagePos = new Vector3();

		for (int i = 0; i < m_Targets.Length; i++)
			averagePos += m_Targets[i].position;

		averagePos /= m_Targets.Length;

		// Fix(ate) z position (because of 2D)
		// Fix(ate) y position for only moving in x direction
		averagePos.z = transform.position.z;
		averagePos.y = transform.position.y;

		m_DesiredPosition = averagePos;
	}


	private float FindRequiredSize()
	{
		Vector3 averagePos = new Vector3 ();

		for (int i = 0; i < m_Targets.Length; i++)
			averagePos += m_Targets [i].position;

		return averagePos.x + (2* m_ScreenEdgeBuffer);


	}
}
