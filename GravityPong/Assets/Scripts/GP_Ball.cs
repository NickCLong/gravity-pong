using UnityEngine;
using System.Collections;

public class GP_Ball : MonoBehaviour {

    public float m_fStartingSpeed;
    public float m_fSpeedIncrementer;
    public float m_fContactImpulseStrength;

    private Rigidbody2D m_mbRigidBody2D;
    private float m_fReflectionIntensity;
    private float m_fMaxVelocity;

    private GP_GravityWell m_mbGravityWell1;
    private GP_GravityWell m_mbGravityWell2;

    private GP_Paddle m_mbPaddle1;
    private GP_Paddle m_mbPaddle2;

	// Use this for initialization
	void Start () {
        m_mbRigidBody2D = GetComponent<Rigidbody2D>();
        m_fReflectionIntensity = 10f;
        m_fMaxVelocity = m_fStartingSpeed;
        transform.position = Vector3.zero;
        m_mbRigidBody2D.velocity = Vector2.zero;

        // If anyone else is reading this code. I am very aware of how terrible this section is,
        // but it works. And game jams don't have much time for refactoring.

        //go_i stands for "GameObject Index"
        int go_i = 0;
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            for(int index = 0; index < go.transform.childCount; ++index)
            {
                if (go_i == 0)
                {
                    if (go.transform.GetChild(index).gameObject.tag == "GravityWell")
                    {
                        m_mbGravityWell1 = go.transform.GetChild(index).GetComponent<GP_GravityWell>();
                    }

                    if (go.transform.GetChild(index).gameObject.tag == "Paddle")
                    {
                        m_mbPaddle1 = go.transform.GetChild(index).GetComponent<GP_Paddle>();
                    }
                }

                else
                {
                    if (go.transform.GetChild(index).gameObject.tag == "GravityWell")
                    {
                        m_mbGravityWell2 = go.transform.GetChild(index).GetComponent<GP_GravityWell>();
                    }

                    if (go.transform.GetChild(index).gameObject.tag == "Paddle")
                    {
                        m_mbPaddle2 = go.transform.GetChild(index).GetComponent<GP_Paddle>();
                    }
                }
            }

            ++go_i;
        }
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        m_mbRigidBody2D.velocity = m_mbRigidBody2D.velocity.normalized * Mathf.Min(m_mbRigidBody2D.velocity.magnitude, m_fMaxVelocity);
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

            if( collision.gameObject == m_mbPaddle1.gameObject )
            {
                m_mbGravityWell1.SetGravityWellActive(false);
                m_mbGravityWell2.SetGravityWellActive(true);
            }
            else
            {
                m_mbGravityWell1.SetGravityWellActive(true);
                m_mbGravityWell2.SetGravityWellActive(false);
            }

            m_mbRigidBody2D.AddForce(collision.contacts[0].normal.normalized * m_fContactImpulseStrength, ForceMode2D.Impulse);
        }

        else if ( collision.gameObject.tag == "Goal" )
        {
            Start();
        }
    }
}
