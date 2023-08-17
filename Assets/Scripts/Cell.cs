using UnityEngine;

public class Cell : MonoBehaviour{
    private bool _available = true;
    [SerializeField] private Transform turretTransform;


    // public void SetAWeapon(Weapon weapon){
    //     weapon.transform.position = turretTransform.position;
    // }

    public bool IsAvailable(){
        return _available;
    }

    public void MakeAvailable(){
        _available = true;
    }

    public void MakeOccupied(){
        _available = false;
    }
}