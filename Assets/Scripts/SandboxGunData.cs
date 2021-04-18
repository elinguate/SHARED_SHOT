using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SHOT/Gun Data", fileName = "Default Gun Data")]
public class SandboxGunData : ScriptableObject
{
    [Header("General")]

    [Tooltip("The human-readable name of our gun.")]
    public string m_DisplayName = "Gun Name";
    
    [Header("Gameplay")]

    [Tooltip("How long in seconds after beginning a shot before the projectile is launched.")]
    public float m_PreFireLength = 0.0f;
    [Tooltip("How long in seconds after the projectile is launched before the gun can be fired again.")]
    public float m_PostFireLength = 0.1f;

    [Tooltip("How many projectiles are launched per discrete shot.")]
    public int m_ProjectilesPerShot = 1;
    [Tooltip("How much each shot costs in ammo.")]
    public int m_AmmoCostPerShot = 1;

    [Tooltip("How large our clip is. A value of -1 indicates it will be infinite.")] 
    public int m_ClipSize = 12;
    [Tooltip("How large our bullet reserve is. A value of -1 indicates it will be infinite.")] 
    public int m_ReserveSize = 48;
    
    [Tooltip("How long in seconds a reload takes.")] 
    public float m_ReloadLength = 0.5f;
    [Tooltip("How much ammo we replenish per reload. A value of -1 indicates it will reload the full clip.")] 
    public int m_ReloadAmount = -1;
    [Tooltip("Whether our gun automatically reloads when not firing.")]
    public bool m_AutoReload = false;
    [Tooltip("Whether our gun automatically reloads when empty.")]
    public bool m_AutoReloadAtEmpty = true;

    [Tooltip("How much variance in degrees from the center a shot can have.")]
    public float m_SpreadAngle = 0.5f; // TO DO: implement spread

    [Header("Projectile")]

    [Tooltip("The range of damage our projectiles will deal. Inclusive to both numbers.")]
    public Vector2Int m_DamageExtents = new Vector2Int(10, 12); // TO DO: implement range usage

    [Tooltip("What percentage of our projectiles will cause critical hits. A value of 100 indicates guaranteed critical hits.")]
    public float m_CriticalChance = 10.0f; // TO DO: implement critting
    [Tooltip("What percentage extra damage our critical hits deal. A value of 100 indicates critical hits cause double damage.")]
    public float m_CriticalDamage = 40.0f; // TO DO: implement crit multiplier

    [Tooltip("How fast our projectiles will move through the scene in units per second. A value of -1 indicates it will be hitscan.")]
    public float m_Speed = 20.0f;

    [Header("Reticule")]

    [Tooltip("The sprite our reticule on our UI will utilise.")]
    public Sprite m_ReticuleSprite;
}
