using UnityEngine;

public class Cells : MonoBehaviour{
    private Cell[] _arrayCells;

   

    private void Awake(){
        _arrayCells = GetComponentsInChildren<Cell>();
    }

    // public void BuyAWeapon(){
    //     for (int i = 0; i < _arrayCells.Length; i++){
    //         if (_arrayCells[i].IsAvailable()){
    //             RocketLauncher rocketLauncher = Instantiate(rocketLauncherPrefab);
    //             _arrayCells[i].SetAWeapon(rocketLauncher);
    //             _arrayCells[i].MakeOccupied();
    //             return;
    //         }
    //     }
    // }
}