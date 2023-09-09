using UnityEngine;

public class Cell : MonoBehaviour{
    private bool _available = true;
    private Weapon _weaponOfThisCell;
    [SerializeField] private Transform cellTransform;

    public Vector3 GetPosition(){
        return cellTransform.position;
    }

    public Weapon GetWeaponOfThisCell(){
        return _weaponOfThisCell;
    }

    public bool IsAvailable(){
        return _available;
    }

    public void MakeAvailable(){
        _weaponOfThisCell = null;
        _available = true;
    }

    public void MakeOccupied(Weapon weapon){
        _weaponOfThisCell = weapon;
        _available = false;
    }
}