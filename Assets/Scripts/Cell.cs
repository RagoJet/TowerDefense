using UnityEngine;

public class Cell : MonoBehaviour{
    private bool _available = true;
    [SerializeField] private Transform cellTransform;

    public Vector3 GetPosition(){
        return cellTransform.position;
    }

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