﻿using System.Collections.Generic;

public class LazyEnemyPool{
    readonly Dictionary<EnemyData, Queue<Enemy>> _enemyDictionary = new();

    public void Init(){
        for (int levelRace = 0; levelRace < 4; levelRace++){
            for (int levelUnit = 0; levelUnit < 7; levelUnit++){
                _enemyDictionary[new EnemyData(levelRace, levelUnit)] = new Queue<Enemy>();
            }
        }
    }

    public void HideEnemy(Enemy enemy){
        Queue<Enemy> queue = _enemyDictionary[enemy.GetLevelData()];
        queue.Enqueue(enemy);
        enemy.gameObject.SetActive(false);
    }

    public Enemy GetEnemy(EnemyData enemyData){
        if (_enemyDictionary[enemyData].TryDequeue(out var enemy)){
            enemy.gameObject.SetActive(true);
        }

        return enemy;
    }
}