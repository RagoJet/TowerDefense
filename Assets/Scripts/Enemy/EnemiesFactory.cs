using System.Collections.Generic;
using UnityEngine;


public class EnemiesFactory : MonoBehaviour{
    [SerializeField] private EnemyDescriptions enemyDescriptions;

    [SerializeField] private King theKing;
    [SerializeField] private GameObject theGate;


    private readonly LazyEnemyPool _lazyEnemyPool = new LazyEnemyPool();

    private readonly List<Enemy> _listOfTargetToShoot = new List<Enemy>();
    public List<Enemy> ListOfTargetToShoot => _listOfTargetToShoot;

    private void Awake(){
        _lazyEnemyPool.Init();
    }


    private void CreateAndDirectEnemy(EnemyData enemyData){
        Enemy enemy = _lazyEnemyPool.GetEnemy(enemyData);

        if (enemy == null){
            enemy = CreateEnemy(enemyData);
        }

        enemy.OnDie += HideEnemy;
        _listOfTargetToShoot.Add(enemy);
        enemy.Construct(theKing, theGate.transform);
    }

    private Enemy CreateEnemy(EnemyData enemyData){
        int levelOfRace = enemyData.levelOfRace;
        int levelOfUnit = enemyData.levelOfUnit;

        Enemy enemy;
        switch (levelOfRace){
            case 0:
                enemy = Instantiate(enemyDescriptions.ListOfHumans[levelOfUnit].enemyPrefab);
                enemy.Construct(theKing, theGate.transform, enemyDescriptions.ListOfHumans[levelOfUnit]);
                return enemy;
            case 1:
                enemy = Instantiate(enemyDescriptions.ListOfElves[levelOfUnit].enemyPrefab);
                enemy.Construct(theKing, theGate.transform, enemyDescriptions.ListOfElves[levelOfUnit]);
                return enemy;
            case 2:
                enemy = Instantiate(enemyDescriptions.ListOfUndead[levelOfUnit].enemyPrefab);
                enemy.Construct(theKing, theGate.transform, enemyDescriptions.ListOfUndead[levelOfUnit]);
                return enemy;
            case 3:
                enemy = Instantiate(enemyDescriptions.ListOfOrcs[levelOfUnit].enemyPrefab);
                enemy.Construct(theKing, theGate.transform, enemyDescriptions.ListOfOrcs[levelOfUnit]);
                return enemy;
        }

        return null;
    }

    private void HideEnemy(Enemy enemy){
        _listOfTargetToShoot.Remove(enemy);
        enemy.OnDie -= HideEnemy;
        _lazyEnemyPool.HideEnemy(enemy);
    }
}