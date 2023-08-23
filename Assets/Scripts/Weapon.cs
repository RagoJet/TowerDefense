using UnityEngine;



public class Weapon : MonoBehaviour{
    private WeaponDescription _description;
    private Cell _cell;

    public void OccupyTheCage(Cell cell){
        _cell = cell;
        transform.position = _cell.GetPosition();
        cell.MakeOccupied();
    }

    public void FreeTheCell(){
        _cell.MakeAvailable();
        _cell = null;
    }

    public void Construct(WeaponDescription description){
        _description = description;
    }

    public DataWeapon GetDataWeapon(){
        return _description._dataWeapon;
    }
}