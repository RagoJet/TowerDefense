using System;
using UnityEngine;

[Serializable]
public struct EnemyData{
    public EnemyData(int _levelOfRace, int _levelOfUnit){
        levelOfRace = _levelOfRace;
        levelOfUnit = _levelOfUnit;
    }

    public int levelOfUnit;
    public int levelOfRace;
}


[CreateAssetMenu(fileName = "EnemyDescription", menuName = "EnemyDescription")]
public class EnemyDescription : ScriptableObject{
    public EnemyData enemyData;
    public int damage;
    public int maxHealth;
    public int gold;

    public Enemy enemyPrefab;
}