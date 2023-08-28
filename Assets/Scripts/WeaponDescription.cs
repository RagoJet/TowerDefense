using System;
using UnityEngine;

[Serializable]
public enum TypeWeapon{
    Catapult = 1,
    RocketLauncher = 2,
    BigRocketLauncher = 3
}

[Serializable]
public struct DataWeapon{
    public TypeWeapon _typeWeapon;
    public int _level;

    public DataWeapon(TypeWeapon typeWeapon, int level){
        _typeWeapon = typeWeapon;
        _level = level;
    }
}

[CreateAssetMenu(fileName = "WeaponDescription", menuName = "WeaponDescription")]
public class WeaponDescription : ScriptableObject{
    public DataWeapon _dataWeapon;

    public int damage;
    public float attackDelay;
    public float cooldown;
    public float rangeAttack;

    public Bullet bulletPrefab;
    public Weapon weaponPrefab;
}