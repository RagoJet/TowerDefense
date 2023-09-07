using System.Collections.Generic;
using UnityEngine;


public class EnemiesFactory : MonoBehaviour{
    [SerializeField] private EnemyDescriptions enemyDescriptions;

    [SerializeField] private Shop shop;

    [SerializeField] private King theKing;
    [SerializeField] private GameObject theGate;


    private readonly LazyEnemyPool _lazyEnemyPool = new LazyEnemyPool();

    private readonly List<Enemy> _listOfAliveEnemies = new List<Enemy>();
    public List<Enemy> ListOfAliveEnemies => _listOfAliveEnemies;


    private void Start(){
        _lazyEnemyPool.Init();
    }


    public void CreateEnemiesButton(){
        int levelOfUnit = Random.Range(0, 7);
        int levelOfRace = Random.Range(0, 4);
        CreateAndDirectEnemy(new EnemyData(levelOfRace, levelOfUnit));
    }


    private void CreateAndDirectEnemy(EnemyData enemyData){
        Enemy enemy = _lazyEnemyPool.GetEnemy(enemyData);

        if (enemy == null){
            enemy = CreateEnemy(enemyData);
        }

        enemy.OnDie += HideEnemy;
        _listOfAliveEnemies.Add(enemy);
        enemy.Construct(theKing, theGate.transform);
    }


    private void HideEnemy(Enemy enemy){
        _listOfAliveEnemies.Remove(enemy);
        shop.AddGoldFromEnemy(enemy.GetGold());
        enemy.OnDie -= HideEnemy;
        _lazyEnemyPool.HideEnemy(enemy);
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
}