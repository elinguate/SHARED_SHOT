using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public CursorLock m_CursorLock;

    public float m_Sensitivity = 2.0f;
    public float m_Spin;
    public float m_Tilt;
    public float m_TargetLean;
    public float m_LeanAngle = 2.5f;
    public float m_LeanSpeed = 8.0f;
    public float m_Lean;
    public Vector2 m_TiltExtents = new Vector2(-95.0f, 95.0f);

    private void UpdateLook()
    {
        m_Tilt = Mathf.Clamp(m_Tilt, m_TiltExtents.x, m_TiltExtents.y);
        transform.localEulerAngles = new Vector3(m_Tilt, m_Spin, m_Lean * m_LeanAngle);
    }

    private void OnValidate()
    {
        UpdateLook();
    }

    private void Update()
    {
        if (!m_CursorLock.m_Locked)
        {
            return;
        }

        float x = Input.GetAxisRaw("Mouse X");
        float y = Input.GetAxisRaw("Mouse Y");

        m_Spin += x * m_Sensitivity;
        m_Tilt += -y * m_Sensitivity;
        m_Lean = Mathf.Lerp(m_Lean, m_TargetLean, m_LeanSpeed * Time.deltaTime);

        UpdateLook(); 
    }
}
