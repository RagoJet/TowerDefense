using UnityEngine;


[CreateAssetMenu(fileName = "WeaponDescription", menuName = "WeaponDescription")]
public class WeaponDescription : ScriptableObject{
    public int level;
    public int damage;
    public float attackDelay;
    public int price;

    public Weapon weaponPrefab;
}