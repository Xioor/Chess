using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera m_Camera;
    public GameObject m_FocusPoint;
    public float m_ZoomSensitivity = 0.5f;
    public float m_CameraZoom = 50;
    public float m_Padding;
    bool m_UseStep;

    BoundingSphere m_CurrentBoundingSphere;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IncreaseZoomStep()
    {
        
    }

    public void ChangeZoom(float zoomAmount) 
    {
        //Check If we are zooming by step or fine controll.
        m_CameraZoom -= zoomAmount * m_ZoomSensitivity;
    }

    public void FocusPiece(GameObject Piece)
    {
        
    }

    public void FocusAttack(GameObject Px, GameObject Py)
    {
        Vector3 center =  Px.transform.position - Py.transform.position;
        float Rad = Mathf.Abs(Vector3.Distance(Px.transform.position, Py.transform.position) * 0.5f);
        m_CurrentBoundingSphere = new BoundingSphere(center, Rad);
    }

    void DecreaseZoomStep()
    {
        
    }

}
