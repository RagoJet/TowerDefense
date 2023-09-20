﻿using TMPro;
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

    private int _goldForAds;
    [SerializeField] private TextMeshProUGUI goldForAdsText;

    private DataContainer _dataContainer;

    public void Construct(WeaponsFactory weaponsFactory, King king){
        this.weaponsFactory = weaponsFactory;
        this.king = king;
        UpdateGoldUI();
        UpdatePriceHealthUI();
        UpdatePriceWeaponUI();
        UpdateGoldForAdsUI();
    }

    private void UpdateGoldForAdsUI(){
        _goldForAds = (int) (_priceHealth * 2.5f);
        goldForAdsText.text = _goldForAds.ToString();
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
        // watch ads
        _gold += _goldForAds;
        UpdateGoldUI();
    }

    private void UpdatePriceHealthUI(){
        priceHealthText.text = "Цена здоровья: " + _priceHealth;
    }

    private void UpdatePriceWeaponUI(){
        priceWeaponText.text = "Цена оружия: " + _priceWeapon;
    }

    private void UpdateGoldUI(){
        goldText.text = "Золото:\n" + _gold;
    }

    public void AddGoldFromEnemy(int newGold){
        _gold += newGold;
        UpdateGoldUI();
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