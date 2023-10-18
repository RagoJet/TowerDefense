using TMPro;
using UnityEngine;
using YG;

public class Shop : MonoBehaviour, ISaveable{
    WeaponsFactory weaponsFactory;
    private King king;

    private int _gold;
    private int _priceWeapon;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI priceWeaponText;


    private int _priceHealth;
    [SerializeField] private TextMeshProUGUI priceHealthText;

    private int _goldForAds;
    [SerializeField] private TextMeshProUGUI goldForAdsText;

    private DataContainer _dataContainer;

    [SerializeField] private TextMeshProUGUI restartText;
    [SerializeField] private TextMeshProUGUI adsText;
    [SerializeField] TextMeshProUGUI buyWeaponText;
    [SerializeField] TextMeshProUGUI buyHealthText;

    public static Shop Instance{ get; private set; }

    private void Awake(){
        Instance = this;
    }

    public void Construct(WeaponsFactory weaponsFactory, King king, string language){
        if (language.Equals("en")){
            restartText.text = "Restart";
            adsText.text = "Watch ad";
            buyHealthText.text = "Health";
            buyWeaponText.text = "Weapon";
        }
        else if (language.Equals("ru")){
            restartText.text = "Рестарт";
            adsText.text = "Смотреть рекламу";
            buyHealthText.text = "Жизни";
            buyWeaponText.text = "Оружие";
        }

        this.weaponsFactory = weaponsFactory;
        this.king = king;
        UpdateGoldUI();
        UpdatePriceHealthUI();
        UpdatePriceWeaponUI();
        UpdateGoldForAdsUI();
    }


    public void TryBuyWeapon(){
        if (_gold < _priceWeapon) return;
        if (weaponsFactory.TryCreateWeapon()){
            _gold -= _priceWeapon;
            _priceWeapon += 1;
            UpdateGoldUI();
            UpdatePriceWeaponUI();
        }
    }

    public void TryBuyHealth(){
        if (_gold < _priceHealth) return;
        king.AddHealth((int) (_priceHealth * 0.5f));
        _gold -= _priceHealth;
        _priceHealth += 5;
        UpdateGoldUI();
        UpdatePriceHealthUI();
        UpdateGoldForAdsUI();
    }


    public void TryWatchAds(){
        YandexGame.RewVideoShow(0);
    }

    public void AddGoldFromAd(){
        _gold += _goldForAds;
        UpdateGoldUI();
    }

    private void UpdateGoldForAdsUI(){
        _goldForAds = _priceHealth * 3;
        goldForAdsText.text = "+" + _goldForAds;
    }

    private void UpdatePriceHealthUI(){
        priceHealthText.text = _priceHealth.ToString();
    }

    private void UpdatePriceWeaponUI(){
        priceWeaponText.text = _priceWeapon.ToString();
    }

    private void UpdateGoldUI(){
        goldText.text = _gold.ToString();
    }

    public void AddGoldFromEnemy(int newGold){
        _gold += newGold;
        UpdateGoldUI();
        AudioManager.Instance.PlayGoldDeathEnemySound();
    }

    public void WriteDataToContainer(){
        _dataContainer.gold = _gold;
        _dataContainer.priceHealth = _priceHealth;
        _dataContainer.priceWeapon = _priceWeapon;
    }

    public void LoadDataFromContainer(){
        _gold = _dataContainer.gold;
        _priceHealth = _dataContainer.priceHealth;
        _priceWeapon = _dataContainer.priceWeapon;
    }

    public void SetDataContainer(DataContainer dataContainer){
        _dataContainer = dataContainer;
    }
}