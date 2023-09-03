using UnityEngine;


[CreateAssetMenu(fileName = "EnemyDescription", menuName = "EnemyDescription")]
public class EnemyDescription : ScriptableObject{
    public int damage;
    public int health;
    public int moveSpeed;
    public float rangeAttack;

    public Enemy enemyPrefab;
}