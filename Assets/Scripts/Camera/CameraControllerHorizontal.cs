using System.Collections;
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
	public Vector3 minCameraPosition;
	public Vector3 maxCameraPosition;

	[HideInInspector] public Transform[] m_Targets;

	private Vector3 m_MoveVelocity;
	private Vector3 m_DesiredPosition;

	private const int ADAPT_POSITION = -1;


	float defaultWidth;

	Vector3 CameraPos;



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

	//CameraPos.x + 
	void FixCameraResolution ()
	{
		transform.position = new Vector3 (ADAPT_POSITION * (defaultWidth - Camera.main.orthographicSize * Camera.main.aspect), CameraPos.y, CameraPos.z);
	}

	// Use this for initialization
	void Start () {
		initCamera ();
	}

	void initCamera ()
	{
		CameraPos = Camera.main.transform.position;

		defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
		FixCameraResolution ();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Move ();
	}

	private void Move()
	{
		FindAveragePosition();
		transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);

		transform.position = new Vector3 (Mathf.Clamp (transform.position.x, minCameraPosition.x, maxCameraPosition.x), 
			Mathf.Clamp (transform.position.y, minCameraPosition.y, maxCameraPosition.y),
			Mathf.Clamp (transform.position.z, minCameraPosition.z, maxCameraPosition.z));
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

	public void SetMinCamPosition(){
		minCameraPosition = transform.position;
	}

	public void SetMaxCamPosition(){
		maxCameraPosition = transform.position;
	}
}
