using UnityEngine;
using System.Collections;

public class GP_Ball : MonoBehaviour {

    public float m_fStartingSpeed;
    public float m_fSpeedIncrementer;
    public float m_fContactImpulseStrength;

    private Rigidbody2D m_mbRigidBody2D;
    private float m_fReflectionIntensity;
    private float m_fMaxVelocity;
	// Use this for initialization
	void Start () {
        m_mbRigidBody2D = GetComponent<Rigidbody2D>();
        m_fReflectionIntensity = 10f;
        m_fMaxVelocity = m_fStartingSpeed;
        transform.position = Vector3.zero;
        m_mbRigidBody2D.velocity = Vector2.zero;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //m_mbRigidBody2D.velocity = m_mbRigidBody2D.velocity.normalized * Mathf.Min(m_mbRigidBody2D.velocity.magnitude, m_fMaxVelocity);
	}

    public void AddForceToBall(Vector3 forceToAdd)
    {
        m_mbRigidBody2D.AddForce(new Vector2(forceToAdd.x, forceToAdd.y), ForceMode2D.Force);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.tag == "Paddle" )
        {
            m_fMaxVelocity += m_fSpeedIncrementer;
            foreach (GameObject well in GameObject.FindGameObjectsWithTag("GravityWell"))
            {
                well.GetComponent<GP_GravityWell>().ToggleGravityWellActive();
                //well.GetComponent<GP_GravityWell>().SetGravitationalStrength(m_mbRigidBody2D.velocity.magnitude);
            }

            m_mbRigidBody2D.AddForce(collision.contacts[0].normal.normalized * m_fContactImpulseStrength, ForceMode2D.Impulse);
        }

        else if ( collision.gameObject.tag == "Goal" )
        {
            Start();
        }
    }
}
