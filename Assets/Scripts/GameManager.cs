using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour{
    [SerializeField] private EnemiesFactory enemiesFactory;
    private int _currentGameLevel = 1;
    private int _maxGameLevel = 40;
    private int _maxLevelUnit = 6; //first 0;
    private int _maxLevelRace = 3; //first 0;


    private void Start(){
        StartLevel(4);
    }


    public void StartLevel(int level){
        switch (level){
            case <10:
                StartHumanWave(level);
                break;
            case < 20:
                StartElfWave(level);
                break;
            case <30:
                StartUndeadWave(level);
                break;
            case <40:
                StartOrcWave(level);
                break;
        }
    }

    private void StartHumanWave(int level){
        EnemyData humanEnemyData = new EnemyData(0, 0);
        int levelOfUnit = Mathf.Clamp(level, 0, _maxLevelUnit);
        for (int i = 0; i < level; i++){
            humanEnemyData.levelOfUnit = Random.Range(0, levelOfUnit);
            enemiesFactory.CreateAndDirectEnemy(humanEnemyData);
        }
    }

    private void StartElfWave(int level){
        EnemyData elfEnemyData = new EnemyData(1, 0);
        int levelOfUnit = Mathf.Clamp(level - 10, 0, _maxLevelUnit);
        for (int i = 0; i < level * 0.4; i++){
            elfEnemyData.levelOfUnit = Random.Range(0, levelOfUnit);
            enemiesFactory.CreateAndDirectEnemy(elfEnemyData);
        }
    }

    private void StartUndeadWave(int level){
        EnemyData undeadEnemyData = new EnemyData(2, 0);
        int levelOfUnit = Mathf.Clamp(level - 20, 0, _maxLevelUnit);
        for (int i = 0; i < level * 0.3; i++){
            undeadEnemyData.levelOfUnit = Random.Range(0, levelOfUnit);
            enemiesFactory.CreateAndDirectEnemy(undeadEnemyData);
        }
    }

    private void StartOrcWave(int level){
        EnemyData orcEnemyData = new EnemyData(3, 0);
        int levelOfUnit = Mathf.Clamp(level - 30, 0, _maxLevelUnit);
        for (int i = 0; i < level * 0.2; i++){
            orcEnemyData.levelOfUnit = Random.Range(0, levelOfUnit);
            enemiesFactory.CreateAndDirectEnemy(orcEnemyData);
        }
    }
}