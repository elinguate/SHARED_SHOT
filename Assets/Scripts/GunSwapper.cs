using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwapper : MonoBehaviour
{
    public int m_GunIndex = 0;
    public SandboxGunData[] m_AvailableGuns;

    public SandboxGun m_Gun;

    private void Awake()
    {
        m_Gun.m_Data = m_AvailableGuns[m_GunIndex];
        m_Gun.ResetGunState();
    }

    private void Update()
    {
        int cacheIndex = m_GunIndex;
        m_GunIndex -= Input.GetKeyDown(KeyCode.Q) ? 1 : 0;
        m_GunIndex += Input.GetKeyDown(KeyCode.E) ? 1 : 0;

        m_GunIndex = (m_GunIndex + m_AvailableGuns.Length) % m_AvailableGuns.Length;

        if (cacheIndex != m_GunIndex)
        {
            m_Gun.m_Data = m_AvailableGuns[m_GunIndex];
            m_Gun.ResetGunState();
        }
    }
}
