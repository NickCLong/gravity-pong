using UnityEngine;
using System.Collections;

public class GP_Controller : MonoBehaviour {

    public GP_Paddle Paddle;
    public int m_iFramesToDelay;
    public int m_iFramesBeforeNewInput;

    private int m_iFramesDelayed;
    private int m_iFrameCounter;
    private bool m_inputReceived;
    private bool m_inputReleased;

    private bool m_bReadyForInput;
	// Use this for initialization
	void Start () {
        m_iFramesDelayed = 0;
        m_inputReceived = false;
        m_inputReleased = true;

        m_bReadyForInput = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if( !m_bReadyForInput )
        {
            if( ++m_iFrameCounter >= m_iFramesBeforeNewInput )
            {
                m_bReadyForInput = true;
                m_iFrameCounter = 0;
            } 
        }

        if ( Input.anyKeyDown )
        {
            if (m_bReadyForInput)
            {
                Paddle.OnTouchInputReceived();
                m_bReadyForInput = false;
                m_iFrameCounter = 0;
            }
        }



        //else if( Input.anyKey && m_inputReleased)
        //{
        //    if( !m_inputReceived )
        //        Paddle.OnHoldReceived();
        //    else
        //    {
        //        if ( ++m_iFramesDelayed >= m_iFramesToDelay )
        //        { 
        //            m_inputReceived = false;
        //            m_iFramesDelayed = 0;
        //            //m_inputReleased = false;
        //        }
        //    }
        //}

        //else
        //{
        //    if( m_inputReceived )
        //    {
        //        Paddle.OnTouchInputReceived();
        //        m_inputReceived = false;
        //        m_iFramesDelayed = 0;
        //    }

        //    Paddle.OnHoldReleased();
        //    m_inputReleased = true;
        //}
	}
}
