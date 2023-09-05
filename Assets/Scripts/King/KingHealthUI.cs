using UnityEngine;
using UnityEngine.UI;

public class KingHealthUI : MonoBehaviour{
    [SerializeField] King king;
    [SerializeField] private Image currentHealthBar;

    private void Start(){
        king.UIHealthEvent += UpdateUIHealth;
        UpdateUIHealth();
    }

    private void UpdateUIHealth(){
        currentHealthBar.fillAmount = king.GetPercentageOfHealth();
    }
}