using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxProjectile : MonoBehaviour
{
    public SandboxGun m_AttachedGun;

    public float m_MoveSpeed = 0.0f;
    public int m_Damage;
    public bool m_IsCritical;

    public float m_Radius;

    public LayerMask m_HittableLayerMask;
    
    public GameObject m_HitParticles;
    public GameObject m_HitDecalProjector;

    public void Initialize(SandboxGun _gun, float _speed, int _damage, bool _isCrit, float _immediateStep)
    {
        m_AttachedGun = _gun;
        
        m_MoveSpeed = _speed;
        m_Damage = _damage;
        m_IsCritical = _isCrit;

        Process(_immediateStep);
    }

    private void Process(float _dT)
    {
        Vector3 step = transform.forward * m_MoveSpeed * _dT;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, m_Radius, step.normalized, out hit, step.magnitude, m_HittableLayerMask))
        {
            CharacterHealth hitHealth = hit.transform.GetComponent<CharacterHealth>();
            if (hitHealth)
            {
                hitHealth.TakeDamage(m_Damage, m_IsCritical, hit.point, hit.normal); 
                m_AttachedGun.RegisterHit();
            }
            else
            {
                Instantiate(m_HitDecalProjector, hit.point, Quaternion.LookRotation(-hit.normal));
                Instantiate(m_HitParticles, hit.point, Quaternion.LookRotation(hit.normal));
            }
            Destroy(gameObject);
        }

        transform.position += step;
    }

    private void Update()
    {
        Process(Time.deltaTime);
    }
}
