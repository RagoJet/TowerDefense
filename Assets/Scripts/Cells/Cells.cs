using UnityEngine;

public class Cells : MonoBehaviour{
    public Cell[] _arrayCells;


    private void Awake(){
        _arrayCells = GetComponentsInChildren<Cell>();
    }

    public bool TryGetCell(out Cell theCell){
        foreach (var cell in _arrayCells){
            if (!cell.IsAvailable()) continue;
            theCell = cell;
            return true;
        }

        if (AreAllFirstLevelWeapons()){
            MakeFirstLevelWeaponsShine();
        }

        theCell = null;
        return false;
    }

    private void MakeFirstLevelWeaponsShine(){
        for (int i = 0; i < _arrayCells.Length; i++){
            _arrayCells[i].GetWeapon().MakeShine();
        }
    }

    private bool AreAllFirstLevelWeapons(){
        for (int i = 0; i < _arrayCells.Length; i++){
            if (_arrayCells[i].GetWeapon().GetLevel() != 1){
                return false;
            }
        }

        return true;
    }


    public void SaveDataToContainer(DataContainer dataContainer){
        dataContainer.cellsInformation.Clear();
        for (int i = 0; i < _arrayCells.Length; i++){
            if (!_arrayCells[i].IsAvailable()){
                dataContainer.cellsInformation.Add(new CellsInformation(i, _arrayCells[i].GetWeapon().GetLevel()));
            }
        }
    }
}