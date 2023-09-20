using UnityEngine;

public class Cell : MonoBehaviour{
    private bool _available = true;
    private Weapon _weapon;
    [SerializeField] private Transform cellTransform;

    public Vector3 GetPosition(){
        return cellTransform.position;
    }

    public Weapon GetWeapon(){
        return _weapon;
    }

    public bool IsAvailable(){
        return _available;
    }

    public void MakeAvailable(){
        _weapon = null;
        _available = true;
    }

    public void MakeOccupied(Weapon weapon){
        _weapon = weapon;
        _available = false;
    }
}