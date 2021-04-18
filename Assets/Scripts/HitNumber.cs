using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitNumber : MonoBehaviour
{
    public Transform m_LookAt;
    public TextMesh m_TextDisplay;

    public bool m_IsCritical = false;

    public float m_LifeLength = 0.5f;
    private float m_LifeTimer;

    public Gradient m_ColourGradient;
    public Gradient m_CritColourGradient;
    public float m_SizeModifier = 0.01f;
    public AnimationCurve m_SizeCurve;

    public Vector3 m_Velocity;
    public Vector2 m_UpwardsSpeedExtent = new Vector2(1.0f, 2.0f);
    public Vector2 m_OutwardsSpeedExtent = new Vector2(1.0f, 2.0f);
    public float m_Gravity = 2.0f;
    public float m_MinYSpeed = -10.0f;
    public float m_OutwardsFriction = 1.0f;

    private void Awake()
    {
        m_Velocity.y = Random.Range(m_UpwardsSpeedExtent.x, m_UpwardsSpeedExtent.y);
        
        float angle = Random.Range(0.0f, 2.0f * Mathf.PI);
        float strength = Random.Range(m_OutwardsSpeedExtent.x, m_OutwardsSpeedExtent.y);
        m_Velocity.x = Mathf.Sin(angle) * strength;
        m_Velocity.z = Mathf.Cos(angle) * strength;

        m_LookAt = Camera.main.transform;
        
        m_LifeTimer = m_LifeLength;

        UpdateState();
    }

    public void Initialize(int _damage, bool _isCrit)
    {
        m_TextDisplay.text = _damage.ToString();
        m_IsCritical = _isCrit;
    }

    private void UpdateState()
    {
        transform.position += m_Velocity * Time.deltaTime;

        float sample = 1.0f - (m_LifeTimer / m_LifeLength);
        m_TextDisplay.color = (m_IsCritical ? m_CritColourGradient : m_ColourGradient).Evaluate(sample);
        float size = m_SizeCurve.Evaluate(sample) * m_SizeModifier;
        m_TextDisplay.transform.localScale = new Vector3(-size, size, 1.0f);

        m_TextDisplay.transform.LookAt(m_LookAt);
    }

    private void Update()
    {
        if (m_LifeTimer < 0.0f)
        {
            Destroy(gameObject);
        }

        m_Velocity.y -= m_Gravity * Time.deltaTime;

        m_Velocity.x -= Mathf.Sign(m_Velocity.x) * m_OutwardsFriction * Time.deltaTime;   
        m_Velocity.z -= Mathf.Sign(m_Velocity.z) * m_OutwardsFriction * Time.deltaTime;

        UpdateState();

        m_LifeTimer -= Time.deltaTime;
    }
}
