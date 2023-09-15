using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour, ISaveable{
    WeaponsFactory weaponsFactory;
    private King king;

    private int _gold;
    private int _priceWeapon;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI priceWeaponText;


    private int _priceHealth;
    [SerializeField] private TextMeshProUGUI priceHealthText;

    private int _priceDamage;
    [SerializeField] private TextMeshProUGUI priceDamageText;

    private DataContainer _dataContainer;

    public void Construct(WeaponsFactory weaponsFactory, King king){
        this.weaponsFactory = weaponsFactory;
        this.king = king;
        UpdateGoldUI();
        UpdatePriceHealthUI();
        UpdatePriceWeaponUI();
        UpdatePriceDamagehUI();
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
        king.AddHealth((int) (_priceHealth * 0.1f));
        _gold -= _priceHealth;
        _priceHealth += 10;
        UpdateGoldUI();
        UpdatePriceHealthUI();
    }

    public void TryBuyDamage(){
        if (_gold >= _priceDamage){
            _gold -= _priceDamage;
            king.AddDamage((int) (_priceDamage * 0.01f));
            _priceDamage += 10;
            UpdateGoldUI();
            UpdatePriceDamagehUI();
        }
    }

    private void UpdatePriceDamagehUI(){
        priceDamageText.text = "Damage price:" + _priceDamage;
    }

    private void UpdatePriceHealthUI(){
        priceHealthText.text = "Health price:" + _priceHealth;
    }

    private void UpdatePriceWeaponUI(){
        priceWeaponText.text = "Weapon price:" + _priceWeapon;
    }

    private void UpdateGoldUI(){
        goldText.text = "Gold: " + _gold;
    }

    public void AddGoldFromEnemy(int newGold){
        _gold += newGold;
        UpdateGoldUI();
    }

    public void WriteDataToContainer(){
        _dataContainer.gold = _gold;
        _dataContainer.priceHealth = _priceHealth;
        _dataContainer.priceDamage = _priceDamage;
        _dataContainer.priceWeapon = _priceWeapon;
    }

    public void LoadDataFromContainer(){
        _gold = _dataContainer.gold;
        _priceHealth = _dataContainer.priceHealth;
        _priceDamage = _dataContainer.priceDamage;
        _priceWeapon = _dataContainer.priceWeapon;
    }

    public void SetDataContainer(DataContainer dataContainer){
        _dataContainer = dataContainer;
    }
}