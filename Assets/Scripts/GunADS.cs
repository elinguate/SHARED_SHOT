using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunADS : MonoBehaviour
{
    public SandboxGun gun;
    public Camera maincamera;
    public CharacterMotorData MotorData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            maincamera.fieldOfView = 50;
            MotorData.m_MoveSpeed = 3;
        }
        else
        {
            maincamera.fieldOfView = 100;
            MotorData.m_MoveSpeed = 12;
        }
    }
}
