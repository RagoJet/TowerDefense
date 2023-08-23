using UnityEngine;

public class Island : MonoBehaviour{
    private Collider _collider;

    private void Awake(){
        _collider = GetComponent<Collider>();
    }
}