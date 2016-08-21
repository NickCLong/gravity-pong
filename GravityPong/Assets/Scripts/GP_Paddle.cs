using UnityEngine;
using System.Collections;
using System;

public class GP_Paddle : MonoBehaviour {
    public enum PaddleState
    {
        normal,
        chargingSuper,
        activeSuper,
        cooldownSuper
    }

    public float m_fMaxRadialVelocity;
    public float m_fAbsValueOfRadialAcceleration;
    public float m_fDistanceFromGoal;
    public float m_fSuperVelocityMultiplier;
    public Transform m_TCenterOfGoal;
    public int m_iWarmUpSuperFrames;
    public int m_iWarmUpSlowDownFrames;
    public int m_iActiveSuperFrames;
    public int m_iCoolDownSuperFrames;

    private bool m_bIsSuperActive;

    private float m_fRadialAcceleration;
    private float m_fRadialVelocity;
    private Rigidbody2D m_rbRigidBody2D;
    private PaddleState m_ePaddleState;
    private int m_iSuperFrameCounter;
    private float m_fVelocityCache;

    private bool m_bHeldThisFrame;
    private bool m_bAbleToSuper;
    private bool m_bSuperIsCharged;

	// Use this for initialization
	void Start () {

        m_fRadialAcceleration = m_fAbsValueOfRadialAcceleration;
        m_fRadialVelocity = 0f;
        m_rbRigidBody2D = GetComponent<Rigidbody2D>();
        m_ePaddleState = PaddleState.normal;
        m_iSuperFrameCounter = 0;
        m_bHeldThisFrame = false;
        m_bAbleToSuper = true;
        m_bSuperIsCharged = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //switch(m_ePaddleState)
        //{
        //    case PaddleState.chargingSuper:
        //        if( m_bHeldThisFrame && m_bAbleToSuper)
        //        {
        //            float t = (float)++m_iSuperFrameCounter / (float)m_iWarmUpSlowDownFrames;
        //            t = Mathf.Min(t, 1f);
        //            m_fRadialVelocity = Mathf.SmoothStep(m_fVelocityCache, 0f, t);
        //            if( m_iSuperFrameCounter >= m_iWarmUpSuperFrames)
        //            {
        //                m_bSuperIsCharged = true;
        //            }
        //        }
        //        else
        //        {
        //            TransitionToState(PaddleState.normal);
        //        }
        //        break;
        //    case PaddleState.activeSuper:
        //        if( ++m_iSuperFrameCounter >= m_iActiveSuperFrames)
        //        {
        //            TransitionToState(PaddleState.cooldownSuper);
        //            m_bAbleToSuper = false;
        //        }
        //        break;
        //    case PaddleState.cooldownSuper:
        //        float t1 = (float)++m_iSuperFrameCounter / (float)m_iCoolDownSuperFrames;
        //        m_fRadialVelocity = Mathf.SmoothStep(m_fVelocityCache, 0f, t1);
        //        if( m_iSuperFrameCounter >= m_iCoolDownSuperFrames )
        //        {
        //            TransitionToState(PaddleState.normal);
        //        }
        //        break;
        //    case PaddleState.normal:
        //    default:
        //        m_fRadialVelocity += m_fRadialAcceleration;
        //        if (m_fRadialVelocity < 0)
        //        {
        //            m_fRadialVelocity = -Mathf.Min(Mathf.Abs(m_fRadialVelocity), m_fMaxRadialVelocity);
        //        }
        //        else
        //        {
        //            m_fRadialVelocity = Mathf.Min(m_fRadialVelocity, m_fMaxRadialVelocity);
        //        }
        //        break;
        //}

        m_fRadialVelocity += m_fRadialAcceleration;
        if (m_fRadialVelocity < 0)
        {
            m_fRadialVelocity = -Mathf.Min(Mathf.Abs(m_fRadialVelocity), m_fMaxRadialVelocity);
        }
        else
        {
            m_fRadialVelocity = Mathf.Min(m_fRadialVelocity, m_fMaxRadialVelocity);
        }

        //transform.eulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(transform.eulerAngles.z, transform.eulerAngles.z + m_fRadialVelocity, 1f));
        //transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + Mathf.DeltaAngle(transform.eulerAngles.z, +transform.eulerAngles.z + m_fRadialVelocity));
        transform.Rotate(new Vector3(0f, 0f, m_fRadialVelocity));
        //transform.eulerAngles = new Vector3(0f, 0f, NormalizeAngleBetween0and360(transform.eulerAngles.z));
        transform.localPosition =
            new Vector3(m_fDistanceFromGoal * Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad),
                        m_fDistanceFromGoal * Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad),
                        0f);
    }

    private float NormalizeAngleBetween0and360(float rotation)
    {
        float result = rotation;
        result = result % 360;
        if (result < 0) result += 360f;

        return result;
    }

    public void OnTouchInputReceived()
    {
        //switch (m_ePaddleState)
        //{
        //    case PaddleState.chargingSuper:
        //        break;
        //    case PaddleState.activeSuper:
        //        break;
        //    case PaddleState.cooldownSuper:
        //        break;
        //    case PaddleState.normal:
        //    default:
        //        break;
        //}

        m_fRadialVelocity = 0f;
        m_fRadialAcceleration = -m_fRadialAcceleration;

        //Debug.Log("Input received at " + DateTime.Now.ToLongTimeString());
    }

    public void OnHoldReceived()
    {
        if( m_ePaddleState == PaddleState.normal && m_bAbleToSuper)
        {
            TransitionToState(PaddleState.chargingSuper);
        }
        m_bHeldThisFrame = true;
    }

    public void OnHoldReleased()
    {
        if (m_bSuperIsCharged)
        {
            TransitionToState(PaddleState.activeSuper);
            m_bSuperIsCharged = false;
        }
        else if ( m_ePaddleState == PaddleState.chargingSuper )
        {
            TransitionToState(PaddleState.normal);
        }
        m_bHeldThisFrame = false;
        m_bAbleToSuper = true;
    }

    public void TransitionToState(PaddleState newState)
    {
        switch (newState)
        {
            case PaddleState.chargingSuper:
                GetComponent<Collider2D>().isTrigger = true;
                m_fVelocityCache = m_fRadialVelocity;
                break;
            case PaddleState.activeSuper:
                m_bIsSuperActive = true;
                m_fRadialVelocity = m_fMaxRadialVelocity * m_fSuperVelocityMultiplier * (m_fVelocityCache >= 0 ? 1 : -1);
                GetComponent<Collider2D>().isTrigger = false;
                break;
            case PaddleState.cooldownSuper:
                m_fVelocityCache = m_fRadialVelocity;
                GetComponent<Collider2D>().isTrigger = true;
                break;
            case PaddleState.normal:
            default:
                GetComponent<Collider2D>().isTrigger = false;
                break;
        }

        m_iSuperFrameCounter = 0;
        m_ePaddleState = newState;
    }
}
