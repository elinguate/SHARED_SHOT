using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunADS : MonoBehaviour
{
    public SandboxGunData gun;
    public Camera maincamera;
    public CharacterMotorData MotorData;

    float originalspread;
    // Start is called before the first frame update
    void Start()
    {
        originalspread = gun.m_SpreadAngle;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            maincamera.fieldOfView = 50;
            MotorData.m_MoveSpeed = 3;
            gun.m_SpreadAngle /= 5;
        }
        else
        {
            maincamera.fieldOfView = 100;
            MotorData.m_MoveSpeed = 12;
            gun.m_SpreadAngle = originalspread;
        }
    }
}
