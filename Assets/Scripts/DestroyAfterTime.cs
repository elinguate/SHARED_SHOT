using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float m_LifeTime = 1.0f;
    
    private void Update()
    {
        m_LifeTime -= Time.deltaTime;
        if (m_LifeTime < 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
