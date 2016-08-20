using UnityEngine;
using System.Collections;

public class GP_Controller : MonoBehaviour {

    public GP_Paddle Paddle;
    public int m_iFramesToDelay;

    private int m_iFramesDelayed;
    private bool m_inputReceived;
    private bool m_inputReleased;
	// Use this for initialization
	void Start () {
        m_iFramesDelayed = 0;
        m_inputReceived = false;
        m_inputReleased = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    if( Input.anyKeyDown )
        {
            //m_inputReceived = true;
            Paddle.OnTouchInputReceived();
        }

        else if( Input.anyKey && m_inputReleased)
        {
            if( !m_inputReceived )
                Paddle.OnHoldReceived();
            else
            {
                if ( ++m_iFramesDelayed >= m_iFramesToDelay )
                { 
                    m_inputReceived = false;
                    m_iFramesDelayed = 0;
                    //m_inputReleased = false;
                }
            }
        }

        else
        {
            if( m_inputReceived )
            {
                Paddle.OnTouchInputReceived();
                m_inputReceived = false;
                m_iFramesDelayed = 0;
            }

            Paddle.OnHoldReleased();
            m_inputReleased = true;
        }
	}
}
