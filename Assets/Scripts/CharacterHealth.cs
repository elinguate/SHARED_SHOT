using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public int m_MaxHealth = 100;
    public int m_Health;

    public bool m_Invulnerable = false;

    public GameObject m_HitIndicatorPrefab;
    public GameObject m_BloodParticlePrefab;

    private void Awake()
    {
        m_Health = m_MaxHealth;
    }

    public void TakeDamage(int _damage, bool _isCrit, Vector3 _point, Vector3 _normal)
    {
        if (!m_Invulnerable)
        {
            m_Health -= _damage;
            if (m_Health <= 0)
            {
                m_Health = 0;
                Destroy(gameObject);
            }
        }

        GameObject hit = Instantiate(m_HitIndicatorPrefab, _point, Quaternion.LookRotation(_normal));
        hit.GetComponent<HitNumber>().Initialize(_damage, _isCrit); 

        Instantiate(m_BloodParticlePrefab, _point, Quaternion.LookRotation(_normal));
    }
}
