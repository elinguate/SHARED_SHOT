using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour
{
    public bool m_Locked = true;

    private void LockCursor()
    {
        Cursor.lockState = m_Locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !m_Locked;
    }

    private void Awake()
    {
        LockCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            m_Locked = !m_Locked;
            LockCursor();
        }
    }
}
