using System.Collections.Generic;
using UnityEngine;


public class EnemiesFactory : MonoBehaviour{
    private EnemyDescriptions enemyDescriptions;
    private Shop shop;
    private King theKing;
    private GameObject theGate;
    private GameManager gameManager;

    private readonly LazyEnemyPool _lazyEnemyPool = new();
    private readonly List<Enemy> _listOfAliveEnemies = new();
    public List<Enemy> ListOfAliveEnemies => _listOfAliveEnemies;


    public void Construct(EnemyDescriptions enemyDescriptions, Shop shop, King king, GameObject gate,
        GameManager gameManager){
        this.enemyDescriptions = enemyDescriptions;
        this.shop = shop;
        this.theKing = king;
        this.theGate = gate;
        this.gameManager = gameManager;
        _lazyEnemyPool.Init();
    }


    public void CreateAndDirectEnemy(EnemyData enemyData){
        Enemy enemy = _lazyEnemyPool.GetEnemy(enemyData);

        if (enemy == null){
            enemy = CreateEnemy(enemyData);
        }

        enemy.OnDie += HideEnemy;
        _listOfAliveEnemies.Add(enemy);
        enemy.Construct(theGate.transform);
    }


    private void HideEnemy(Enemy enemy){
        _listOfAliveEnemies.Remove(enemy);
        shop.AddGoldFromEnemy(enemy.GetGold());
        enemy.OnDie -= HideEnemy;
        gameManager.OnDieEnemy();
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