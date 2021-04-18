using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour
{
    public Image m_HitMarkerDisplay;

    public float m_LifeLength = 0.25f;
    private float m_LifeTimer;

    public Gradient m_ColourGradient;
    public float m_SizeModifier = 1.0f;
    public AnimationCurve m_SizeCurve;
    public float m_PositionModifier = 10.0f;
    public AnimationCurve m_PositionCurve;

    private void Awake()
    {
        m_LifeTimer = m_LifeLength;

        UpdateState();
    }

    private void UpdateState()
    {
        float sample = 1.0f - (m_LifeTimer / m_LifeLength);

        transform.localPosition = transform.up * m_PositionCurve.Evaluate(sample) * m_PositionModifier;

        m_HitMarkerDisplay.color = m_ColourGradient.Evaluate(sample);
        float size = m_SizeCurve.Evaluate(sample) * m_SizeModifier;
        m_HitMarkerDisplay.transform.localScale = new Vector3(1.0f, size, 1.0f);
    }

    private void Update()
    {
        if (m_LifeTimer < 0.0f)
        {
            Destroy(gameObject);
        }

        UpdateState();

        m_LifeTimer -= Time.deltaTime;
    }
}
