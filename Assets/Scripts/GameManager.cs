using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum GameState{
    Playing,
    Stop
}

public class GameManager : MonoBehaviour, ISaveable{
    [SerializeField] private WeaponsFactory weaponsFactory;
    [SerializeField] private WeaponDescriptions weaponDescriptions;
    [SerializeField] private EnemiesFactory enemiesFactory;
    [SerializeField] private EnemyDescriptions enemyDescriptions;


    [SerializeField] private Shop shop;

    [SerializeField] private KingHealthUI kingHealthUI;
    [SerializeField] private King theKing;
    [SerializeField] private GameObject theGate;
    [SerializeField] private Cells cells;

    [SerializeField] private Button restartButton;

    public GameState state = GameState.Stop;
    private int _currentGameLevel = 1;

    public int countOfAliveEnemies;

    private DataContainer _dataContainer;
    private SaveLoadController _saveLoadController;
    private SaveablesObjects _saveablesObjects;

    private void Awake(){
        _saveLoadController = new SaveLoadController();
        _dataContainer = _saveLoadController.GetDataContainerFromFile();
        _saveablesObjects = new SaveablesObjects(_dataContainer, this, weaponsFactory, theKing, shop);

        weaponsFactory.Construct(weaponDescriptions, cells);
        enemiesFactory.Construct(enemyDescriptions, shop, theKing, theGate, this);


        _saveablesObjects.LoadAllDataFromContainer();

        theKing.Construct(this);
        kingHealthUI.Construct(theKing);
        shop.Construct(weaponsFactory, theKing);

        state = GameState.Playing;
        restartButton.image.color = Color.gray;
        StartLevel(_currentGameLevel);
    }


    public void StopGame(){
        state = GameState.Stop;
        restartButton.image.color = Color.white;
        restartButton.onClick.AddListener(RestartLevel);
    }


    public void StartLevel(int level){
        _saveablesObjects.WriteAllDataToContainer();
        _saveLoadController.SaveDataContainerToFile(_dataContainer);
        theKing.Refresh();
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

    public void OnDieEnemy(){
        if (state == GameState.Playing){
            countOfAliveEnemies--;
            if (countOfAliveEnemies == 0){
                _currentGameLevel++;
                StartLevel(_currentGameLevel);
            }
        }
    }

    public void RestartLevel(){
        if (state == GameState.Stop){
            restartButton.image.color = Color.gray;
            restartButton.onClick.RemoveListener(RestartLevel);
            StartCoroutine(RestartLevelCoroutine());
        }
    }

    IEnumerator RestartLevelCoroutine(){
        enemiesFactory.DieAndClearAllEnemies();
        yield return new WaitForSeconds(2f);
        state = GameState.Playing;
        StartLevel(_currentGameLevel);
    }

    public void LoadDataFromContainer(){
        _currentGameLevel = _dataContainer.currentLevel;
    }

    public void WriteDataToContainer(){
        _dataContainer.currentLevel = _currentGameLevel;
    }

    public void SetDataContainer(DataContainer dataContainer){
        _dataContainer = dataContainer;
    }
}