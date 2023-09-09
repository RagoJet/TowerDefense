using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour{
    [SerializeField] private WeaponsFactory weaponsFactory;
    [SerializeField] private WeaponDescriptions weaponDescriptions;
    [SerializeField] private EnemiesFactory enemiesFactory;
    [SerializeField] private EnemyDescriptions enemyDescriptions;

    [SerializeField] private Cells cells;

    [SerializeField] private Shop shop;

    [SerializeField] private KingHealthUI kingHealthUI;
    [SerializeField] private King theKing;
    [SerializeField] private GameObject theGate;

    private int _currentGameLevel = 1;

    public int countOfAliveEnemies;


    private void Awake(){
        weaponsFactory.Construct(weaponDescriptions, cells);
        enemiesFactory.Construct(enemyDescriptions, shop, theKing, theGate, this);
        shop.Construct(weaponsFactory, theKing);
        theKing.Init();
        kingHealthUI.Construct(theKing);
    }

    public void OnDieEnemy(){
        countOfAliveEnemies--;
        if (countOfAliveEnemies == 0){
            _currentGameLevel++;
            StartLevel(_currentGameLevel);
        }
    }

    public void WaveButton(){
        StartLevel(_currentGameLevel);
    }


    public void StartLevel(int level){
        switch (level){
            case < 10:
                StartHumanWave(level);
                break;
            case < 20:
                StartElfWave(level);
                break;
            case < 30:
                StartUndeadWave(level);
                break;
            case < 40:
                StartOrcWave(level);
                break;
        }
    }

    private void StartHumanWave(int level){
        countOfAliveEnemies = level;
        EnemyData humanEnemyData = new EnemyData(0, 0);
        int levelOfUnit = Mathf.Clamp(level, 0, 7);
        for (int i = 0; i < countOfAliveEnemies; i++){
            humanEnemyData.levelOfUnit = Random.Range(0, levelOfUnit);
            enemiesFactory.CreateAndDirectEnemy(humanEnemyData);
        }
    }

    private void StartElfWave(int level){
        countOfAliveEnemies = (int) (level * 0.4);
        EnemyData elfEnemyData = new EnemyData(1, 0);
        int levelOfUnit = Mathf.Clamp(level - 10, 0, 7);
        for (int i = 0; i < level * 0.4; i++){
            elfEnemyData.levelOfUnit = Random.Range(0, levelOfUnit);
            enemiesFactory.CreateAndDirectEnemy(elfEnemyData);
        }
    }

    private void StartUndeadWave(int level){
        countOfAliveEnemies = (int) (level * 0.3);
        EnemyData undeadEnemyData = new EnemyData(2, 0);
        int levelOfUnit = Mathf.Clamp(level - 20, 0, 7);
        for (int i = 0; i < countOfAliveEnemies; i++){
            undeadEnemyData.levelOfUnit = Random.Range(0, levelOfUnit);
            enemiesFactory.CreateAndDirectEnemy(undeadEnemyData);
        }
    }

    private void StartOrcWave(int level){
        countOfAliveEnemies = (int) (level * 0.2);
        EnemyData orcEnemyData = new EnemyData(3, 0);
        int levelOfUnit = Mathf.Clamp(level - 30, 0, 7);
        for (int i = 0; i < countOfAliveEnemies; i++){
            orcEnemyData.levelOfUnit = Random.Range(0, levelOfUnit);
            enemiesFactory.CreateAndDirectEnemy(orcEnemyData);
        }
    }
}