using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMotor : MonoBehaviour
{
    public CharacterController m_Controller;
    public MouseLook m_Look;

    public CharacterMotorData m_Data;

    private Vector3 m_Velocity = new Vector3(0.0f, 0.0f, 0.0f);
    private float m_GroundedTimer = 0.0f;

    private float m_HeadBumpTimer;


    [Header("DebugUI")]
    public bool showDebugPosition = false;
    public Text m_ShowPos;

    private void Update()
    {
        float x = 0.0f;
        x -= Input.GetKey(KeyCode.A) ? 1.0f : 0.0f;
        x += Input.GetKey(KeyCode.D) ? 1.0f : 0.0f;
        m_Look.m_TargetLean = -x;
        float z = 0.0f;
        z -= Input.GetKey(KeyCode.S) ? 1.0f : 0.0f;
        z += Input.GetKey(KeyCode.W) ? 1.0f : 0.0f;
        if (m_GroundedTimer > 0.0f && Input.GetKey(KeyCode.Space))
        {
            m_Velocity.y = m_Data.m_JumpSpeed;
            m_HeadBumpTimer = 0.0f;
            m_GroundedTimer = 0.0f;
        }

        Vector3 inputVelocity = new Vector3(x, 0.0f, z);
        inputVelocity = Quaternion.Euler(0.0f, m_Look.m_Spin, 0.0f) * inputVelocity;
        if (inputVelocity.magnitude > 1.0f)
        {
            inputVelocity.Normalize();
        }

        float cacheY = m_Velocity.y;
        m_Velocity.y = 0.0f;

        m_Velocity += inputVelocity * m_Data.m_Acceleration * Time.deltaTime;
        m_Velocity -= m_Velocity.normalized * m_Data.m_FrictionCurve.Evaluate(m_Velocity.magnitude) * m_Data.m_Acceleration * Time.deltaTime;

        m_Velocity.y = cacheY;
        m_Velocity.y -= m_Data.m_Gravity * Time.deltaTime;

        Vector3 trueVelocity = m_Velocity;
        trueVelocity.x *= m_Data.m_MoveSpeed;
        trueVelocity.z *= m_Data.m_MoveSpeed;
        m_Controller.Move(trueVelocity * Time.deltaTime);

        m_GroundedTimer -= Time.deltaTime;
        bool groundCollision = (m_Controller.collisionFlags & CollisionFlags.Below) != 0;
        bool headCollision = (m_Controller.collisionFlags & CollisionFlags.Above) != 0;
        if (groundCollision && !headCollision)
        {
            m_Velocity.y = -0.5f;
            m_GroundedTimer = m_Data.m_CoyoteLength;
        }

        m_HeadBumpTimer -= Time.deltaTime;
        if (m_HeadBumpTimer < 0.0f && headCollision)
        {
            m_Velocity.y = -0.5f;
            m_HeadBumpTimer = m_Data.m_HeadBumpCooldown;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        /*
         * UI to Show the players current velocity, Postion and Rotation.
         * Toggleable with the ShowDebugPostion in UI
         */
        m_ShowPos.enabled = showDebugPosition ? true : false;
        string UIText = "WorldPos: " + transform.position.ToString() + "\nVelocity: " + m_Velocity.ToString() + "\nRotation: " + m_Look.transform.forward;
        m_ShowPos.text = UIText;
    }
}
