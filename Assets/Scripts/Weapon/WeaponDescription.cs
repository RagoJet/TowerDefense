using UnityEngine;


[CreateAssetMenu(fileName = "WeaponDescription", menuName = "WeaponDescription")]
public class WeaponDescription : ScriptableObject{
    public int level;
    public int damage;
    public float attackDelay;

    public Weapon weaponPrefab;
}