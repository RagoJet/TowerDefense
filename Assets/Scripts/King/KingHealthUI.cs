using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KingHealthUI : MonoBehaviour{
    King king;
    [SerializeField] private Image currentHealthBar;

    [SerializeField] private TextMeshProUGUI healthBarText;


    public void Construct(King king){
        this.king = king;
        king.UIHealthEvent += UpdateUIHealth;
        UpdateUIHealth();
    }

    private void UpdateUIHealth(){
        currentHealthBar.DOFillAmount(king.GetPercentageOfHealth(), 0.5f).SetEase(Ease.OutExpo);
        healthBarText.text = king.GetStringHealthAndMaxHealth();
    }
}