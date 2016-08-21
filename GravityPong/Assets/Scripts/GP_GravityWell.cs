using UnityEngine;
using System.Collections;




public class GP_GravityWell : MonoBehaviour {
    public enum GPE_GrowthFunction
    {
        linear,
        quadratic,
        cubic
    }

    public GP_Ball m_mbBall;
    public GPE_GrowthFunction m_eGrowthFunction;
    public float m_fA;
    public float m_fB;
    public float m_fC;
    public float m_fK;
    public float m_fGravitationalConstant;

    public float m_fGravitationalStrength;
    private bool m_bIsActive;
    private static bool bShouldStartActive = true;

	// Use this for initialization
	void Start () {
	    if( bShouldStartActive )
        {
            m_bIsActive = true;
            bShouldStartActive = false;
        }
        else
        {
            m_bIsActive = false;
        }

        m_fGravitationalStrength = 40f;
}
	
	// Update is called once per frame
	void FixedUpdate () {
        if( m_bIsActive )
        {
            Vector3 ForceVector = (transform.position - m_mbBall.transform.position).normalized * m_fGravitationalStrength / Mathf.Pow(Vector3.Distance(transform.position, m_mbBall.transform.position),1f);
            m_mbBall.AddForceToBall(ForceVector);
        }
	
	}

    public void ToggleGravityWellActive()
    {
        m_bIsActive = !m_bIsActive;
    }

    public void SetGravityWellActive(bool _active)
    {
        m_bIsActive = _active;
    }

    public void SetGravitationalStrength(float ballVelocity)
    {
        float newStrength = 0f;
        switch( m_eGrowthFunction )
        {

            case GPE_GrowthFunction.quadratic:
                newStrength = GP_Math.QuadraticAdjust(ballVelocity, m_fA, m_fB);
                break;
            case GPE_GrowthFunction.cubic:
                newStrength = GP_Math.CubicAdjust(ballVelocity, m_fA, m_fB, m_fC);
                break;
            case GPE_GrowthFunction.linear:
            default:
                newStrength = GP_Math.LinearAdjust(ballVelocity, m_fA);
                break;
        }

        m_fGravitationalStrength = newStrength;
    }

    public bool IsGravityWellActive()
    {
        return m_bIsActive;
    }
}
