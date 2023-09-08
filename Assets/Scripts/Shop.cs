using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour{
    [SerializeField] WeaponsFactory weaponsFactory;
    [SerializeField] private King king;

    [SerializeField] private int gold = 100;
    [SerializeField] private int priceWeapon = 100;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI priceWeaponText;


    [SerializeField] private int priceHealth = 100;
    [SerializeField] private TextMeshProUGUI priceHealthText;

    [SerializeField] private int priceDamage = 100;
    [SerializeField] private TextMeshProUGUI priceDamageText;


    private void Start(){
        UpdateGoldUI();
        UpdatePriceHealthUI();
        UpdatePriceWeaponUI();
        UpdatePriceDamagehUI();
    }

    public void TryBuyHealth(){
        if (gold < priceHealth) return;
        king.AddHealth((int) (priceHealth * 0.1f));
        gold -= priceHealth;
        priceHealth += 10;
        UpdateGoldUI();
        UpdatePriceHealthUI();
    }

    public void TryBuyWeapon(){
        if (gold < priceWeapon) return;
        if (weaponsFactory.TryCreateWeapon()){
            gold -= priceWeapon;
            priceWeapon += 1;
            UpdateGoldUI();
            UpdatePriceWeaponUI();
        }
    }

    public void TryBuyDamage(){
        if (gold >= priceDamage){
            gold -= priceDamage;
            king.AddDamage((int) (priceDamage * 0.01f));
            priceDamage += 10;
            UpdateGoldUI();
            UpdatePriceDamagehUI();
        }
    }

    private void UpdatePriceDamagehUI(){
        priceDamageText.text = priceDamage.ToString();
    }

    private void UpdatePriceHealthUI(){
        priceHealthText.text = priceHealth.ToString();
    }

    private void UpdatePriceWeaponUI(){
        priceWeaponText.text = priceWeapon.ToString();
    }

    private void UpdateGoldUI(){
        goldText.text = "Gold: " + gold;
    }

    public void AddGoldFromEnemy(int newGold){
        gold += newGold;
        UpdateGoldUI();
    }
}