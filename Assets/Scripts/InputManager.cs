using UnityEngine;

public class InputManager : MonoBehaviour{
    [SerializeField] private Camera GameCamera;

    private Ray _ray;
    private RaycastHit _hit;

    private void Update(){
        if (Input.GetMouseButtonDown(0)){
            _ray = GameCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit)){
                if (_hit.collider.TryGetComponent(out Cell cell)){
                    Debug.Log("eee");
                }
            }
        }
    }
}