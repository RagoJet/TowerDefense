﻿using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDescription", menuName = "WeaponDescription")]
public class WeaponDescription : ScriptableObject{
    public int damage;
    public float attackDelay;
    public float rangeAttack;

    public GameObject prefab;
}