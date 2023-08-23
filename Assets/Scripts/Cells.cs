using UnityEngine;

public class Cells : MonoBehaviour{
    private Cell[] _arrayCells;


    private void Awake(){
        _arrayCells = GetComponentsInChildren<Cell>();
    }

    public Cell TryGetAvailableCell(){
        foreach (var cell in _arrayCells){
            if (cell.IsAvailable()){
                return cell;
            }
        }

        return null;
    }
}