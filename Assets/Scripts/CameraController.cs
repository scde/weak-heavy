using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float m_DampTime = 0.2f;
    public float m_ScreenEdgeBuffer = 4f;
    public float m_MinSize = 6.5f;
    //public float m_MaxSize = ?f; // If implemented TODO block player movement.
    /*[HideInInspector]*/ public Transform[] m_Targets; // show in Inspector to add Targets with drag and drop

    private Camera m_Camera;
    private float m_ZoomSpeed;
    private Vector3 m_MoveVelocity;
    private Vector3 m_DesiredPosition;

    // Use this for initialization
    private void Start () {
		m_Camera = GetComponentInChildren<Camera> ();
		if (m_Targets [0] == null || m_Targets [1] == null) {
			Debug.LogWarning ("m_Targets not set. Attempting to autofill.");
			m_Targets = new Transform[2];
			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
			if (players != null) {
				foreach (GameObject player in players) {
					if (player.name == "PlayerWeak") {
						m_Targets [0] = player.transform;
					}
					if (player.name == "PlayerHeavy") {
						m_Targets [1] = player.transform;
					}
				}
			}
			if (m_Targets [0] == null || m_Targets [1] == null)
				Debug.LogError ("m_Targets could not be autofilled. To fix unhide m_Targets and manually fill the array.");
		}
	}

    private void FixedUpdate() {
        Move();
        Zoom();
    }

    // Update is called once per frame
    private void Update () {
    }

    private void Move() {
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }

    private void FindAveragePosition() {
        Vector3 averagePos = new Vector3();

        for (int i = 0; i < m_Targets.Length; i++)
            averagePos += m_Targets[i].position;

        averagePos /= m_Targets.Length;

        // Fix(ate) z position (because of 2D)
        averagePos.z = transform.position.z;

        m_DesiredPosition = averagePos;
    }

    private void Zoom() {
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }

    private float FindRequiredSize() {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

        float size = 0f;

        for (int i = 0; i < m_Targets.Length; i++) {
            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
        }

        size += m_ScreenEdgeBuffer;

        size = Mathf.Max(size, m_MinSize);

        //size = Mathf.Min(size, m_MaxSize);

        return size;
    }
}
