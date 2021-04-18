using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SandboxGun))]
public class SandboxGunEditor : Editor
{
    private void OnSceneGUI()
    {
        SandboxGun sandboxGun = GameObject.FindGameObjectWithTag("Player").GetComponent<SandboxGun>();
        Handles.color = Color.white;
        Handles.DrawLine(sandboxGun.m_FiringOrigin.position, (sandboxGun.m_FiringOrigin.position + sandboxGun.m_FiringOrigin.forward));
    }
}
