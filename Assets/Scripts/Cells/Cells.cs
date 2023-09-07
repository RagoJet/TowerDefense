using UnityEngine;

public class Cells : MonoBehaviour{
    private Cell[] _arrayCells;

    private void Start(){
        _arrayCells = GetComponentsInChildren<Cell>();
    }

    public bool TryGetCell(out Cell theCell){
        foreach (var cell in _arrayCells){
            if (!cell.IsAvailable()) continue;
            theCell = cell;
            return true;
        }

        theCell = null;
        return false;
    }
}