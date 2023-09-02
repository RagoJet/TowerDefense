using System;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponDescription", menuName = "WeaponDescription")]
public class WeaponDescription : ScriptableObject{
    public int level;
    public int damage;
    public float attackDelay;
    public float cooldown;
    public float rangeAttack;

    public Bullet bulletPrefab;
    public Weapon weaponPrefab;
}