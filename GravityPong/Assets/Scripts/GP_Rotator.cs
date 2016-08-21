using UnityEngine;
using System.Collections;

public class GP_Rotator : MonoBehaviour {

    public float m_fRotationSpeed;

    private GP_GravityWell m_mbGravityWell;

	// Use this for initialization
	void Start () {
        m_mbGravityWell = GetComponent<GP_GravityWell>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if( m_mbGravityWell.IsGravityWellActive() )
        {
            transform.Rotate(new Vector3(0f, 0f, m_fRotationSpeed));
        }
	
	}
}
