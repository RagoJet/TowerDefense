using System.Collections.Generic;
using UnityEngine;


public class EnemiesFactory : MonoBehaviour{
    private EnemyDescriptions enemyDescriptions;
    private King theKing;
    private GameObject theGate;
    private GameManager gameManager;

    private readonly LazyEnemyPool _lazyEnemyPool = new();
    private readonly List<Enemy> _listOfAliveEnemies = new();
    public List<Enemy> ListOfAliveEnemies => _listOfAliveEnemies;

    public void Construct(EnemyDescriptions enemyDescriptions, King king, GameObject gate, GameManager gameManager){
        this.enemyDescriptions = enemyDescriptions;
        this.theKing = king;
        this.theGate = gate;
        this.gameManager = gameManager;
        _lazyEnemyPool.Init();
    }


    public void DieAndClearAllEnemies(){
        foreach (var enemy in _listOfAliveEnemies){
            enemy.PlayDeathState();
        }

        _listOfAliveEnemies.Clear();
    }

    private void HideEnemy(Enemy enemy){
        enemy.OnDie -= HideEnemy;
        _listOfAliveEnemies.Remove(enemy);
        gameManager.OnDieEnemy();
        _lazyEnemyPool.HideEnemy(enemy);
    }

    public void CreateAndDirectEnemy(EnemyData enemyData){
        Enemy enemy = _lazyEnemyPool.GetEnemy(enemyData);
        if (enemy == null){
            enemy = CreateEnemy(enemyData);
        }

        enemy.OnDie += HideEnemy;
        _listOfAliveEnemies.Add(enemy);

        if (enemy.GetEnemyData().levelOfRace < 4){
            enemy.Construct(theGate.transform);
            return;
        }

        enemy.Construct(theKing, theGate.transform, enemyDescriptions.ListOfLastMonster[enemyData.levelOfUnit],
            gameManager.CurrentGameLevel);
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
            case 4:
                enemy = Instantiate(enemyDescriptions.ListOfLastMonster[levelOfUnit].enemyPrefab);
                enemy.Construct(theKing, theGate.transform, enemyDescriptions.ListOfLastMonster[levelOfUnit],
                    gameManager.CurrentGameLevel);
                return enemy;
        }

        return null;
    }
}