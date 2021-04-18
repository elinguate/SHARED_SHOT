using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandboxGun : MonoBehaviour
{
    public enum State
    {
        PASSIVE,
        FIRING,
        RELOADING
    }

    [Header("External Components")]
    public SandboxGunData m_Data;
    public GameObject m_ProjectilePrefab;

    [Header("Internal Components")]
    public Transform m_Gun;
    public Transform m_Target;
    public float m_TargetMoveSpeed = 8.0f;
    public float m_TargetRotateSpeed = 32.0f;
    public Transform m_FiringOrigin;

    [Header("Input")]
    public bool m_AttemptingFire;
    public bool m_AttemptingReload;

    [Header("State")]
    public State m_State;
    
    public int m_Clip = 0;
    public int m_Reserve = 0;

    public float m_FiringTimer = 0.0f;
    public bool m_Fired = false;
    public float m_ReloadTimer = 0.0f;

    [Header("Recoil")]
    public float m_RecoilAngle = 30.0f;
    public float m_RecoilReturnSpeed = 5.0f;

    [Header("Reloading")]
    public Vector2 m_ReloadingHeights;

    [Header("Perspective Adjustment")]
    public Transform m_PerspectiveTransform;
    public Vector2 m_PerspectiveDistanceExtents = new Vector2(2.0f, 100.0f);
    public LayerMask m_TargetableLayerMask;

    [Header("UI")]
    public Image m_ReticuleDisplay;
    public Text m_NameDisplay;
    public Text m_StateDisplay;
    public Text m_AmmoDisplay;

    public GameObject m_HitMarkerPrefab;

    public void ResetGunState()
    {
        m_State = State.PASSIVE;

        m_Clip = m_Data.m_ClipSize;
        m_Reserve = m_Data.m_ReserveSize;

        m_FiringTimer = 0.0f;
        m_Fired = false;
        m_ReloadTimer = 0.0f;

        m_ReticuleDisplay.sprite = m_Data.m_ReticuleSprite;

        m_NameDisplay.text = m_Data.m_DisplayName;
    }

    public void RegisterHit()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject hitMarker = Instantiate(m_HitMarkerPrefab, m_ReticuleDisplay.transform.position, Quaternion.Euler(0.0f, 0.0f, (float)i * 90.0f));
            hitMarker.GetComponent<RectTransform>().SetParent(m_ReticuleDisplay.transform);
            hitMarker.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
        }
    }

    private bool CanFire()
    {
        bool infiniteClip = m_Data.m_ClipSize < 0;
        bool notEmpty = m_Clip > 0;
        bool stateFree = m_State != State.FIRING;

        return (notEmpty || infiniteClip) && stateFree; 
    }

    private bool CanReload()
    {
        bool notFull = m_Clip < m_Data.m_ClipSize;
        bool notEmptyReserve = m_Reserve > 0;
        bool stateFree = m_State == State.PASSIVE;

        return notFull && notEmptyReserve && stateFree; 
    }

    private bool ShouldAutoReload()
    {
        bool emptyReload = m_Clip <= 0 && m_Data.m_AutoReloadAtEmpty;
        bool generalReload = m_Clip < m_Data.m_ClipSize && m_Data.m_AutoReload;
        bool stateFree = m_State == State.PASSIVE;

        return (emptyReload || generalReload) && stateFree;
    }

    private void ProcessInput()
    {
        bool cachedAttemptingFire = m_AttemptingFire;
        m_AttemptingFire = Input.GetKey(KeyCode.Mouse0);
        m_AttemptingReload = Input.GetKeyDown(KeyCode.R);

        if (m_AttemptingFire && CanFire())
        {
            m_State = State.FIRING;
            if (!cachedAttemptingFire)
            {
                m_FiringTimer = 0.0f;
            }
            m_Fired = false;
        }

        if ((m_AttemptingReload || ShouldAutoReload()) && CanReload())
        {
            m_State = State.RELOADING;
            m_ReloadTimer = 0.0f;
        }
    }

    private Quaternion GenerateFireAngle()
    {
        Quaternion cacheRotation = m_FiringOrigin.localRotation;

        RaycastHit hit;
        Vector3 target = m_PerspectiveTransform.position + m_PerspectiveTransform.forward * m_PerspectiveDistanceExtents.x;
        if (Physics.Raycast(m_PerspectiveTransform.position, m_PerspectiveTransform.forward, out hit, m_PerspectiveDistanceExtents.y, m_TargetableLayerMask))
        {
            if (hit.distance > m_PerspectiveDistanceExtents.x)
            {
                target = hit.point;
            }
        }
        m_FiringOrigin.LookAt(target);

        float angle = Random.Range(0.0f, 360.0f);
        float spread = Random.Range(0.0f, m_Data.m_SpreadAngle);

        m_FiringOrigin.Rotate(Vector3.forward, angle, Space.Self);
        m_FiringOrigin.Rotate(Vector3.right, spread, Space.Self);

        //Debug.DrawLine(m_FiringOrigin.position, m_FiringOrigin.position + m_FiringOrigin.forward, Color.yellow, 5.0f);

        Quaternion firingAngle = m_FiringOrigin.rotation;
        m_FiringOrigin.localRotation = cacheRotation;
        return firingAngle;
    }

    private void SpawnProjectile()
    {
        bool isCrit = Random.Range(0.0f, 100.0f) < m_Data.m_CriticalChance;
        int damage = Random.Range(m_Data.m_DamageExtents.x, m_Data.m_DamageExtents.y + 1);
        int critDamage = isCrit ? Mathf.CeilToInt(m_Data.m_CriticalDamage * 0.01f * (float)damage) : 0;
        GameObject projectile = Instantiate(m_ProjectilePrefab, m_FiringOrigin.position, GenerateFireAngle());
        projectile.GetComponent<SandboxProjectile>().Initialize(this, m_Data.m_Speed, damage + critDamage, isCrit, 0.0f);
    }

    private void Fire()
    {
        for (int i = 0; i < m_Data.m_ProjectilesPerShot; i++)
        {
            SpawnProjectile();
        }
        
        int ammoCost = Mathf.Min(m_Clip, m_Data.m_AmmoCostPerShot);
        m_Clip -= ammoCost;

        m_Gun.localEulerAngles += new Vector3(-m_RecoilAngle, 0.0f, 0.0f);
    }

    private void Reload()
    {
        int missingAmount = m_Data.m_ClipSize - m_Clip;
        int perReload = m_Data.m_ReloadAmount < 0 ? m_Data.m_ClipSize : m_Data.m_ReloadAmount; 
        int reloadAmount = Mathf.Min(perReload, missingAmount);
        reloadAmount = Mathf.Min(reloadAmount, m_Reserve);
        m_Clip += reloadAmount;
        m_Reserve -= reloadAmount;
    }

    private void ProcessState()
    {
        switch (m_State)
        {
            case State.FIRING:
            {
                if (m_FiringTimer > (m_Data.m_PreFireLength + m_Data.m_PostFireLength))
                {
                    m_FiringTimer -= (m_Data.m_PreFireLength + m_Data.m_PostFireLength);
                    m_State = State.PASSIVE;
                }
                else if (!m_Fired && m_FiringTimer > m_Data.m_PreFireLength)
                {
                    Fire();
                    m_Fired = true;
                }
                
                m_FiringTimer += Time.deltaTime;
            } break;
            case State.RELOADING:
            {
                if (m_ReloadTimer > m_Data.m_ReloadLength)
                {
                    Reload();
                    m_State = State.PASSIVE;
                }

                m_ReloadTimer += Time.deltaTime;
            } break;
            default: {} break;
        }
    }

    private void DisplayUI()
    {
        m_StateDisplay.text = m_State.ToString();
        m_AmmoDisplay.text = m_Clip.ToString() + " / " + m_Reserve.ToString();
    }

    private void UpdateProcess()
    {
        ProcessInput();
        ProcessState();

        Vector3 cachePos = m_Target.localPosition;
        cachePos.y = m_State == State.RELOADING ? m_ReloadingHeights.y : m_ReloadingHeights.x;
        m_Target.localPosition = cachePos;

        m_Gun.rotation = Quaternion.Slerp(m_Gun.rotation, m_Target.rotation, m_TargetRotateSpeed * Time.deltaTime);
        m_Gun.position = Vector3.Lerp(m_Gun.position, m_Target.position, m_TargetMoveSpeed * Time.deltaTime);

        DisplayUI();
    }

    private void Update()
    {
        UpdateProcess();
    }
}
