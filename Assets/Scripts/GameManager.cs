using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
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

    [SerializeField] private TextMeshProUGUI currentLevelText;
    private string _currentLevelLang;

    private int _currentGameLevel = 1;

    public int CurrentGameLevel => _currentGameLevel;

    public int countOfAliveEnemies;

    private DataContainer _dataContainer;
    private SaveablesObjects _saveablesObjects;

    [SerializeField] private ParticleSystem WaveFXHuman;
    [SerializeField] private ParticleSystem WaveFXElf;
    [SerializeField] private ParticleSystem WaveFXUndead;
    [SerializeField] private ParticleSystem WaveFXOrc;


    private void OnEnable() => YandexGame.GetDataEvent += Init;

    private void OnDisable() => YandexGame.GetDataEvent -= Init;

    private void Awake(){
        if (YandexGame.SDKEnabled == true){
            Init();
        }
    }

    private void Init(){
        _dataContainer = new DataContainer();

        _dataContainer.gold = YandexGame.savesData.gold;
        _dataContainer.cellsInformation = YandexGame.savesData.cellsInformation;
        _dataContainer.currentLevel = YandexGame.savesData.currentLevel;
        _dataContainer.priceHealth = YandexGame.savesData.priceHealth;
        _dataContainer.priceWeapon = YandexGame.savesData.priceWeapon;
        _dataContainer.levelShopWeapon = YandexGame.savesData.levelShopWeapon;
        _dataContainer.maxHealthKing = YandexGame.savesData.maxHealthKing;
        _dataContainer.maxLevelOfCreatedWeapon = YandexGame.savesData.maxLevelOfCreatedWeapon;

        _saveablesObjects = new SaveablesObjects(_dataContainer, this, weaponsFactory, theKing, shop);
        weaponsFactory.Construct(weaponDescriptions, cells, this);
        enemiesFactory.Construct(enemyDescriptions, theKing, theGate, this);
        _saveablesObjects.LoadAllDataFromContainer();

        theKing.Construct(this);
        kingHealthUI.Construct(theKing);
        string lang = YandexGame.savesData.language;
        shop.Construct(weaponsFactory, theKing, lang);
        if (lang.Equals("ru")){
            _currentLevelLang = "Уровень: ";
        }
        else if (lang.Equals("en")){
            _currentLevelLang = "Level: ";
        }

        state = GameState.Playing;
        restartButton.image.color = Color.gray;
        StartLevel(_currentGameLevel);
    }


    public void StopGame(){
        AudioManager.Instance.PlayLoseGameSound();
        state = GameState.Stop;
        restartButton.image.color = Color.white;
        restartButton.onClick.AddListener(RestartLevel);
    }


    public void StartLevel(int level){
        currentLevelText.text = _currentLevelLang + _currentGameLevel;
        AudioManager.Instance.PlayStartLevelSound();
        theKing.Refresh();
        switch (level){
            case 1:
                StartHumanWave(level);
                weaponsFactory.TryCreateWeapon();
                break;
            case < 11:
                StartHumanWave(level);
                break;
            case < 21:
                StartElfWave(level);
                break;
            case < 31:
                StartUndeadWave(level);
                break;
            case < 41:
                StartOrcWave(level);
                break;
            default:
                StartLastMonstersWave(level);
                break;
        }
    }

    private void StartHumanWave(int level){
        WaveFXHuman.Play();
        countOfAliveEnemies = level;
        EnemyData humanEnemyData = new EnemyData(0, 0);
        int levelOfUnit = Mathf.Clamp(level, 0, 7);
        for (int i = 0; i < countOfAliveEnemies; i++){
            humanEnemyData.levelOfUnit = Random.Range(0, levelOfUnit);
            enemiesFactory.CreateAndDirectEnemy(humanEnemyData);
        }
    }

    private void StartElfWave(int level){
        WaveFXElf.Play();
        countOfAliveEnemies = (int) (level * 0.4f);
        EnemyData elfEnemyData = new EnemyData(1, 0);
        int levelOfUnit = Mathf.Clamp(level - 10, 0, 7);
        for (int i = 0; i < countOfAliveEnemies; i++){
            elfEnemyData.levelOfUnit = Random.Range(0, levelOfUnit);
            enemiesFactory.CreateAndDirectEnemy(elfEnemyData);
        }
    }

    private void StartUndeadWave(int level){
        WaveFXUndead.Play();
        countOfAliveEnemies = (int) (level * 0.3f);
        EnemyData undeadEnemyData = new EnemyData(2, 0);
        int levelOfUnit = Mathf.Clamp(level - 20, 0, 7);
        for (int i = 0; i < countOfAliveEnemies; i++){
            undeadEnemyData.levelOfUnit = Random.Range(0, levelOfUnit);
            enemiesFactory.CreateAndDirectEnemy(undeadEnemyData);
        }
    }

    private void StartOrcWave(int level){
        WaveFXOrc.Play();
        countOfAliveEnemies = (int) (level * 0.2f);
        EnemyData orcEnemyData = new EnemyData(3, 0);
        int levelOfUnit = Mathf.Clamp(level - 30, 0, 7);
        for (int i = 0; i < countOfAliveEnemies; i++){
            orcEnemyData.levelOfUnit = Random.Range(0, levelOfUnit);
            enemiesFactory.CreateAndDirectEnemy(orcEnemyData);
        }
    }

    private void StartLastMonstersWave(int level){
        var x = Random.Range(0, 4);
        switch (x){
            case 0:
                WaveFXHuman.Play();
                break;
            case 1:
                WaveFXElf.Play();
                break;
            case 2:
                WaveFXUndead.Play();
                break;
            case 3:
                WaveFXOrc.Play();
                break;
        }

        countOfAliveEnemies = 10;
        EnemyData enemyData = new EnemyData(4, 0);
        for (int i = 0; i < countOfAliveEnemies; i++){
            enemyData.levelOfUnit = Random.Range(0, 28);
            enemiesFactory.CreateAndDirectEnemy(enemyData);
        }
    }

    public void OnDieEnemy(){
        if (state == GameState.Playing){
            countOfAliveEnemies--;
            if (countOfAliveEnemies == 0){
                _currentGameLevel++;
                SaveProgressYandex();

                StartLevel(_currentGameLevel);
            }
        }
    }


    public void RestartLevel(){
        if (state == GameState.Stop){
            if (CurrentGameLevel > 11 && YandexGame.EnvironmentData.reviewCanShow){
                YandexGame.ReviewShow(false);

                Time.timeScale = 0;
                AudioManager.Instance.AudioSource.Pause();


                YandexGame.ReviewSentEvent += _ => {
                    Time.timeScale = 1;
                    AudioManager.Instance.AudioSource.UnPause();
                };
            }

            SaveProgressYandex();
            YandexGame.FullscreenShow();

            restartButton.image.color = Color.gray;
            restartButton.onClick.RemoveListener(RestartLevel);
            StartCoroutine(RestartLevelCoroutine());
        }
    }

    private void SaveProgressYandex(){
        _saveablesObjects.WriteAllDataToContainer();

        YandexGame.savesData.gold = _dataContainer.gold;
        YandexGame.savesData.cellsInformation = _dataContainer.cellsInformation;
        YandexGame.savesData.currentLevel = _dataContainer.currentLevel;
        YandexGame.savesData.priceHealth = _dataContainer.priceHealth;
        YandexGame.savesData.priceWeapon = _dataContainer.priceWeapon;
        YandexGame.savesData.levelShopWeapon = _dataContainer.levelShopWeapon;
        YandexGame.savesData.maxHealthKing = _dataContainer.maxHealthKing;
        YandexGame.savesData.maxLevelOfCreatedWeapon = _dataContainer.maxLevelOfCreatedWeapon;

        YandexGame.SaveProgress();
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