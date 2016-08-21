using UnityEngine;
using System.Collections;

public class GP_CameraManager : MonoBehaviour {
    private enum CameraState
    {
        zoomingIn,
        zoomingOut,
        steady
    }

    public GameObject m_goBall;
    public float m_fMinimumSize;
    public float m_fPaddingPercent;
    public float m_fSpeed;
    public int m_iFramesToReachGoal;

    private float m_fHorizontalBallDistanceFromOrigin;
    private float m_fVerticalBallDistanceFromOrigin;
    private float m_fAspectRatio;
    private float m_fMaxZoomOutValue;
    private int m_iFramesSinceMove;
    private Camera cameraComp;
    private CameraState state;

	// Use this for initialization
	void Start () {
        m_fAspectRatio = 16f / 9f;
        m_iFramesSinceMove = 0;
        m_fMaxZoomOutValue = 0;
        state = CameraState.steady;
        cameraComp = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        //Horizontal distance is divided by the aspect ratio so that we can compare the two distances.
        m_fHorizontalBallDistanceFromOrigin = Mathf.Abs(m_goBall.transform.position.x) / m_fAspectRatio;
        m_fVerticalBallDistanceFromOrigin = Mathf.Abs(m_goBall.transform.position.y);

        float maxDistance = Mathf.Max(m_fHorizontalBallDistanceFromOrigin, m_fVerticalBallDistanceFromOrigin);
        //HandleCameraInterp(maxDistance);
        HandleCameraInterp_simple(maxDistance);
	}

    public void ResetCamera()
    {
        state = CameraState.steady;
        cameraComp.orthographicSize = m_fMinimumSize;
    }

    private void HandleCameraInterp_simple(float ballDistanceFromOrigin)
    {
        if (ballDistanceFromOrigin * (1 + m_fPaddingPercent) > cameraComp.orthographicSize )
        {
            cameraComp.orthographicSize += m_fSpeed * Time.deltaTime;
            //cameraComp.orthographicSize = Mathf.Min(cameraComp.orthographicSize, ballDistanceFromOrigin * (1 + m_fPaddingPercent));
        }

        if (ballDistanceFromOrigin < cameraComp.orthographicSize )
        {
            cameraComp.orthographicSize -= m_fSpeed * Time.deltaTime;            
        }

        cameraComp.orthographicSize = Mathf.Max(cameraComp.orthographicSize, m_fMinimumSize);
    }

    private void HandleCameraInterp(float ballDistanceFromOrigin)
    {
        switch(state)
        {
            case CameraState.zoomingOut:
                if( ballDistanceFromOrigin * (1f + m_fPaddingPercent) <= m_fMinimumSize )
                {
                    state = CameraState.zoomingIn;
                    //Debug.Log("CAMERA STATE: zooming in");
                    m_iFramesSinceMove = 0;
                }
                else
                {
                    float paddedDistance = ballDistanceFromOrigin * (1f + m_fPaddingPercent);
                    m_fMaxZoomOutValue = Mathf.Max(paddedDistance, m_fMaxZoomOutValue);
                    float t = (float)m_iFramesSinceMove / (float)m_iFramesToReachGoal;
                    m_iFramesSinceMove = Mathf.Min(++m_iFramesSinceMove, m_iFramesToReachGoal);
                    GetComponent<Camera>().orthographicSize = Mathf.SmoothStep(m_fMinimumSize, m_fMaxZoomOutValue, t);
                }
                break;
            case CameraState.zoomingIn:
                Vector2 nextPos = new Vector2(m_goBall.transform.position.x, m_goBall.transform.position.y)
                    + m_goBall.GetComponent<Rigidbody2D>().velocity;
                float nextHorizontalDistance = Mathf.Abs(nextPos.x) / m_fAspectRatio;
                float nextVerticalDistance = Mathf.Abs(nextPos.y);
                float newMaxDistance = Mathf.Max(ballDistanceFromOrigin, nextHorizontalDistance);
                newMaxDistance = Mathf.Max(newMaxDistance, nextVerticalDistance);

                if( newMaxDistance > m_fMinimumSize 
                    && ballDistanceFromOrigin * (1f + m_fPaddingPercent) > m_fMinimumSize 
                    && newMaxDistance > ballDistanceFromOrigin )
                {
                    state = CameraState.zoomingOut;
                    m_fMaxZoomOutValue = newMaxDistance;
                    //m_iFramesSinceMove = Mathf.RoundToInt(Mathf.InverseLerp(m_fMinimumSize, m_fMaxZoomOutValue, ballDistanceFromOrigin) * m_iFramesToReachGoal);
                    //m_iFramesSinceMove = 0;
                    m_iFramesSinceMove = Mathf.CeilToInt(Mathf.InverseLerp(m_fMinimumSize, m_fMinimumSize, ballDistanceFromOrigin) * m_iFramesToReachGoal);
                }
                else if( ballDistanceFromOrigin <= m_fMinimumSize && m_iFramesSinceMove >= m_iFramesToReachGoal)
                {
                    state = CameraState.steady;
                    //Debug.Log("CAMERA STATE: steady");
                    m_fMaxZoomOutValue = 0;
                    m_iFramesSinceMove = 0;
                }
                else
                {
                    float t = (float)m_iFramesSinceMove / (float)m_iFramesToReachGoal;
                    m_iFramesSinceMove = Mathf.Min(++m_iFramesSinceMove, m_iFramesToReachGoal);
                    GetComponent<Camera>().orthographicSize = Mathf.SmoothStep(m_fMaxZoomOutValue, m_fMinimumSize, t);
                }
                break;
            case CameraState.steady:
            default:
                if( ballDistanceFromOrigin * (1f + m_fPaddingPercent) > m_fMinimumSize )
                {
                    state = CameraState.zoomingOut;
                    //Debug.Log("CAMERA STATE: zooming out");
                    m_fMaxZoomOutValue = ballDistanceFromOrigin * (1f + m_fPaddingPercent);
                }
                break;
        }
    }
}
