using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SHOT/Motor Data", fileName = "Default Motor Data")]
public class CharacterMotorData : ScriptableObject
{
    public float m_MoveSpeed = 12.0f;
    public float m_Acceleration = 12.0f;
    public AnimationCurve m_FrictionCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public float m_StopFriction = 2.0f;

    public float m_Gravity = 30.0f;
    public float m_JumpSpeed = 12.0f;

    public float m_CoyoteLength = 0.25f;
    public float m_HeadBumpCooldown = 0.1f;
}
